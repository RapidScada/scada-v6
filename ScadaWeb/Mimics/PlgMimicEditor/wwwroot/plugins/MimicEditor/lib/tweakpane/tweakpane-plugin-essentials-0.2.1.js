function forceCast(v) {
    return v;
}
function isEmpty(value) {
    return value === null || value === undefined;
}
function isObject$1(value) {
    return value !== null && typeof value === 'object';
}
function isRecord(value) {
    return value !== null && typeof value === 'object';
}
function deepEqualsArray(a1, a2) {
    if (a1.length !== a2.length) {
        return false;
    }
    for (let i = 0; i < a1.length; i++) {
        if (a1[i] !== a2[i]) {
            return false;
        }
    }
    return true;
}
function deepMerge(r1, r2) {
    const keys = Array.from(new Set([...Object.keys(r1), ...Object.keys(r2)]));
    return keys.reduce((result, key) => {
        const v1 = r1[key];
        const v2 = r2[key];
        return isRecord(v1) && isRecord(v2)
            ? Object.assign(Object.assign({}, result), { [key]: deepMerge(v1, v2) }) : Object.assign(Object.assign({}, result), { [key]: key in r2 ? v2 : v1 });
    }, {});
}

function isBinding(value) {
    if (!isObject$1(value)) {
        return false;
    }
    return 'target' in value;
}

const CREATE_MESSAGE_MAP = {
    alreadydisposed: () => 'View has been already disposed',
    invalidparams: (context) => `Invalid parameters for '${context.name}'`,
    nomatchingcontroller: (context) => `No matching controller for '${context.key}'`,
    nomatchingview: (context) => `No matching view for '${JSON.stringify(context.params)}'`,
    notbindable: () => `Value is not bindable`,
    notcompatible: (context) => `Not compatible with  plugin '${context.id}'`,
    propertynotfound: (context) => `Property '${context.name}' not found`,
    shouldneverhappen: () => 'This error should never happen',
};
class TpError {
    static alreadyDisposed() {
        return new TpError({ type: 'alreadydisposed' });
    }
    static notBindable() {
        return new TpError({
            type: 'notbindable',
        });
    }
    static notCompatible(bundleId, id) {
        return new TpError({
            type: 'notcompatible',
            context: {
                id: `${bundleId}.${id}`,
            },
        });
    }
    static propertyNotFound(name) {
        return new TpError({
            type: 'propertynotfound',
            context: {
                name: name,
            },
        });
    }
    static shouldNeverHappen() {
        return new TpError({ type: 'shouldneverhappen' });
    }
    constructor(config) {
        var _a;
        this.message =
            (_a = CREATE_MESSAGE_MAP[config.type](forceCast(config.context))) !== null && _a !== void 0 ? _a : 'Unexpected error';
        this.name = this.constructor.name;
        this.stack = new Error(this.message).stack;
        this.type = config.type;
    }
    toString() {
        return this.message;
    }
}

class BindingTarget {
    constructor(obj, key) {
        this.obj_ = obj;
        this.key = key;
    }
    static isBindable(obj) {
        if (obj === null) {
            return false;
        }
        if (typeof obj !== 'object' && typeof obj !== 'function') {
            return false;
        }
        return true;
    }
    read() {
        return this.obj_[this.key];
    }
    write(value) {
        this.obj_[this.key] = value;
    }
    writeProperty(name, value) {
        const valueObj = this.read();
        if (!BindingTarget.isBindable(valueObj)) {
            throw TpError.notBindable();
        }
        if (!(name in valueObj)) {
            throw TpError.propertyNotFound(name);
        }
        valueObj[name] = value;
    }
}

class Emitter {
    constructor() {
        this.observers_ = {};
    }
    on(eventName, handler) {
        let observers = this.observers_[eventName];
        if (!observers) {
            observers = this.observers_[eventName] = [];
        }
        observers.push({
            handler: handler,
        });
        return this;
    }
    off(eventName, handler) {
        const observers = this.observers_[eventName];
        if (observers) {
            this.observers_[eventName] = observers.filter((observer) => {
                return observer.handler !== handler;
            });
        }
        return this;
    }
    emit(eventName, event) {
        const observers = this.observers_[eventName];
        if (!observers) {
            return;
        }
        observers.forEach((observer) => {
            observer.handler(event);
        });
    }
}

class ComplexValue {
    constructor(initialValue, config) {
        var _a;
        this.constraint_ = config === null || config === void 0 ? void 0 : config.constraint;
        this.equals_ = (_a = config === null || config === void 0 ? void 0 : config.equals) !== null && _a !== void 0 ? _a : ((v1, v2) => v1 === v2);
        this.emitter = new Emitter();
        this.rawValue_ = initialValue;
    }
    get constraint() {
        return this.constraint_;
    }
    get rawValue() {
        return this.rawValue_;
    }
    set rawValue(rawValue) {
        this.setRawValue(rawValue, {
            forceEmit: false,
            last: true,
        });
    }
    setRawValue(rawValue, options) {
        const opts = options !== null && options !== void 0 ? options : {
            forceEmit: false,
            last: true,
        };
        const constrainedValue = this.constraint_
            ? this.constraint_.constrain(rawValue)
            : rawValue;
        const prevValue = this.rawValue_;
        const changed = !this.equals_(prevValue, constrainedValue);
        if (!changed && !opts.forceEmit) {
            return;
        }
        this.emitter.emit('beforechange', {
            sender: this,
        });
        this.rawValue_ = constrainedValue;
        this.emitter.emit('change', {
            options: opts,
            previousRawValue: prevValue,
            rawValue: constrainedValue,
            sender: this,
        });
    }
}

class PrimitiveValue {
    constructor(initialValue) {
        this.emitter = new Emitter();
        this.value_ = initialValue;
    }
    get rawValue() {
        return this.value_;
    }
    set rawValue(value) {
        this.setRawValue(value, {
            forceEmit: false,
            last: true,
        });
    }
    setRawValue(value, options) {
        const opts = options !== null && options !== void 0 ? options : {
            forceEmit: false,
            last: true,
        };
        const prevValue = this.value_;
        if (prevValue === value && !opts.forceEmit) {
            return;
        }
        this.emitter.emit('beforechange', {
            sender: this,
        });
        this.value_ = value;
        this.emitter.emit('change', {
            options: opts,
            previousRawValue: prevValue,
            rawValue: this.value_,
            sender: this,
        });
    }
}

class ReadonlyPrimitiveValue {
    constructor(value) {
        this.emitter = new Emitter();
        this.onValueBeforeChange_ = this.onValueBeforeChange_.bind(this);
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.value_ = value;
        this.value_.emitter.on('beforechange', this.onValueBeforeChange_);
        this.value_.emitter.on('change', this.onValueChange_);
    }
    get rawValue() {
        return this.value_.rawValue;
    }
    onValueBeforeChange_(ev) {
        this.emitter.emit('beforechange', Object.assign(Object.assign({}, ev), { sender: this }));
    }
    onValueChange_(ev) {
        this.emitter.emit('change', Object.assign(Object.assign({}, ev), { sender: this }));
    }
}

function createValue(initialValue, config) {
    const constraint = config === null || config === void 0 ? void 0 : config.constraint;
    const equals = config === null || config === void 0 ? void 0 : config.equals;
    if (!constraint && !equals) {
        return new PrimitiveValue(initialValue);
    }
    return new ComplexValue(initialValue, config);
}
function createReadonlyValue(value) {
    return [
        new ReadonlyPrimitiveValue(value),
        (rawValue, options) => {
            value.setRawValue(rawValue, options);
        },
    ];
}

class ValueMap {
    constructor(valueMap) {
        this.emitter = new Emitter();
        this.valMap_ = valueMap;
        for (const key in this.valMap_) {
            const v = this.valMap_[key];
            v.emitter.on('change', () => {
                this.emitter.emit('change', {
                    key: key,
                    sender: this,
                });
            });
        }
    }
    static createCore(initialValue) {
        const keys = Object.keys(initialValue);
        return keys.reduce((o, key) => {
            return Object.assign(o, {
                [key]: createValue(initialValue[key]),
            });
        }, {});
    }
    static fromObject(initialValue) {
        const core = this.createCore(initialValue);
        return new ValueMap(core);
    }
    get(key) {
        return this.valMap_[key].rawValue;
    }
    set(key, value) {
        this.valMap_[key].rawValue = value;
    }
    value(key) {
        return this.valMap_[key];
    }
}

class DefiniteRangeConstraint {
    constructor(config) {
        this.values = ValueMap.fromObject({
            max: config.max,
            min: config.min,
        });
    }
    constrain(value) {
        const max = this.values.get('max');
        const min = this.values.get('min');
        return Math.min(Math.max(value, min), max);
    }
}

class RangeConstraint {
    constructor(config) {
        this.values = ValueMap.fromObject({
            max: config.max,
            min: config.min,
        });
    }
    constrain(value) {
        const max = this.values.get('max');
        const min = this.values.get('min');
        let result = value;
        if (!isEmpty(min)) {
            result = Math.max(result, min);
        }
        if (!isEmpty(max)) {
            result = Math.min(result, max);
        }
        return result;
    }
}

class StepConstraint {
    constructor(step, origin = 0) {
        this.step = step;
        this.origin = origin;
    }
    constrain(value) {
        const o = this.origin % this.step;
        const r = Math.round((value - o) / this.step);
        return o + r * this.step;
    }
}

class NumberLiteralNode {
    constructor(text) {
        this.text = text;
    }
    evaluate() {
        return Number(this.text);
    }
    toString() {
        return this.text;
    }
}
const BINARY_OPERATION_MAP = {
    '**': (v1, v2) => Math.pow(v1, v2),
    '*': (v1, v2) => v1 * v2,
    '/': (v1, v2) => v1 / v2,
    '%': (v1, v2) => v1 % v2,
    '+': (v1, v2) => v1 + v2,
    '-': (v1, v2) => v1 - v2,
    '<<': (v1, v2) => v1 << v2,
    '>>': (v1, v2) => v1 >> v2,
    '>>>': (v1, v2) => v1 >>> v2,
    '&': (v1, v2) => v1 & v2,
    '^': (v1, v2) => v1 ^ v2,
    '|': (v1, v2) => v1 | v2,
};
class BinaryOperationNode {
    constructor(operator, left, right) {
        this.left = left;
        this.operator = operator;
        this.right = right;
    }
    evaluate() {
        const op = BINARY_OPERATION_MAP[this.operator];
        if (!op) {
            throw new Error(`unexpected binary operator: '${this.operator}`);
        }
        return op(this.left.evaluate(), this.right.evaluate());
    }
    toString() {
        return [
            'b(',
            this.left.toString(),
            this.operator,
            this.right.toString(),
            ')',
        ].join(' ');
    }
}
const UNARY_OPERATION_MAP = {
    '+': (v) => v,
    '-': (v) => -v,
    '~': (v) => ~v,
};
class UnaryOperationNode {
    constructor(operator, expr) {
        this.operator = operator;
        this.expression = expr;
    }
    evaluate() {
        const op = UNARY_OPERATION_MAP[this.operator];
        if (!op) {
            throw new Error(`unexpected unary operator: '${this.operator}`);
        }
        return op(this.expression.evaluate());
    }
    toString() {
        return ['u(', this.operator, this.expression.toString(), ')'].join(' ');
    }
}

function combineReader(parsers) {
    return (text, cursor) => {
        for (let i = 0; i < parsers.length; i++) {
            const result = parsers[i](text, cursor);
            if (result !== '') {
                return result;
            }
        }
        return '';
    };
}
function readWhitespace(text, cursor) {
    var _a;
    const m = text.substr(cursor).match(/^\s+/);
    return (_a = (m && m[0])) !== null && _a !== void 0 ? _a : '';
}
function readNonZeroDigit(text, cursor) {
    const ch = text.substr(cursor, 1);
    return ch.match(/^[1-9]$/) ? ch : '';
}
function readDecimalDigits(text, cursor) {
    var _a;
    const m = text.substr(cursor).match(/^[0-9]+/);
    return (_a = (m && m[0])) !== null && _a !== void 0 ? _a : '';
}
function readSignedInteger(text, cursor) {
    const ds = readDecimalDigits(text, cursor);
    if (ds !== '') {
        return ds;
    }
    const sign = text.substr(cursor, 1);
    cursor += 1;
    if (sign !== '-' && sign !== '+') {
        return '';
    }
    const sds = readDecimalDigits(text, cursor);
    if (sds === '') {
        return '';
    }
    return sign + sds;
}
function readExponentPart(text, cursor) {
    const e = text.substr(cursor, 1);
    cursor += 1;
    if (e.toLowerCase() !== 'e') {
        return '';
    }
    const si = readSignedInteger(text, cursor);
    if (si === '') {
        return '';
    }
    return e + si;
}
function readDecimalIntegerLiteral(text, cursor) {
    const ch = text.substr(cursor, 1);
    if (ch === '0') {
        return ch;
    }
    const nzd = readNonZeroDigit(text, cursor);
    cursor += nzd.length;
    if (nzd === '') {
        return '';
    }
    return nzd + readDecimalDigits(text, cursor);
}
function readDecimalLiteral1(text, cursor) {
    const dil = readDecimalIntegerLiteral(text, cursor);
    cursor += dil.length;
    if (dil === '') {
        return '';
    }
    const dot = text.substr(cursor, 1);
    cursor += dot.length;
    if (dot !== '.') {
        return '';
    }
    const dds = readDecimalDigits(text, cursor);
    cursor += dds.length;
    return dil + dot + dds + readExponentPart(text, cursor);
}
function readDecimalLiteral2(text, cursor) {
    const dot = text.substr(cursor, 1);
    cursor += dot.length;
    if (dot !== '.') {
        return '';
    }
    const dds = readDecimalDigits(text, cursor);
    cursor += dds.length;
    if (dds === '') {
        return '';
    }
    return dot + dds + readExponentPart(text, cursor);
}
function readDecimalLiteral3(text, cursor) {
    const dil = readDecimalIntegerLiteral(text, cursor);
    cursor += dil.length;
    if (dil === '') {
        return '';
    }
    return dil + readExponentPart(text, cursor);
}
const readDecimalLiteral = combineReader([
    readDecimalLiteral1,
    readDecimalLiteral2,
    readDecimalLiteral3,
]);
function parseBinaryDigits(text, cursor) {
    var _a;
    const m = text.substr(cursor).match(/^[01]+/);
    return (_a = (m && m[0])) !== null && _a !== void 0 ? _a : '';
}
function readBinaryIntegerLiteral(text, cursor) {
    const prefix = text.substr(cursor, 2);
    cursor += prefix.length;
    if (prefix.toLowerCase() !== '0b') {
        return '';
    }
    const bds = parseBinaryDigits(text, cursor);
    if (bds === '') {
        return '';
    }
    return prefix + bds;
}
function readOctalDigits(text, cursor) {
    var _a;
    const m = text.substr(cursor).match(/^[0-7]+/);
    return (_a = (m && m[0])) !== null && _a !== void 0 ? _a : '';
}
function readOctalIntegerLiteral(text, cursor) {
    const prefix = text.substr(cursor, 2);
    cursor += prefix.length;
    if (prefix.toLowerCase() !== '0o') {
        return '';
    }
    const ods = readOctalDigits(text, cursor);
    if (ods === '') {
        return '';
    }
    return prefix + ods;
}
function readHexDigits(text, cursor) {
    var _a;
    const m = text.substr(cursor).match(/^[0-9a-f]+/i);
    return (_a = (m && m[0])) !== null && _a !== void 0 ? _a : '';
}
function readHexIntegerLiteral(text, cursor) {
    const prefix = text.substr(cursor, 2);
    cursor += prefix.length;
    if (prefix.toLowerCase() !== '0x') {
        return '';
    }
    const hds = readHexDigits(text, cursor);
    if (hds === '') {
        return '';
    }
    return prefix + hds;
}
const readNonDecimalIntegerLiteral = combineReader([
    readBinaryIntegerLiteral,
    readOctalIntegerLiteral,
    readHexIntegerLiteral,
]);
const readNumericLiteral = combineReader([
    readNonDecimalIntegerLiteral,
    readDecimalLiteral,
]);

function parseLiteral(text, cursor) {
    const num = readNumericLiteral(text, cursor);
    cursor += num.length;
    if (num === '') {
        return null;
    }
    return {
        evaluable: new NumberLiteralNode(num),
        cursor: cursor,
    };
}
function parseParenthesizedExpression(text, cursor) {
    const op = text.substr(cursor, 1);
    cursor += op.length;
    if (op !== '(') {
        return null;
    }
    const expr = parseExpression(text, cursor);
    if (!expr) {
        return null;
    }
    cursor = expr.cursor;
    cursor += readWhitespace(text, cursor).length;
    const cl = text.substr(cursor, 1);
    cursor += cl.length;
    if (cl !== ')') {
        return null;
    }
    return {
        evaluable: expr.evaluable,
        cursor: cursor,
    };
}
function parsePrimaryExpression(text, cursor) {
    var _a;
    return ((_a = parseLiteral(text, cursor)) !== null && _a !== void 0 ? _a : parseParenthesizedExpression(text, cursor));
}
function parseUnaryExpression(text, cursor) {
    const expr = parsePrimaryExpression(text, cursor);
    if (expr) {
        return expr;
    }
    const op = text.substr(cursor, 1);
    cursor += op.length;
    if (op !== '+' && op !== '-' && op !== '~') {
        return null;
    }
    const num = parseUnaryExpression(text, cursor);
    if (!num) {
        return null;
    }
    cursor = num.cursor;
    return {
        cursor: cursor,
        evaluable: new UnaryOperationNode(op, num.evaluable),
    };
}
function readBinaryOperator(ops, text, cursor) {
    cursor += readWhitespace(text, cursor).length;
    const op = ops.filter((op) => text.startsWith(op, cursor))[0];
    if (!op) {
        return null;
    }
    cursor += op.length;
    cursor += readWhitespace(text, cursor).length;
    return {
        cursor: cursor,
        operator: op,
    };
}
function createBinaryOperationExpressionParser(exprParser, ops) {
    return (text, cursor) => {
        const firstExpr = exprParser(text, cursor);
        if (!firstExpr) {
            return null;
        }
        cursor = firstExpr.cursor;
        let expr = firstExpr.evaluable;
        for (;;) {
            const op = readBinaryOperator(ops, text, cursor);
            if (!op) {
                break;
            }
            cursor = op.cursor;
            const nextExpr = exprParser(text, cursor);
            if (!nextExpr) {
                return null;
            }
            cursor = nextExpr.cursor;
            expr = new BinaryOperationNode(op.operator, expr, nextExpr.evaluable);
        }
        return expr
            ? {
                cursor: cursor,
                evaluable: expr,
            }
            : null;
    };
}
const parseBinaryOperationExpression = [
    ['**'],
    ['*', '/', '%'],
    ['+', '-'],
    ['<<', '>>>', '>>'],
    ['&'],
    ['^'],
    ['|'],
].reduce((parser, ops) => {
    return createBinaryOperationExpressionParser(parser, ops);
}, parseUnaryExpression);
function parseExpression(text, cursor) {
    cursor += readWhitespace(text, cursor).length;
    return parseBinaryOperationExpression(text, cursor);
}
function parseEcmaNumberExpression(text) {
    const expr = parseExpression(text, 0);
    if (!expr) {
        return null;
    }
    const cursor = expr.cursor + readWhitespace(text, expr.cursor).length;
    if (cursor !== text.length) {
        return null;
    }
    return expr.evaluable;
}

function parseNumber(text) {
    var _a;
    const r = parseEcmaNumberExpression(text);
    return (_a = r === null || r === void 0 ? void 0 : r.evaluate()) !== null && _a !== void 0 ? _a : null;
}
function numberFromUnknown(value) {
    if (typeof value === 'number') {
        return value;
    }
    if (typeof value === 'string') {
        const pv = parseNumber(value);
        if (!isEmpty(pv)) {
            return pv;
        }
    }
    return 0;
}
function createNumberFormatter(digits) {
    return (value) => {
        return value.toFixed(Math.max(Math.min(digits, 20), 0));
    };
}

function mapRange(value, start1, end1, start2, end2) {
    const p = (value - start1) / (end1 - start1);
    return start2 + p * (end2 - start2);
}
function getDecimalDigits(value) {
    const text = String(value.toFixed(10));
    const frac = text.split('.')[1];
    return frac.replace(/0+$/, '').length;
}
function constrainRange(value, min, max) {
    return Math.min(Math.max(value, min), max);
}
function loopRange(value, max) {
    return ((value % max) + max) % max;
}
function getSuitableDecimalDigits(params, rawValue) {
    return !isEmpty(params.step)
        ? getDecimalDigits(params.step)
        : Math.max(getDecimalDigits(rawValue), 2);
}
function getSuitableKeyScale(params) {
    var _a;
    return (_a = params.step) !== null && _a !== void 0 ? _a : 1;
}
function getSuitablePointerScale(params, rawValue) {
    var _a;
    const base = Math.abs((_a = params.step) !== null && _a !== void 0 ? _a : rawValue);
    return base === 0 ? 0.1 : Math.pow(10, Math.floor(Math.log10(base)) - 1);
}
function createStepConstraint(params, initialValue) {
    if (!isEmpty(params.step)) {
        return new StepConstraint(params.step, initialValue);
    }
    return null;
}
function createRangeConstraint(params) {
    if (!isEmpty(params.max) && !isEmpty(params.min)) {
        return new DefiniteRangeConstraint({
            max: params.max,
            min: params.min,
        });
    }
    if (!isEmpty(params.max) || !isEmpty(params.min)) {
        return new RangeConstraint({
            max: params.max,
            min: params.min,
        });
    }
    return null;
}
function createNumberTextPropsObject(params, initialValue) {
    var _a, _b, _c;
    return {
        formatter: (_a = params.format) !== null && _a !== void 0 ? _a : createNumberFormatter(getSuitableDecimalDigits(params, initialValue)),
        keyScale: (_b = params.keyScale) !== null && _b !== void 0 ? _b : getSuitableKeyScale(params),
        pointerScale: (_c = params.pointerScale) !== null && _c !== void 0 ? _c : getSuitablePointerScale(params, initialValue),
    };
}
function createNumberTextInputParamsParser(p) {
    return {
        format: p.optional.function,
        keyScale: p.optional.number,
        max: p.optional.number,
        min: p.optional.number,
        pointerScale: p.optional.number,
        step: p.optional.number,
    };
}

function createPointAxis(config) {
    return {
        constraint: config.constraint,
        textProps: ValueMap.fromObject(createNumberTextPropsObject(config.params, config.initialValue)),
    };
}

class BladeApi {
    constructor(controller) {
        this.controller = controller;
    }
    get element() {
        return this.controller.view.element;
    }
    get disabled() {
        return this.controller.viewProps.get('disabled');
    }
    set disabled(disabled) {
        this.controller.viewProps.set('disabled', disabled);
    }
    get hidden() {
        return this.controller.viewProps.get('hidden');
    }
    set hidden(hidden) {
        this.controller.viewProps.set('hidden', hidden);
    }
    dispose() {
        this.controller.viewProps.set('disposed', true);
    }
    importState(state) {
        return this.controller.importState(state);
    }
    exportState() {
        return this.controller.exportState();
    }
}

class TpEvent {
    constructor(target) {
        this.target = target;
    }
}
class TpChangeEvent extends TpEvent {
    constructor(target, value, last) {
        super(target);
        this.value = value;
        this.last = last !== null && last !== void 0 ? last : true;
    }
}
class TpFoldEvent extends TpEvent {
    constructor(target, expanded) {
        super(target);
        this.expanded = expanded;
    }
}
class TpTabSelectEvent extends TpEvent {
    constructor(target, index) {
        super(target);
        this.index = index;
    }
}

class BindingApi extends BladeApi {
    constructor(controller) {
        super(controller);
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.emitter_ = new Emitter();
        this.controller.value.emitter.on('change', this.onValueChange_);
    }
    get label() {
        return this.controller.labelController.props.get('label');
    }
    set label(label) {
        this.controller.labelController.props.set('label', label);
    }
    get key() {
        return this.controller.value.binding.target.key;
    }
    get tag() {
        return this.controller.tag;
    }
    set tag(tag) {
        this.controller.tag = tag;
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.emitter_.on(eventName, (ev) => {
            bh(ev);
        });
        return this;
    }
    refresh() {
        this.controller.value.fetch();
    }
    onValueChange_(ev) {
        const value = this.controller.value;
        this.emitter_.emit('change', new TpChangeEvent(this, forceCast(value.binding.target.read()), ev.options.last));
    }
}

function parseObject(value, keyToParserMap) {
    const keys = Object.keys(keyToParserMap);
    const result = keys.reduce((tmp, key) => {
        if (tmp === undefined) {
            return undefined;
        }
        const parser = keyToParserMap[key];
        const result = parser(value[key]);
        return result.succeeded
            ? Object.assign(Object.assign({}, tmp), { [key]: result.value }) : undefined;
    }, {});
    return forceCast(result);
}
function parseArray(value, parseItem) {
    return value.reduce((tmp, item) => {
        if (tmp === undefined) {
            return undefined;
        }
        const result = parseItem(item);
        if (!result.succeeded || result.value === undefined) {
            return undefined;
        }
        return [...tmp, result.value];
    }, []);
}
function isObject(value) {
    if (value === null) {
        return false;
    }
    return typeof value === 'object';
}
function createMicroParserBuilder(parse) {
    return (optional) => (v) => {
        if (!optional && v === undefined) {
            return {
                succeeded: false,
                value: undefined,
            };
        }
        if (optional && v === undefined) {
            return {
                succeeded: true,
                value: undefined,
            };
        }
        const result = parse(v);
        return result !== undefined
            ? {
                succeeded: true,
                value: result,
            }
            : {
                succeeded: false,
                value: undefined,
            };
    };
}
function createMicroParserBuilders(optional) {
    return {
        custom: (parse) => createMicroParserBuilder(parse)(optional),
        boolean: createMicroParserBuilder((v) => typeof v === 'boolean' ? v : undefined)(optional),
        number: createMicroParserBuilder((v) => typeof v === 'number' ? v : undefined)(optional),
        string: createMicroParserBuilder((v) => typeof v === 'string' ? v : undefined)(optional),
        function: createMicroParserBuilder((v) =>
        typeof v === 'function' ? v : undefined)(optional),
        constant: (value) => createMicroParserBuilder((v) => (v === value ? value : undefined))(optional),
        raw: createMicroParserBuilder((v) => v)(optional),
        object: (keyToParserMap) => createMicroParserBuilder((v) => {
            if (!isObject(v)) {
                return undefined;
            }
            return parseObject(v, keyToParserMap);
        })(optional),
        array: (itemParser) => createMicroParserBuilder((v) => {
            if (!Array.isArray(v)) {
                return undefined;
            }
            return parseArray(v, itemParser);
        })(optional),
    };
}
const MicroParsers = {
    optional: createMicroParserBuilders(true),
    required: createMicroParserBuilders(false),
};
function parseRecord(value, keyToParserMap) {
    const map = keyToParserMap(MicroParsers);
    const result = MicroParsers.required.object(map)(value);
    return result.succeeded ? result.value : undefined;
}

function importBladeState(state, superImport, parser, callback) {
    if (superImport && !superImport(state)) {
        return false;
    }
    const result = parseRecord(state, parser);
    return result ? callback(result) : false;
}
function exportBladeState(superExport, thisState) {
    var _a;
    return deepMerge((_a = superExport === null || superExport === void 0 ? void 0 : superExport()) !== null && _a !== void 0 ? _a : {}, thisState);
}

function isValueBladeController(bc) {
    return 'value' in bc;
}

function isBindingValue(v) {
    if (!isObject$1(v) || !('binding' in v)) {
        return false;
    }
    const b = v.binding;
    return isBinding(b);
}

const SVG_NS = 'http://www.w3.org/2000/svg';
function forceReflow(element) {
    element.offsetHeight;
}
function disableTransitionTemporarily(element, callback) {
    const t = element.style.transition;
    element.style.transition = 'none';
    callback();
    element.style.transition = t;
}
function supportsTouch(doc) {
    return doc.ontouchstart !== undefined;
}
function getCanvasContext(canvasElement) {
    const win = canvasElement.ownerDocument.defaultView;
    if (!win) {
        return null;
    }
    const isBrowser = 'document' in win;
    return isBrowser
        ? canvasElement.getContext('2d', {
            willReadFrequently: true,
        })
        : null;
}
const ICON_ID_TO_INNER_HTML_MAP = {
    check: '<path d="M2 8l4 4l8 -8"/>',
    dropdown: '<path d="M5 7h6l-3 3 z"/>',
    p2dpad: '<path d="M8 4v8"/><path d="M4 8h8"/><circle cx="12" cy="12" r="1.2"/>',
};
function createSvgIconElement(document, iconId) {
    const elem = document.createElementNS(SVG_NS, 'svg');
    elem.innerHTML = ICON_ID_TO_INNER_HTML_MAP[iconId];
    return elem;
}
function insertElementAt(parentElement, element, index) {
    parentElement.insertBefore(element, parentElement.children[index]);
}
function removeElement(element) {
    if (element.parentElement) {
        element.parentElement.removeChild(element);
    }
}
function removeChildElements(element) {
    while (element.children.length > 0) {
        element.removeChild(element.children[0]);
    }
}
function removeChildNodes(element) {
    while (element.childNodes.length > 0) {
        element.removeChild(element.childNodes[0]);
    }
}
function findNextTarget(ev) {
    if (ev.relatedTarget) {
        return forceCast(ev.relatedTarget);
    }
    if ('explicitOriginalTarget' in ev) {
        return ev.explicitOriginalTarget;
    }
    return null;
}

function bindValue(value, applyValue) {
    value.emitter.on('change', (ev) => {
        applyValue(ev.rawValue);
    });
    applyValue(value.rawValue);
}
function bindValueMap(valueMap, key, applyValue) {
    bindValue(valueMap.value(key), applyValue);
}

const PREFIX = 'tp';
function ClassName(viewName) {
    const fn = (opt_elementName, opt_modifier) => {
        return [
            PREFIX,
            '-',
            viewName,
            'v',
            opt_elementName ? `_${opt_elementName}` : '',
            opt_modifier ? `-${opt_modifier}` : '',
        ].join('');
    };
    return fn;
}

const cn$q = ClassName('lbl');
function createLabelNode(doc, label) {
    const frag = doc.createDocumentFragment();
    const lineNodes = label.split('\n').map((line) => {
        return doc.createTextNode(line);
    });
    lineNodes.forEach((lineNode, index) => {
        if (index > 0) {
            frag.appendChild(doc.createElement('br'));
        }
        frag.appendChild(lineNode);
    });
    return frag;
}
class LabelView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$q());
        config.viewProps.bindClassModifiers(this.element);
        const labelElem = doc.createElement('div');
        labelElem.classList.add(cn$q('l'));
        bindValueMap(config.props, 'label', (value) => {
            if (isEmpty(value)) {
                this.element.classList.add(cn$q(undefined, 'nol'));
            }
            else {
                this.element.classList.remove(cn$q(undefined, 'nol'));
                removeChildNodes(labelElem);
                labelElem.appendChild(createLabelNode(doc, value));
            }
        });
        this.element.appendChild(labelElem);
        this.labelElement = labelElem;
        const valueElem = doc.createElement('div');
        valueElem.classList.add(cn$q('v'));
        this.element.appendChild(valueElem);
        this.valueElement = valueElem;
    }
}

class LabelController {
    constructor(doc, config) {
        this.props = config.props;
        this.valueController = config.valueController;
        this.viewProps = config.valueController.viewProps;
        this.view = new LabelView(doc, {
            props: config.props,
            viewProps: this.viewProps,
        });
        this.view.valueElement.appendChild(this.valueController.view.element);
    }
    importProps(state) {
        return importBladeState(state, null, (p) => ({
            label: p.optional.string,
        }), (result) => {
            this.props.set('label', result.label);
            return true;
        });
    }
    exportProps() {
        return exportBladeState(null, {
            label: this.props.get('label'),
        });
    }
}

function getAllBladePositions() {
    return ['veryfirst', 'first', 'last', 'verylast'];
}

const cn$p = ClassName('');
const POS_TO_CLASS_NAME_MAP = {
    veryfirst: 'vfst',
    first: 'fst',
    last: 'lst',
    verylast: 'vlst',
};
class BladeController {
    constructor(config) {
        this.parent_ = null;
        this.blade = config.blade;
        this.view = config.view;
        this.viewProps = config.viewProps;
        const elem = this.view.element;
        this.blade.value('positions').emitter.on('change', () => {
            getAllBladePositions().forEach((pos) => {
                elem.classList.remove(cn$p(undefined, POS_TO_CLASS_NAME_MAP[pos]));
            });
            this.blade.get('positions').forEach((pos) => {
                elem.classList.add(cn$p(undefined, POS_TO_CLASS_NAME_MAP[pos]));
            });
        });
        this.viewProps.handleDispose(() => {
            removeElement(elem);
        });
    }
    get parent() {
        return this.parent_;
    }
    set parent(parent) {
        this.parent_ = parent;
        this.viewProps.set('parent', this.parent_ ? this.parent_.viewProps : null);
    }
    importState(state) {
        return importBladeState(state, null, (p) => ({
            disabled: p.required.boolean,
            hidden: p.required.boolean,
        }), (result) => {
            this.viewProps.importState(result);
            return true;
        });
    }
    exportState() {
        return exportBladeState(null, Object.assign({}, this.viewProps.exportState()));
    }
}

class LabeledValueBladeController extends BladeController {
    constructor(doc, config) {
        if (config.value !== config.valueController.value) {
            throw TpError.shouldNeverHappen();
        }
        const viewProps = config.valueController.viewProps;
        const lc = new LabelController(doc, {
            blade: config.blade,
            props: config.props,
            valueController: config.valueController,
        });
        super(Object.assign(Object.assign({}, config), { view: new LabelView(doc, {
                props: config.props,
                viewProps: viewProps,
            }), viewProps: viewProps }));
        this.labelController = lc;
        this.value = config.value;
        this.valueController = config.valueController;
        this.view.valueElement.appendChild(this.valueController.view.element);
    }
    importState(state) {
        return importBladeState(state, (s) => {
            var _a, _b, _c;
            return super.importState(s) &&
                this.labelController.importProps(s) &&
                ((_c = (_b = (_a = this.valueController).importProps) === null || _b === void 0 ? void 0 : _b.call(_a, state)) !== null && _c !== void 0 ? _c : true);
        }, (p) => ({
            value: p.optional.raw,
        }), (result) => {
            if (result.value) {
                this.value.rawValue = result.value;
            }
            return true;
        });
    }
    exportState() {
        var _a, _b, _c;
        return exportBladeState(() => super.exportState(), Object.assign(Object.assign({ value: this.value.rawValue }, this.labelController.exportProps()), ((_c = (_b = (_a = this.valueController).exportProps) === null || _b === void 0 ? void 0 : _b.call(_a)) !== null && _c !== void 0 ? _c : {})));
    }
}

function fillBuffer(buffer, bufferSize) {
    while (buffer.length < bufferSize) {
        buffer.push(undefined);
    }
}
function initializeBuffer(bufferSize) {
    const buffer = [];
    fillBuffer(buffer, bufferSize);
    return buffer;
}
function createTrimmedBuffer(buffer) {
    const index = buffer.indexOf(undefined);
    return forceCast(index < 0 ? buffer : buffer.slice(0, index));
}
function createPushedBuffer(buffer, newValue) {
    const newBuffer = [...createTrimmedBuffer(buffer), newValue];
    if (newBuffer.length > buffer.length) {
        newBuffer.splice(0, newBuffer.length - buffer.length);
    }
    else {
        fillBuffer(newBuffer, buffer.length);
    }
    return newBuffer;
}

class ButtonApi extends BladeApi {
    get label() {
        return this.controller.labelController.props.get('label');
    }
    set label(label) {
        this.controller.labelController.props.set('label', label);
    }
    get title() {
        var _a;
        return (_a = this.controller.buttonController.props.get('title')) !== null && _a !== void 0 ? _a : '';
    }
    set title(title) {
        this.controller.buttonController.props.set('title', title);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        const emitter = this.controller.buttonController.emitter;
        emitter.on(eventName, () => {
            bh(new TpEvent(this));
        });
        return this;
    }
}

function applyClass(elem, className, active) {
    if (active) {
        elem.classList.add(className);
    }
    else {
        elem.classList.remove(className);
    }
}
function valueToClassName(elem, className) {
    return (value) => {
        applyClass(elem, className, value);
    };
}
function bindValueToTextContent(value, elem) {
    bindValue(value, (text) => {
        elem.textContent = text !== null && text !== void 0 ? text : '';
    });
}

const cn$o = ClassName('btn');
class ButtonView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$o());
        config.viewProps.bindClassModifiers(this.element);
        const buttonElem = doc.createElement('button');
        buttonElem.classList.add(cn$o('b'));
        config.viewProps.bindDisabled(buttonElem);
        this.element.appendChild(buttonElem);
        this.buttonElement = buttonElem;
        const titleElem = doc.createElement('div');
        titleElem.classList.add(cn$o('t'));
        bindValueToTextContent(config.props.value('title'), titleElem);
        this.buttonElement.appendChild(titleElem);
    }
}

class ButtonController {
    constructor(doc, config) {
        this.emitter = new Emitter();
        this.onClick_ = this.onClick_.bind(this);
        this.props = config.props;
        this.viewProps = config.viewProps;
        this.view = new ButtonView(doc, {
            props: this.props,
            viewProps: this.viewProps,
        });
        this.view.buttonElement.addEventListener('click', this.onClick_);
    }
    importProps(state) {
        return importBladeState(state, null, (p) => ({
            title: p.optional.string,
        }), (result) => {
            this.props.set('title', result.title);
            return true;
        });
    }
    exportProps() {
        return exportBladeState(null, {
            title: this.props.get('title'),
        });
    }
    onClick_() {
        this.emitter.emit('click', {
            sender: this,
        });
    }
}

class ButtonBladeController extends BladeController {
    constructor(doc, config) {
        const bc = new ButtonController(doc, {
            props: config.buttonProps,
            viewProps: config.viewProps,
        });
        const lc = new LabelController(doc, {
            blade: config.blade,
            props: config.labelProps,
            valueController: bc,
        });
        super({
            blade: config.blade,
            view: lc.view,
            viewProps: config.viewProps,
        });
        this.buttonController = bc;
        this.labelController = lc;
    }
    importState(state) {
        return importBladeState(state, (s) => super.importState(s) &&
            this.buttonController.importProps(s) &&
            this.labelController.importProps(s), () => ({}), () => true);
    }
    exportState() {
        return exportBladeState(() => super.exportState(), Object.assign(Object.assign({}, this.buttonController.exportProps()), this.labelController.exportProps()));
    }
}

class Semver {
    constructor(text) {
        const [core, prerelease] = text.split('-');
        const coreComps = core.split('.');
        this.major = parseInt(coreComps[0], 10);
        this.minor = parseInt(coreComps[1], 10);
        this.patch = parseInt(coreComps[2], 10);
        this.prerelease = prerelease !== null && prerelease !== void 0 ? prerelease : null;
    }
    toString() {
        const core = [this.major, this.minor, this.patch].join('.');
        return this.prerelease !== null ? [core, this.prerelease].join('-') : core;
    }
}

const VERSION = new Semver('2.0.0-beta.2');

function createPlugin(plugin) {
    return Object.assign({ core: VERSION }, plugin);
}

createPlugin({
    id: 'button',
    type: 'blade',
    accept(params) {
        const result = parseRecord(params, (p) => ({
            title: p.required.string,
            view: p.required.constant('button'),
            label: p.optional.string,
        }));
        return result ? { params: result } : null;
    },
    controller(args) {
        return new ButtonBladeController(args.document, {
            blade: args.blade,
            buttonProps: ValueMap.fromObject({
                title: args.params.title,
            }),
            labelProps: ValueMap.fromObject({
                label: args.params.label,
            }),
            viewProps: args.viewProps,
        });
    },
    api(args) {
        if (args.controller instanceof ButtonBladeController) {
            return new ButtonApi(args.controller);
        }
        return null;
    },
});

function addButtonAsBlade(api, params) {
    return api.addBlade(Object.assign(Object.assign({}, params), { view: 'button' }));
}
function addFolderAsBlade(api, params) {
    return api.addBlade(Object.assign(Object.assign({}, params), { view: 'folder' }));
}
function addTabAsBlade(api, params) {
    return api.addBlade(Object.assign(Object.assign({}, params), { view: 'tab' }));
}

function isRefreshable(value) {
    if (!isObject$1(value)) {
        return false;
    }
    return 'refresh' in value && typeof value.refresh === 'function';
}

function createBindingTarget(obj, key) {
    if (!BindingTarget.isBindable(obj)) {
        throw TpError.notBindable();
    }
    return new BindingTarget(obj, key);
}
class RackApi {
    constructor(controller, pool) {
        this.onRackValueChange_ = this.onRackValueChange_.bind(this);
        this.controller_ = controller;
        this.emitter_ = new Emitter();
        this.pool_ = pool;
        const rack = this.controller_.rack;
        rack.emitter.on('valuechange', this.onRackValueChange_);
    }
    get children() {
        return this.controller_.rack.children.map((bc) => this.pool_.createApi(bc));
    }
    addBinding(object, key, opt_params) {
        const params = opt_params !== null && opt_params !== void 0 ? opt_params : {};
        const doc = this.controller_.element.ownerDocument;
        const bc = this.pool_.createBinding(doc, createBindingTarget(object, key), params);
        const api = this.pool_.createBindingApi(bc);
        return this.add(api, params.index);
    }
    addFolder(params) {
        return addFolderAsBlade(this, params);
    }
    addButton(params) {
        return addButtonAsBlade(this, params);
    }
    addTab(params) {
        return addTabAsBlade(this, params);
    }
    add(api, opt_index) {
        const bc = api.controller;
        this.controller_.rack.add(bc, opt_index);
        return api;
    }
    remove(api) {
        this.controller_.rack.remove(api.controller);
    }
    addBlade(params) {
        const doc = this.controller_.element.ownerDocument;
        const bc = this.pool_.createBlade(doc, params);
        const api = this.pool_.createApi(bc);
        return this.add(api, params.index);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.emitter_.on(eventName, (ev) => {
            bh(ev);
        });
        return this;
    }
    refresh() {
        this.children.forEach((c) => {
            if (isRefreshable(c)) {
                c.refresh();
            }
        });
    }
    onRackValueChange_(ev) {
        const bc = ev.bladeController;
        const api = this.pool_.createApi(bc);
        const binding = isBindingValue(bc.value) ? bc.value.binding : null;
        this.emitter_.emit('change', new TpChangeEvent(api, binding ? binding.target.read() : bc.value.rawValue, ev.options.last));
    }
}

class ContainerBladeApi extends BladeApi {
    constructor(controller, pool) {
        super(controller);
        this.rackApi_ = new RackApi(controller.rackController, pool);
    }
}

class ContainerBladeController extends BladeController {
    constructor(config) {
        super({
            blade: config.blade,
            view: config.view,
            viewProps: config.rackController.viewProps,
        });
        this.rackController = config.rackController;
    }
    importState(state) {
        return importBladeState(state, (s) => super.importState(s), (p) => ({
            children: p.required.array(p.required.raw),
        }), (result) => {
            return this.rackController.rack.children.every((c, index) => {
                return c.importState(result.children[index]);
            });
        });
    }
    exportState() {
        return exportBladeState(() => super.exportState(), {
            children: this.rackController.rack.children.map((c) => c.exportState()),
        });
    }
}
function isContainerBladeController(bc) {
    return 'rackController' in bc;
}

class NestedOrderedSet {
    constructor(extract) {
        this.emitter = new Emitter();
        this.items_ = [];
        this.cache_ = new Set();
        this.onSubListAdd_ = this.onSubListAdd_.bind(this);
        this.onSubListRemove_ = this.onSubListRemove_.bind(this);
        this.extract_ = extract;
    }
    get items() {
        return this.items_;
    }
    allItems() {
        return Array.from(this.cache_);
    }
    find(callback) {
        for (const item of this.allItems()) {
            if (callback(item)) {
                return item;
            }
        }
        return null;
    }
    includes(item) {
        return this.cache_.has(item);
    }
    add(item, opt_index) {
        if (this.includes(item)) {
            throw TpError.shouldNeverHappen();
        }
        const index = opt_index !== undefined ? opt_index : this.items_.length;
        this.items_.splice(index, 0, item);
        this.cache_.add(item);
        const subList = this.extract_(item);
        if (subList) {
            subList.emitter.on('add', this.onSubListAdd_);
            subList.emitter.on('remove', this.onSubListRemove_);
            subList.allItems().forEach((i) => {
                this.cache_.add(i);
            });
        }
        this.emitter.emit('add', {
            index: index,
            item: item,
            root: this,
            target: this,
        });
    }
    remove(item) {
        const index = this.items_.indexOf(item);
        if (index < 0) {
            return;
        }
        this.items_.splice(index, 1);
        this.cache_.delete(item);
        const subList = this.extract_(item);
        if (subList) {
            subList.allItems().forEach((i) => {
                this.cache_.delete(i);
            });
            subList.emitter.off('add', this.onSubListAdd_);
            subList.emitter.off('remove', this.onSubListRemove_);
        }
        this.emitter.emit('remove', {
            index: index,
            item: item,
            root: this,
            target: this,
        });
    }
    onSubListAdd_(ev) {
        this.cache_.add(ev.item);
        this.emitter.emit('add', {
            index: ev.index,
            item: ev.item,
            root: this,
            target: ev.target,
        });
    }
    onSubListRemove_(ev) {
        this.cache_.delete(ev.item);
        this.emitter.emit('remove', {
            index: ev.index,
            item: ev.item,
            root: this,
            target: ev.target,
        });
    }
}

function findValueBladeController(bcs, v) {
    for (let i = 0; i < bcs.length; i++) {
        const bc = bcs[i];
        if (isValueBladeController(bc) && bc.value === v) {
            return bc;
        }
    }
    return null;
}
function findSubBladeControllerSet(bc) {
    return isContainerBladeController(bc)
        ? bc.rackController.rack['bcSet_']
        : null;
}
class Rack {
    constructor(config) {
        var _a, _b;
        this.emitter = new Emitter();
        this.onBladePositionsChange_ = this.onBladePositionsChange_.bind(this);
        this.onSetAdd_ = this.onSetAdd_.bind(this);
        this.onSetRemove_ = this.onSetRemove_.bind(this);
        this.onChildDispose_ = this.onChildDispose_.bind(this);
        this.onChildPositionsChange_ = this.onChildPositionsChange_.bind(this);
        this.onChildValueChange_ = this.onChildValueChange_.bind(this);
        this.onChildViewPropsChange_ = this.onChildViewPropsChange_.bind(this);
        this.onRackLayout_ = this.onRackLayout_.bind(this);
        this.onRackValueChange_ = this.onRackValueChange_.bind(this);
        this.blade_ = (_a = config.blade) !== null && _a !== void 0 ? _a : null;
        (_b = this.blade_) === null || _b === void 0 ? void 0 : _b.value('positions').emitter.on('change', this.onBladePositionsChange_);
        this.viewProps = config.viewProps;
        this.bcSet_ = new NestedOrderedSet(findSubBladeControllerSet);
        this.bcSet_.emitter.on('add', this.onSetAdd_);
        this.bcSet_.emitter.on('remove', this.onSetRemove_);
    }
    get children() {
        return this.bcSet_.items;
    }
    add(bc, opt_index) {
        var _a;
        (_a = bc.parent) === null || _a === void 0 ? void 0 : _a.remove(bc);
        bc.parent = this;
        this.bcSet_.add(bc, opt_index);
    }
    remove(bc) {
        bc.parent = null;
        this.bcSet_.remove(bc);
    }
    find(finder) {
        return this.bcSet_.allItems().filter(finder);
    }
    onSetAdd_(ev) {
        this.updatePositions_();
        const root = ev.target === ev.root;
        this.emitter.emit('add', {
            bladeController: ev.item,
            index: ev.index,
            root: root,
            sender: this,
        });
        if (!root) {
            return;
        }
        const bc = ev.item;
        bc.viewProps.emitter.on('change', this.onChildViewPropsChange_);
        bc.blade
            .value('positions')
            .emitter.on('change', this.onChildPositionsChange_);
        bc.viewProps.handleDispose(this.onChildDispose_);
        if (isValueBladeController(bc)) {
            bc.value.emitter.on('change', this.onChildValueChange_);
        }
        else if (isContainerBladeController(bc)) {
            const rack = bc.rackController.rack;
            if (rack) {
                const emitter = rack.emitter;
                emitter.on('layout', this.onRackLayout_);
                emitter.on('valuechange', this.onRackValueChange_);
            }
        }
    }
    onSetRemove_(ev) {
        this.updatePositions_();
        const root = ev.target === ev.root;
        this.emitter.emit('remove', {
            bladeController: ev.item,
            root: root,
            sender: this,
        });
        if (!root) {
            return;
        }
        const bc = ev.item;
        if (isValueBladeController(bc)) {
            bc.value.emitter.off('change', this.onChildValueChange_);
        }
        else if (isContainerBladeController(bc)) {
            const rack = bc.rackController.rack;
            if (rack) {
                const emitter = rack.emitter;
                emitter.off('layout', this.onRackLayout_);
                emitter.off('valuechange', this.onRackValueChange_);
            }
        }
    }
    updatePositions_() {
        const visibleItems = this.bcSet_.items.filter((bc) => !bc.viewProps.get('hidden'));
        const firstVisibleItem = visibleItems[0];
        const lastVisibleItem = visibleItems[visibleItems.length - 1];
        this.bcSet_.items.forEach((bc) => {
            const ps = [];
            if (bc === firstVisibleItem) {
                ps.push('first');
                if (!this.blade_ ||
                    this.blade_.get('positions').includes('veryfirst')) {
                    ps.push('veryfirst');
                }
            }
            if (bc === lastVisibleItem) {
                ps.push('last');
                if (!this.blade_ || this.blade_.get('positions').includes('verylast')) {
                    ps.push('verylast');
                }
            }
            bc.blade.set('positions', ps);
        });
    }
    onChildPositionsChange_() {
        this.updatePositions_();
        this.emitter.emit('layout', {
            sender: this,
        });
    }
    onChildViewPropsChange_(_ev) {
        this.updatePositions_();
        this.emitter.emit('layout', {
            sender: this,
        });
    }
    onChildDispose_() {
        const disposedUcs = this.bcSet_.items.filter((bc) => {
            return bc.viewProps.get('disposed');
        });
        disposedUcs.forEach((bc) => {
            this.bcSet_.remove(bc);
        });
    }
    onChildValueChange_(ev) {
        const bc = findValueBladeController(this.find(isValueBladeController), ev.sender);
        if (!bc) {
            throw TpError.alreadyDisposed();
        }
        this.emitter.emit('valuechange', {
            bladeController: bc,
            options: ev.options,
            sender: this,
        });
    }
    onRackLayout_(_) {
        this.updatePositions_();
        this.emitter.emit('layout', {
            sender: this,
        });
    }
    onRackValueChange_(ev) {
        this.emitter.emit('valuechange', {
            bladeController: ev.bladeController,
            options: ev.options,
            sender: this,
        });
    }
    onBladePositionsChange_() {
        this.updatePositions_();
    }
}

class RackController {
    constructor(config) {
        this.onRackAdd_ = this.onRackAdd_.bind(this);
        this.onRackRemove_ = this.onRackRemove_.bind(this);
        this.element = config.element;
        this.viewProps = config.viewProps;
        const rack = new Rack({
            blade: config.root ? undefined : config.blade,
            viewProps: config.viewProps,
        });
        rack.emitter.on('add', this.onRackAdd_);
        rack.emitter.on('remove', this.onRackRemove_);
        this.rack = rack;
        this.viewProps.handleDispose(() => {
            for (let i = this.rack.children.length - 1; i >= 0; i--) {
                const bc = this.rack.children[i];
                bc.viewProps.set('disposed', true);
            }
        });
    }
    onRackAdd_(ev) {
        if (!ev.root) {
            return;
        }
        insertElementAt(this.element, ev.bladeController.view.element, ev.index);
    }
    onRackRemove_(ev) {
        if (!ev.root) {
            return;
        }
        removeElement(ev.bladeController.view.element);
    }
}

function createBlade() {
    return new ValueMap({
        positions: createValue([], {
            equals: deepEqualsArray,
        }),
    });
}

class Foldable extends ValueMap {
    constructor(valueMap) {
        super(valueMap);
    }
    static create(expanded) {
        const coreObj = {
            completed: true,
            expanded: expanded,
            expandedHeight: null,
            shouldFixHeight: false,
            temporaryExpanded: null,
        };
        const core = ValueMap.createCore(coreObj);
        return new Foldable(core);
    }
    get styleExpanded() {
        var _a;
        return (_a = this.get('temporaryExpanded')) !== null && _a !== void 0 ? _a : this.get('expanded');
    }
    get styleHeight() {
        if (!this.styleExpanded) {
            return '0';
        }
        const exHeight = this.get('expandedHeight');
        if (this.get('shouldFixHeight') && !isEmpty(exHeight)) {
            return `${exHeight}px`;
        }
        return 'auto';
    }
    bindExpandedClass(elem, expandedClassName) {
        const onExpand = () => {
            const expanded = this.styleExpanded;
            if (expanded) {
                elem.classList.add(expandedClassName);
            }
            else {
                elem.classList.remove(expandedClassName);
            }
        };
        bindValueMap(this, 'expanded', onExpand);
        bindValueMap(this, 'temporaryExpanded', onExpand);
    }
    cleanUpTransition() {
        this.set('shouldFixHeight', false);
        this.set('expandedHeight', null);
        this.set('completed', true);
    }
}
function computeExpandedFolderHeight(folder, containerElement) {
    let height = 0;
    disableTransitionTemporarily(containerElement, () => {
        folder.set('expandedHeight', null);
        folder.set('temporaryExpanded', true);
        forceReflow(containerElement);
        height = containerElement.clientHeight;
        folder.set('temporaryExpanded', null);
        forceReflow(containerElement);
    });
    return height;
}
function applyHeight(foldable, elem) {
    elem.style.height = foldable.styleHeight;
}
function bindFoldable(foldable, elem) {
    foldable.value('expanded').emitter.on('beforechange', () => {
        foldable.set('completed', false);
        if (isEmpty(foldable.get('expandedHeight'))) {
            const h = computeExpandedFolderHeight(foldable, elem);
            if (h > 0) {
                foldable.set('expandedHeight', h);
            }
        }
        foldable.set('shouldFixHeight', true);
        forceReflow(elem);
    });
    foldable.emitter.on('change', () => {
        applyHeight(foldable, elem);
    });
    applyHeight(foldable, elem);
    elem.addEventListener('transitionend', (ev) => {
        if (ev.propertyName !== 'height') {
            return;
        }
        foldable.cleanUpTransition();
    });
}

class FolderApi extends ContainerBladeApi {
    constructor(controller, pool) {
        super(controller, pool);
        this.emitter_ = new Emitter();
        this.controller.foldable
            .value('expanded')
            .emitter.on('change', (ev) => {
            this.emitter_.emit('fold', new TpFoldEvent(this, ev.sender.rawValue));
        });
        this.rackApi_.on('change', (ev) => {
            this.emitter_.emit('change', ev);
        });
    }
    get expanded() {
        return this.controller.foldable.get('expanded');
    }
    set expanded(expanded) {
        this.controller.foldable.set('expanded', expanded);
    }
    get title() {
        return this.controller.props.get('title');
    }
    set title(title) {
        this.controller.props.set('title', title);
    }
    get children() {
        return this.rackApi_.children;
    }
    addBinding(object, key, opt_params) {
        return this.rackApi_.addBinding(object, key, opt_params);
    }
    addFolder(params) {
        return this.rackApi_.addFolder(params);
    }
    addButton(params) {
        return this.rackApi_.addButton(params);
    }
    addTab(params) {
        return this.rackApi_.addTab(params);
    }
    add(api, opt_index) {
        return this.rackApi_.add(api, opt_index);
    }
    remove(api) {
        this.rackApi_.remove(api);
    }
    addBlade(params) {
        return this.rackApi_.addBlade(params);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.emitter_.on(eventName, (ev) => {
            bh(ev);
        });
        return this;
    }
    refresh() {
        this.rackApi_.refresh();
    }
}

const bladeContainerClassName = ClassName('cnt');

class FolderView {
    constructor(doc, config) {
        var _a;
        this.className_ = ClassName((_a = config.viewName) !== null && _a !== void 0 ? _a : 'fld');
        this.element = doc.createElement('div');
        this.element.classList.add(this.className_(), bladeContainerClassName());
        config.viewProps.bindClassModifiers(this.element);
        this.foldable_ = config.foldable;
        this.foldable_.bindExpandedClass(this.element, this.className_(undefined, 'expanded'));
        bindValueMap(this.foldable_, 'completed', valueToClassName(this.element, this.className_(undefined, 'cpl')));
        const buttonElem = doc.createElement('button');
        buttonElem.classList.add(this.className_('b'));
        bindValueMap(config.props, 'title', (title) => {
            if (isEmpty(title)) {
                this.element.classList.add(this.className_(undefined, 'not'));
            }
            else {
                this.element.classList.remove(this.className_(undefined, 'not'));
            }
        });
        config.viewProps.bindDisabled(buttonElem);
        this.element.appendChild(buttonElem);
        this.buttonElement = buttonElem;
        const indentElem = doc.createElement('div');
        indentElem.classList.add(this.className_('i'));
        this.element.appendChild(indentElem);
        const titleElem = doc.createElement('div');
        titleElem.classList.add(this.className_('t'));
        bindValueToTextContent(config.props.value('title'), titleElem);
        this.buttonElement.appendChild(titleElem);
        this.titleElement = titleElem;
        const markElem = doc.createElement('div');
        markElem.classList.add(this.className_('m'));
        this.buttonElement.appendChild(markElem);
        const containerElem = doc.createElement('div');
        containerElem.classList.add(this.className_('c'));
        this.element.appendChild(containerElem);
        this.containerElement = containerElem;
    }
}

class FolderController extends ContainerBladeController {
    constructor(doc, config) {
        var _a;
        const foldable = Foldable.create((_a = config.expanded) !== null && _a !== void 0 ? _a : true);
        const view = new FolderView(doc, {
            foldable: foldable,
            props: config.props,
            viewName: config.root ? 'rot' : undefined,
            viewProps: config.viewProps,
        });
        super(Object.assign(Object.assign({}, config), { rackController: new RackController({
                blade: config.blade,
                element: view.containerElement,
                root: config.root,
                viewProps: config.viewProps,
            }), view: view }));
        this.onTitleClick_ = this.onTitleClick_.bind(this);
        this.props = config.props;
        this.foldable = foldable;
        bindFoldable(this.foldable, this.view.containerElement);
        this.rackController.rack.emitter.on('add', () => {
            this.foldable.cleanUpTransition();
        });
        this.rackController.rack.emitter.on('remove', () => {
            this.foldable.cleanUpTransition();
        });
        this.view.buttonElement.addEventListener('click', this.onTitleClick_);
    }
    get document() {
        return this.view.element.ownerDocument;
    }
    importState(state) {
        return importBladeState(state, (s) => super.importState(s), (p) => ({
            expanded: p.required.boolean,
            title: p.optional.string,
        }), (result) => {
            this.foldable.set('expanded', result.expanded);
            this.props.set('title', result.title);
            return true;
        });
    }
    exportState() {
        return exportBladeState(() => super.exportState(), {
            expanded: this.foldable.get('expanded'),
            title: this.props.get('title'),
        });
    }
    onTitleClick_() {
        this.foldable.set('expanded', !this.foldable.get('expanded'));
    }
}

createPlugin({
    id: 'folder',
    type: 'blade',
    accept(params) {
        const result = parseRecord(params, (p) => ({
            title: p.required.string,
            view: p.required.constant('folder'),
            expanded: p.optional.boolean,
        }));
        return result ? { params: result } : null;
    },
    controller(args) {
        return new FolderController(args.document, {
            blade: args.blade,
            expanded: args.params.expanded,
            props: ValueMap.fromObject({
                title: args.params.title,
            }),
            viewProps: args.viewProps,
        });
    },
    api(args) {
        if (!(args.controller instanceof FolderController)) {
            return null;
        }
        return new FolderApi(args.controller, args.pool);
    },
});

const cn$n = ClassName('');
function valueToModifier(elem, modifier) {
    return valueToClassName(elem, cn$n(undefined, modifier));
}
class ViewProps extends ValueMap {
    constructor(valueMap) {
        var _a;
        super(valueMap);
        this.onDisabledChange_ = this.onDisabledChange_.bind(this);
        this.onParentChange_ = this.onParentChange_.bind(this);
        this.onParentGlobalDisabledChange_ =
            this.onParentGlobalDisabledChange_.bind(this);
        [this.globalDisabled_, this.setGlobalDisabled_] = createReadonlyValue(createValue(this.getGlobalDisabled_()));
        this.value('disabled').emitter.on('change', this.onDisabledChange_);
        this.value('parent').emitter.on('change', this.onParentChange_);
        (_a = this.get('parent')) === null || _a === void 0 ? void 0 : _a.globalDisabled.emitter.on('change', this.onParentGlobalDisabledChange_);
    }
    static create(opt_initialValue) {
        var _a, _b, _c;
        const initialValue = opt_initialValue !== null && opt_initialValue !== void 0 ? opt_initialValue : {};
        return new ViewProps(ValueMap.createCore({
            disabled: (_a = initialValue.disabled) !== null && _a !== void 0 ? _a : false,
            disposed: false,
            hidden: (_b = initialValue.hidden) !== null && _b !== void 0 ? _b : false,
            parent: (_c = initialValue.parent) !== null && _c !== void 0 ? _c : null,
        }));
    }
    get globalDisabled() {
        return this.globalDisabled_;
    }
    bindClassModifiers(elem) {
        bindValue(this.globalDisabled_, valueToModifier(elem, 'disabled'));
        bindValueMap(this, 'hidden', valueToModifier(elem, 'hidden'));
    }
    bindDisabled(target) {
        bindValue(this.globalDisabled_, (disabled) => {
            target.disabled = disabled;
        });
    }
    bindTabIndex(elem) {
        bindValue(this.globalDisabled_, (disabled) => {
            elem.tabIndex = disabled ? -1 : 0;
        });
    }
    handleDispose(callback) {
        this.value('disposed').emitter.on('change', (disposed) => {
            if (disposed) {
                callback();
            }
        });
    }
    importState(state) {
        this.set('disabled', state.disabled);
        this.set('hidden', state.hidden);
    }
    exportState() {
        return {
            disabled: this.get('disabled'),
            hidden: this.get('hidden'),
        };
    }
    getGlobalDisabled_() {
        const parent = this.get('parent');
        const parentDisabled = parent ? parent.globalDisabled.rawValue : false;
        return parentDisabled || this.get('disabled');
    }
    updateGlobalDisabled_() {
        this.setGlobalDisabled_(this.getGlobalDisabled_());
    }
    onDisabledChange_() {
        this.updateGlobalDisabled_();
    }
    onParentGlobalDisabledChange_() {
        this.updateGlobalDisabled_();
    }
    onParentChange_(ev) {
        var _a;
        const prevParent = ev.previousRawValue;
        prevParent === null || prevParent === void 0 ? void 0 : prevParent.globalDisabled.emitter.off('change', this.onParentGlobalDisabledChange_);
        (_a = this.get('parent')) === null || _a === void 0 ? void 0 : _a.globalDisabled.emitter.on('change', this.onParentGlobalDisabledChange_);
        this.updateGlobalDisabled_();
    }
}

const cn$m = ClassName('tbp');
class TabPageView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$m());
        config.viewProps.bindClassModifiers(this.element);
        const containerElem = doc.createElement('div');
        containerElem.classList.add(cn$m('c'));
        this.element.appendChild(containerElem);
        this.containerElement = containerElem;
    }
}

const cn$l = ClassName('tbi');
class TabItemView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$l());
        config.viewProps.bindClassModifiers(this.element);
        bindValueMap(config.props, 'selected', (selected) => {
            if (selected) {
                this.element.classList.add(cn$l(undefined, 'sel'));
            }
            else {
                this.element.classList.remove(cn$l(undefined, 'sel'));
            }
        });
        const buttonElem = doc.createElement('button');
        buttonElem.classList.add(cn$l('b'));
        config.viewProps.bindDisabled(buttonElem);
        this.element.appendChild(buttonElem);
        this.buttonElement = buttonElem;
        const titleElem = doc.createElement('div');
        titleElem.classList.add(cn$l('t'));
        bindValueToTextContent(config.props.value('title'), titleElem);
        this.buttonElement.appendChild(titleElem);
        this.titleElement = titleElem;
    }
}

class TabItemController {
    constructor(doc, config) {
        this.emitter = new Emitter();
        this.onClick_ = this.onClick_.bind(this);
        this.props = config.props;
        this.viewProps = config.viewProps;
        this.view = new TabItemView(doc, {
            props: config.props,
            viewProps: config.viewProps,
        });
        this.view.buttonElement.addEventListener('click', this.onClick_);
    }
    onClick_() {
        this.emitter.emit('click', {
            sender: this,
        });
    }
}

class TabPageController extends ContainerBladeController {
    constructor(doc, config) {
        const view = new TabPageView(doc, {
            viewProps: config.viewProps,
        });
        super(Object.assign(Object.assign({}, config), { rackController: new RackController({
                blade: config.blade,
                element: view.containerElement,
                viewProps: config.viewProps,
            }), view: view }));
        this.onItemClick_ = this.onItemClick_.bind(this);
        this.ic_ = new TabItemController(doc, {
            props: config.itemProps,
            viewProps: ViewProps.create(),
        });
        this.ic_.emitter.on('click', this.onItemClick_);
        this.props = config.props;
        bindValueMap(this.props, 'selected', (selected) => {
            this.itemController.props.set('selected', selected);
            this.viewProps.set('hidden', !selected);
        });
    }
    get itemController() {
        return this.ic_;
    }
    importState(state) {
        return importBladeState(state, (s) => super.importState(s), (p) => ({
            selected: p.required.boolean,
            title: p.required.string,
        }), (result) => {
            this.ic_.props.set('selected', result.selected);
            this.ic_.props.set('title', result.title);
            return true;
        });
    }
    exportState() {
        return exportBladeState(() => super.exportState(), {
            selected: this.ic_.props.get('selected'),
            title: this.ic_.props.get('title'),
        });
    }
    onItemClick_() {
        this.props.set('selected', true);
    }
}

class TabApi extends ContainerBladeApi {
    constructor(controller, pool) {
        super(controller, pool);
        this.emitter_ = new Emitter();
        this.onSelect_ = this.onSelect_.bind(this);
        this.pool_ = pool;
        this.rackApi_.on('change', (ev) => {
            this.emitter_.emit('change', ev);
        });
        this.controller.tab.selectedIndex.emitter.on('change', this.onSelect_);
    }
    get pages() {
        return this.rackApi_.children;
    }
    addPage(params) {
        const doc = this.controller.view.element.ownerDocument;
        const pc = new TabPageController(doc, {
            blade: createBlade(),
            itemProps: ValueMap.fromObject({
                selected: false,
                title: params.title,
            }),
            props: ValueMap.fromObject({
                selected: false,
            }),
            viewProps: ViewProps.create(),
        });
        const papi = this.pool_.createApi(pc);
        return this.rackApi_.add(papi, params.index);
    }
    removePage(index) {
        this.rackApi_.remove(this.rackApi_.children[index]);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.emitter_.on(eventName, (ev) => {
            bh(ev);
        });
        return this;
    }
    onSelect_(ev) {
        this.emitter_.emit('select', new TpTabSelectEvent(this, ev.rawValue));
    }
}

class TabPageApi extends ContainerBladeApi {
    get title() {
        var _a;
        return (_a = this.controller.itemController.props.get('title')) !== null && _a !== void 0 ? _a : '';
    }
    set title(title) {
        this.controller.itemController.props.set('title', title);
    }
    get selected() {
        return this.controller.props.get('selected');
    }
    set selected(selected) {
        this.controller.props.set('selected', selected);
    }
    get children() {
        return this.rackApi_.children;
    }
    addButton(params) {
        return this.rackApi_.addButton(params);
    }
    addFolder(params) {
        return this.rackApi_.addFolder(params);
    }
    addTab(params) {
        return this.rackApi_.addTab(params);
    }
    add(api, opt_index) {
        this.rackApi_.add(api, opt_index);
    }
    remove(api) {
        this.rackApi_.remove(api);
    }
    addBinding(object, key, opt_params) {
        return this.rackApi_.addBinding(object, key, opt_params);
    }
    addBlade(params) {
        return this.rackApi_.addBlade(params);
    }
    refresh() {
        this.rackApi_.refresh();
    }
}

const INDEX_NOT_SELECTED = -1;
class Tab {
    constructor() {
        this.onItemSelectedChange_ = this.onItemSelectedChange_.bind(this);
        this.empty = createValue(true);
        this.selectedIndex = createValue(INDEX_NOT_SELECTED);
        this.items_ = [];
    }
    add(item, opt_index) {
        const index = opt_index !== null && opt_index !== void 0 ? opt_index : this.items_.length;
        this.items_.splice(index, 0, item);
        item.emitter.on('change', this.onItemSelectedChange_);
        this.keepSelection_();
    }
    remove(item) {
        const index = this.items_.indexOf(item);
        if (index < 0) {
            return;
        }
        this.items_.splice(index, 1);
        item.emitter.off('change', this.onItemSelectedChange_);
        this.keepSelection_();
    }
    keepSelection_() {
        if (this.items_.length === 0) {
            this.selectedIndex.rawValue = INDEX_NOT_SELECTED;
            this.empty.rawValue = true;
            return;
        }
        const firstSelIndex = this.items_.findIndex((s) => s.rawValue);
        if (firstSelIndex < 0) {
            this.items_.forEach((s, i) => {
                s.rawValue = i === 0;
            });
            this.selectedIndex.rawValue = 0;
        }
        else {
            this.items_.forEach((s, i) => {
                s.rawValue = i === firstSelIndex;
            });
            this.selectedIndex.rawValue = firstSelIndex;
        }
        this.empty.rawValue = false;
    }
    onItemSelectedChange_(ev) {
        if (ev.rawValue) {
            const index = this.items_.findIndex((s) => s === ev.sender);
            this.items_.forEach((s, i) => {
                s.rawValue = i === index;
            });
            this.selectedIndex.rawValue = index;
        }
        else {
            this.keepSelection_();
        }
    }
}

const cn$k = ClassName('tab');
class TabView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$k(), bladeContainerClassName());
        config.viewProps.bindClassModifiers(this.element);
        bindValue(config.empty, valueToClassName(this.element, cn$k(undefined, 'nop')));
        const titleElem = doc.createElement('div');
        titleElem.classList.add(cn$k('t'));
        this.element.appendChild(titleElem);
        this.itemsElement = titleElem;
        const indentElem = doc.createElement('div');
        indentElem.classList.add(cn$k('i'));
        this.element.appendChild(indentElem);
        const contentsElem = doc.createElement('div');
        contentsElem.classList.add(cn$k('c'));
        this.element.appendChild(contentsElem);
        this.contentsElement = contentsElem;
    }
}

class TabController extends ContainerBladeController {
    constructor(doc, config) {
        const tab = new Tab();
        const view = new TabView(doc, {
            empty: tab.empty,
            viewProps: config.viewProps,
        });
        super({
            blade: config.blade,
            rackController: new RackController({
                blade: config.blade,
                element: view.contentsElement,
                viewProps: config.viewProps,
            }),
            view: view,
        });
        this.onRackAdd_ = this.onRackAdd_.bind(this);
        this.onRackRemove_ = this.onRackRemove_.bind(this);
        const rack = this.rackController.rack;
        rack.emitter.on('add', this.onRackAdd_);
        rack.emitter.on('remove', this.onRackRemove_);
        this.tab = tab;
    }
    add(pc, opt_index) {
        this.rackController.rack.add(pc, opt_index);
    }
    remove(index) {
        this.rackController.rack.remove(this.rackController.rack.children[index]);
    }
    onRackAdd_(ev) {
        if (!ev.root) {
            return;
        }
        const pc = ev.bladeController;
        insertElementAt(this.view.itemsElement, pc.itemController.view.element, ev.index);
        pc.itemController.viewProps.set('parent', this.viewProps);
        this.tab.add(pc.props.value('selected'));
    }
    onRackRemove_(ev) {
        if (!ev.root) {
            return;
        }
        const pc = ev.bladeController;
        removeElement(pc.itemController.view.element);
        pc.itemController.viewProps.set('parent', null);
        this.tab.remove(pc.props.value('selected'));
    }
}

createPlugin({
    id: 'tab',
    type: 'blade',
    accept(params) {
        const result = parseRecord(params, (p) => ({
            pages: p.required.array(p.required.object({ title: p.required.string })),
            view: p.required.constant('tab'),
        }));
        if (!result || result.pages.length === 0) {
            return null;
        }
        return { params: result };
    },
    controller(args) {
        const c = new TabController(args.document, {
            blade: args.blade,
            viewProps: args.viewProps,
        });
        args.params.pages.forEach((p) => {
            const pc = new TabPageController(args.document, {
                blade: createBlade(),
                itemProps: ValueMap.fromObject({
                    selected: false,
                    title: p.title,
                }),
                props: ValueMap.fromObject({
                    selected: false,
                }),
                viewProps: ViewProps.create(),
            });
            c.add(pc);
        });
        return c;
    },
    api(args) {
        if (args.controller instanceof TabController) {
            return new TabApi(args.controller, args.pool);
        }
        if (args.controller instanceof TabPageController) {
            return new TabPageApi(args.controller, args.pool);
        }
        return null;
    },
});

class ListInputBindingApi extends BindingApi {
    get options() {
        return this.controller.valueController.props.get('options');
    }
    set options(options) {
        this.controller.valueController.props.set('options', options);
    }
}

class ManualTicker {
    constructor() {
        this.disabled = false;
        this.emitter = new Emitter();
    }
    dispose() { }
    tick() {
        if (this.disabled) {
            return;
        }
        this.emitter.emit('tick', {
            sender: this,
        });
    }
}

class IntervalTicker {
    constructor(doc, interval) {
        this.disabled_ = false;
        this.timerId_ = null;
        this.onTick_ = this.onTick_.bind(this);
        this.doc_ = doc;
        this.emitter = new Emitter();
        this.interval_ = interval;
        this.setTimer_();
    }
    get disabled() {
        return this.disabled_;
    }
    set disabled(inactive) {
        this.disabled_ = inactive;
        if (this.disabled_) {
            this.clearTimer_();
        }
        else {
            this.setTimer_();
        }
    }
    dispose() {
        this.clearTimer_();
    }
    clearTimer_() {
        if (this.timerId_ === null) {
            return;
        }
        const win = this.doc_.defaultView;
        if (win) {
            win.clearInterval(this.timerId_);
        }
        this.timerId_ = null;
    }
    setTimer_() {
        this.clearTimer_();
        if (this.interval_ <= 0) {
            return;
        }
        const win = this.doc_.defaultView;
        if (win) {
            this.timerId_ = win.setInterval(this.onTick_, this.interval_);
        }
    }
    onTick_() {
        if (this.disabled_) {
            return;
        }
        this.emitter.emit('tick', {
            sender: this,
        });
    }
}

class CompositeConstraint {
    constructor(constraints) {
        this.constraints = constraints;
    }
    constrain(value) {
        return this.constraints.reduce((result, c) => {
            return c.constrain(result);
        }, value);
    }
}
function findConstraint(c, constraintClass) {
    if (c instanceof constraintClass) {
        return c;
    }
    if (c instanceof CompositeConstraint) {
        const result = c.constraints.reduce((tmpResult, sc) => {
            if (tmpResult) {
                return tmpResult;
            }
            return sc instanceof constraintClass ? sc : null;
        }, null);
        if (result) {
            return result;
        }
    }
    return null;
}

class ListConstraint {
    constructor(options) {
        this.values = ValueMap.fromObject({
            options: options,
        });
    }
    constrain(value) {
        const opts = this.values.get('options');
        if (opts.length === 0) {
            return value;
        }
        const matched = opts.filter((item) => {
            return item.value === value;
        }).length > 0;
        return matched ? value : opts[0].value;
    }
}

function parseListOptions(value) {
    var _a;
    const p = MicroParsers;
    if (Array.isArray(value)) {
        return (_a = parseRecord({ items: value }, (p) => ({
            items: p.required.array(p.required.object({
                text: p.required.string,
                value: p.required.raw,
            })),
        }))) === null || _a === void 0 ? void 0 : _a.items;
    }
    if (typeof value === 'object') {
        return p.required.raw(value)
            .value;
    }
    return undefined;
}
function normalizeListOptions(options) {
    if (Array.isArray(options)) {
        return options;
    }
    const items = [];
    Object.keys(options).forEach((text) => {
        items.push({ text: text, value: options[text] });
    });
    return items;
}
function createListConstraint(options) {
    return !isEmpty(options)
        ? new ListConstraint(normalizeListOptions(forceCast(options)))
        : null;
}

const cn$j = ClassName('lst');
class ListView {
    constructor(doc, config) {
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.props_ = config.props;
        this.element = doc.createElement('div');
        this.element.classList.add(cn$j());
        config.viewProps.bindClassModifiers(this.element);
        const selectElem = doc.createElement('select');
        selectElem.classList.add(cn$j('s'));
        config.viewProps.bindDisabled(selectElem);
        this.element.appendChild(selectElem);
        this.selectElement = selectElem;
        const markElem = doc.createElement('div');
        markElem.classList.add(cn$j('m'));
        markElem.appendChild(createSvgIconElement(doc, 'dropdown'));
        this.element.appendChild(markElem);
        config.value.emitter.on('change', this.onValueChange_);
        this.value_ = config.value;
        bindValueMap(this.props_, 'options', (opts) => {
            removeChildElements(this.selectElement);
            opts.forEach((item) => {
                const optionElem = doc.createElement('option');
                optionElem.textContent = item.text;
                this.selectElement.appendChild(optionElem);
            });
            this.update_();
        });
    }
    update_() {
        const values = this.props_.get('options').map((o) => o.value);
        this.selectElement.selectedIndex = values.indexOf(this.value_.rawValue);
    }
    onValueChange_() {
        this.update_();
    }
}

class ListController {
    constructor(doc, config) {
        this.onSelectChange_ = this.onSelectChange_.bind(this);
        this.props = config.props;
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new ListView(doc, {
            props: this.props,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.selectElement.addEventListener('change', this.onSelectChange_);
    }
    onSelectChange_(e) {
        const selectElem = forceCast(e.currentTarget);
        this.value.rawValue =
            this.props.get('options')[selectElem.selectedIndex].value;
    }
    importProps(state) {
        return importBladeState(state, null, (p) => ({
            options: p.required.custom(parseListOptions),
        }), (result) => {
            this.props.set('options', normalizeListOptions(result.options));
            return true;
        });
    }
    exportProps() {
        return exportBladeState(null, {
            options: this.props.get('options'),
        });
    }
}

const cn$i = ClassName('pop');
class PopupView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$i());
        config.viewProps.bindClassModifiers(this.element);
        bindValue(config.shows, valueToClassName(this.element, cn$i(undefined, 'v')));
    }
}

class PopupController {
    constructor(doc, config) {
        this.shows = createValue(false);
        this.viewProps = config.viewProps;
        this.view = new PopupView(doc, {
            shows: this.shows,
            viewProps: this.viewProps,
        });
    }
}

const cn$h = ClassName('txt');
class TextView {
    constructor(doc, config) {
        this.onChange_ = this.onChange_.bind(this);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$h());
        config.viewProps.bindClassModifiers(this.element);
        this.props_ = config.props;
        this.props_.emitter.on('change', this.onChange_);
        const inputElem = doc.createElement('input');
        inputElem.classList.add(cn$h('i'));
        inputElem.type = 'text';
        config.viewProps.bindDisabled(inputElem);
        this.element.appendChild(inputElem);
        this.inputElement = inputElem;
        config.value.emitter.on('change', this.onChange_);
        this.value_ = config.value;
        this.refresh();
    }
    refresh() {
        const formatter = this.props_.get('formatter');
        this.inputElement.value = formatter(this.value_.rawValue);
    }
    onChange_() {
        this.refresh();
    }
}

class TextController {
    constructor(doc, config) {
        this.onInputChange_ = this.onInputChange_.bind(this);
        this.parser_ = config.parser;
        this.props = config.props;
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new TextView(doc, {
            props: config.props,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.inputElement.addEventListener('change', this.onInputChange_);
    }
    onInputChange_(e) {
        const inputElem = forceCast(e.currentTarget);
        const value = inputElem.value;
        const parsedValue = this.parser_(value);
        if (!isEmpty(parsedValue)) {
            this.value.rawValue = parsedValue;
        }
        this.view.refresh();
    }
}

function boolToString(value) {
    return String(value);
}
function boolFromUnknown(value) {
    if (value === 'false') {
        return false;
    }
    return !!value;
}
function BooleanFormatter(value) {
    return boolToString(value);
}

function composeParsers(parsers) {
    return (text) => {
        return parsers.reduce((result, parser) => {
            if (result !== null) {
                return result;
            }
            return parser(text);
        }, null);
    };
}

const innerFormatter = createNumberFormatter(0);
function formatPercentage(value) {
    return innerFormatter(value) + '%';
}

function stringFromUnknown(value) {
    return String(value);
}
function formatString(value) {
    return value;
}

function connectValues({ primary, secondary, forward, backward, }) {
    let changing = false;
    function preventFeedback(callback) {
        if (changing) {
            return;
        }
        changing = true;
        callback();
        changing = false;
    }
    primary.emitter.on('change', (ev) => {
        preventFeedback(() => {
            secondary.setRawValue(forward(primary.rawValue, secondary.rawValue), ev.options);
        });
    });
    secondary.emitter.on('change', (ev) => {
        preventFeedback(() => {
            primary.setRawValue(backward(primary.rawValue, secondary.rawValue), ev.options);
        });
        preventFeedback(() => {
            secondary.setRawValue(forward(primary.rawValue, secondary.rawValue), ev.options);
        });
    });
    preventFeedback(() => {
        secondary.setRawValue(forward(primary.rawValue, secondary.rawValue), {
            forceEmit: false,
            last: true,
        });
    });
}

function getStepForKey(keyScale, keys) {
    const step = keyScale * (keys.altKey ? 0.1 : 1) * (keys.shiftKey ? 10 : 1);
    if (keys.upKey) {
        return +step;
    }
    else if (keys.downKey) {
        return -step;
    }
    return 0;
}
function getVerticalStepKeys(ev) {
    return {
        altKey: ev.altKey,
        downKey: ev.key === 'ArrowDown',
        shiftKey: ev.shiftKey,
        upKey: ev.key === 'ArrowUp',
    };
}
function getHorizontalStepKeys(ev) {
    return {
        altKey: ev.altKey,
        downKey: ev.key === 'ArrowLeft',
        shiftKey: ev.shiftKey,
        upKey: ev.key === 'ArrowRight',
    };
}
function isVerticalArrowKey(key) {
    return key === 'ArrowUp' || key === 'ArrowDown';
}
function isArrowKey(key) {
    return isVerticalArrowKey(key) || key === 'ArrowLeft' || key === 'ArrowRight';
}

function computeOffset$1(ev, elem) {
    var _a, _b;
    const win = elem.ownerDocument.defaultView;
    const rect = elem.getBoundingClientRect();
    return {
        x: ev.pageX - (((_a = (win && win.scrollX)) !== null && _a !== void 0 ? _a : 0) + rect.left),
        y: ev.pageY - (((_b = (win && win.scrollY)) !== null && _b !== void 0 ? _b : 0) + rect.top),
    };
}
class PointerHandler {
    constructor(element) {
        this.lastTouch_ = null;
        this.onDocumentMouseMove_ = this.onDocumentMouseMove_.bind(this);
        this.onDocumentMouseUp_ = this.onDocumentMouseUp_.bind(this);
        this.onMouseDown_ = this.onMouseDown_.bind(this);
        this.onTouchEnd_ = this.onTouchEnd_.bind(this);
        this.onTouchMove_ = this.onTouchMove_.bind(this);
        this.onTouchStart_ = this.onTouchStart_.bind(this);
        this.elem_ = element;
        this.emitter = new Emitter();
        element.addEventListener('touchstart', this.onTouchStart_, {
            passive: false,
        });
        element.addEventListener('touchmove', this.onTouchMove_, {
            passive: true,
        });
        element.addEventListener('touchend', this.onTouchEnd_);
        element.addEventListener('mousedown', this.onMouseDown_);
    }
    computePosition_(offset) {
        const rect = this.elem_.getBoundingClientRect();
        return {
            bounds: {
                width: rect.width,
                height: rect.height,
            },
            point: offset
                ? {
                    x: offset.x,
                    y: offset.y,
                }
                : null,
        };
    }
    onMouseDown_(ev) {
        var _a;
        ev.preventDefault();
        (_a = ev.currentTarget) === null || _a === void 0 ? void 0 : _a.focus();
        const doc = this.elem_.ownerDocument;
        doc.addEventListener('mousemove', this.onDocumentMouseMove_);
        doc.addEventListener('mouseup', this.onDocumentMouseUp_);
        this.emitter.emit('down', {
            altKey: ev.altKey,
            data: this.computePosition_(computeOffset$1(ev, this.elem_)),
            sender: this,
            shiftKey: ev.shiftKey,
        });
    }
    onDocumentMouseMove_(ev) {
        this.emitter.emit('move', {
            altKey: ev.altKey,
            data: this.computePosition_(computeOffset$1(ev, this.elem_)),
            sender: this,
            shiftKey: ev.shiftKey,
        });
    }
    onDocumentMouseUp_(ev) {
        const doc = this.elem_.ownerDocument;
        doc.removeEventListener('mousemove', this.onDocumentMouseMove_);
        doc.removeEventListener('mouseup', this.onDocumentMouseUp_);
        this.emitter.emit('up', {
            altKey: ev.altKey,
            data: this.computePosition_(computeOffset$1(ev, this.elem_)),
            sender: this,
            shiftKey: ev.shiftKey,
        });
    }
    onTouchStart_(ev) {
        ev.preventDefault();
        const touch = ev.targetTouches.item(0);
        const rect = this.elem_.getBoundingClientRect();
        this.emitter.emit('down', {
            altKey: ev.altKey,
            data: this.computePosition_(touch
                ? {
                    x: touch.clientX - rect.left,
                    y: touch.clientY - rect.top,
                }
                : undefined),
            sender: this,
            shiftKey: ev.shiftKey,
        });
        this.lastTouch_ = touch;
    }
    onTouchMove_(ev) {
        const touch = ev.targetTouches.item(0);
        const rect = this.elem_.getBoundingClientRect();
        this.emitter.emit('move', {
            altKey: ev.altKey,
            data: this.computePosition_(touch
                ? {
                    x: touch.clientX - rect.left,
                    y: touch.clientY - rect.top,
                }
                : undefined),
            sender: this,
            shiftKey: ev.shiftKey,
        });
        this.lastTouch_ = touch;
    }
    onTouchEnd_(ev) {
        var _a;
        const touch = (_a = ev.targetTouches.item(0)) !== null && _a !== void 0 ? _a : this.lastTouch_;
        const rect = this.elem_.getBoundingClientRect();
        this.emitter.emit('up', {
            altKey: ev.altKey,
            data: this.computePosition_(touch
                ? {
                    x: touch.clientX - rect.left,
                    y: touch.clientY - rect.top,
                }
                : undefined),
            sender: this,
            shiftKey: ev.shiftKey,
        });
    }
}

const cn$g = ClassName('txt');
class NumberTextView {
    constructor(doc, config) {
        this.onChange_ = this.onChange_.bind(this);
        this.props_ = config.props;
        this.props_.emitter.on('change', this.onChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$g(), cn$g(undefined, 'num'));
        if (config.arrayPosition) {
            this.element.classList.add(cn$g(undefined, config.arrayPosition));
        }
        config.viewProps.bindClassModifiers(this.element);
        const inputElem = doc.createElement('input');
        inputElem.classList.add(cn$g('i'));
        inputElem.type = 'text';
        config.viewProps.bindDisabled(inputElem);
        this.element.appendChild(inputElem);
        this.inputElement = inputElem;
        this.onDraggingChange_ = this.onDraggingChange_.bind(this);
        this.dragging_ = config.dragging;
        this.dragging_.emitter.on('change', this.onDraggingChange_);
        this.element.classList.add(cn$g());
        this.inputElement.classList.add(cn$g('i'));
        const knobElem = doc.createElement('div');
        knobElem.classList.add(cn$g('k'));
        this.element.appendChild(knobElem);
        this.knobElement = knobElem;
        const guideElem = doc.createElementNS(SVG_NS, 'svg');
        guideElem.classList.add(cn$g('g'));
        this.knobElement.appendChild(guideElem);
        const bodyElem = doc.createElementNS(SVG_NS, 'path');
        bodyElem.classList.add(cn$g('gb'));
        guideElem.appendChild(bodyElem);
        this.guideBodyElem_ = bodyElem;
        const headElem = doc.createElementNS(SVG_NS, 'path');
        headElem.classList.add(cn$g('gh'));
        guideElem.appendChild(headElem);
        this.guideHeadElem_ = headElem;
        const tooltipElem = doc.createElement('div');
        tooltipElem.classList.add(ClassName('tt')());
        this.knobElement.appendChild(tooltipElem);
        this.tooltipElem_ = tooltipElem;
        config.value.emitter.on('change', this.onChange_);
        this.value = config.value;
        this.refresh();
    }
    onDraggingChange_(ev) {
        if (ev.rawValue === null) {
            this.element.classList.remove(cn$g(undefined, 'drg'));
            return;
        }
        this.element.classList.add(cn$g(undefined, 'drg'));
        const x = ev.rawValue / this.props_.get('pointerScale');
        const aox = x + (x > 0 ? -1 : x < 0 ? +1 : 0);
        const adx = constrainRange(-aox, -4, +4);
        this.guideHeadElem_.setAttributeNS(null, 'd', [`M ${aox + adx},0 L${aox},4 L${aox + adx},8`, `M ${x},-1 L${x},9`].join(' '));
        this.guideBodyElem_.setAttributeNS(null, 'd', `M 0,4 L${x},4`);
        const formatter = this.props_.get('formatter');
        this.tooltipElem_.textContent = formatter(this.value.rawValue);
        this.tooltipElem_.style.left = `${x}px`;
    }
    refresh() {
        const formatter = this.props_.get('formatter');
        this.inputElement.value = formatter(this.value.rawValue);
    }
    onChange_() {
        this.refresh();
    }
}

class NumberTextController {
    constructor(doc, config) {
        var _a;
        this.originRawValue_ = 0;
        this.onInputChange_ = this.onInputChange_.bind(this);
        this.onInputKeyDown_ = this.onInputKeyDown_.bind(this);
        this.onInputKeyUp_ = this.onInputKeyUp_.bind(this);
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.parser_ = config.parser;
        this.props = config.props;
        this.sliderProps_ = (_a = config.sliderProps) !== null && _a !== void 0 ? _a : null;
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.dragging_ = createValue(null);
        this.view = new NumberTextView(doc, {
            arrayPosition: config.arrayPosition,
            dragging: this.dragging_,
            props: this.props,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.inputElement.addEventListener('change', this.onInputChange_);
        this.view.inputElement.addEventListener('keydown', this.onInputKeyDown_);
        this.view.inputElement.addEventListener('keyup', this.onInputKeyUp_);
        const ph = new PointerHandler(this.view.knobElement);
        ph.emitter.on('down', this.onPointerDown_);
        ph.emitter.on('move', this.onPointerMove_);
        ph.emitter.on('up', this.onPointerUp_);
    }
    constrainValue_(value) {
        var _a, _b;
        const min = (_a = this.sliderProps_) === null || _a === void 0 ? void 0 : _a.get('min');
        const max = (_b = this.sliderProps_) === null || _b === void 0 ? void 0 : _b.get('max');
        let v = value;
        if (min !== undefined) {
            v = Math.max(v, min);
        }
        if (max !== undefined) {
            v = Math.min(v, max);
        }
        return v;
    }
    onInputChange_(e) {
        const inputElem = forceCast(e.currentTarget);
        const value = inputElem.value;
        const parsedValue = this.parser_(value);
        if (!isEmpty(parsedValue)) {
            this.value.rawValue = this.constrainValue_(parsedValue);
        }
        this.view.refresh();
    }
    onInputKeyDown_(ev) {
        const step = getStepForKey(this.props.get('keyScale'), getVerticalStepKeys(ev));
        if (step === 0) {
            return;
        }
        this.value.setRawValue(this.constrainValue_(this.value.rawValue + step), {
            forceEmit: false,
            last: false,
        });
    }
    onInputKeyUp_(ev) {
        const step = getStepForKey(this.props.get('keyScale'), getVerticalStepKeys(ev));
        if (step === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
    onPointerDown_() {
        this.originRawValue_ = this.value.rawValue;
        this.dragging_.rawValue = 0;
    }
    computeDraggingValue_(data) {
        if (!data.point) {
            return null;
        }
        const dx = data.point.x - data.bounds.width / 2;
        return this.constrainValue_(this.originRawValue_ + dx * this.props.get('pointerScale'));
    }
    onPointerMove_(ev) {
        const v = this.computeDraggingValue_(ev.data);
        if (v === null) {
            return;
        }
        this.value.setRawValue(v, {
            forceEmit: false,
            last: false,
        });
        this.dragging_.rawValue = this.value.rawValue - this.originRawValue_;
    }
    onPointerUp_(ev) {
        const v = this.computeDraggingValue_(ev.data);
        if (v === null) {
            return;
        }
        this.value.setRawValue(v, {
            forceEmit: true,
            last: true,
        });
        this.dragging_.rawValue = null;
    }
}

const cn$f = ClassName('sld');
class SliderView {
    constructor(doc, config) {
        this.onChange_ = this.onChange_.bind(this);
        this.props_ = config.props;
        this.props_.emitter.on('change', this.onChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$f());
        config.viewProps.bindClassModifiers(this.element);
        const trackElem = doc.createElement('div');
        trackElem.classList.add(cn$f('t'));
        config.viewProps.bindTabIndex(trackElem);
        this.element.appendChild(trackElem);
        this.trackElement = trackElem;
        const knobElem = doc.createElement('div');
        knobElem.classList.add(cn$f('k'));
        this.trackElement.appendChild(knobElem);
        this.knobElement = knobElem;
        config.value.emitter.on('change', this.onChange_);
        this.value = config.value;
        this.update_();
    }
    update_() {
        const p = constrainRange(mapRange(this.value.rawValue, this.props_.get('min'), this.props_.get('max'), 0, 100), 0, 100);
        this.knobElement.style.width = `${p}%`;
    }
    onChange_() {
        this.update_();
    }
}

class SliderController {
    constructor(doc, config) {
        this.onKeyDown_ = this.onKeyDown_.bind(this);
        this.onKeyUp_ = this.onKeyUp_.bind(this);
        this.onPointerDownOrMove_ = this.onPointerDownOrMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.props = config.props;
        this.view = new SliderView(doc, {
            props: this.props,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.ptHandler_ = new PointerHandler(this.view.trackElement);
        this.ptHandler_.emitter.on('down', this.onPointerDownOrMove_);
        this.ptHandler_.emitter.on('move', this.onPointerDownOrMove_);
        this.ptHandler_.emitter.on('up', this.onPointerUp_);
        this.view.trackElement.addEventListener('keydown', this.onKeyDown_);
        this.view.trackElement.addEventListener('keyup', this.onKeyUp_);
    }
    handlePointerEvent_(d, opts) {
        if (!d.point) {
            return;
        }
        this.value.setRawValue(mapRange(constrainRange(d.point.x, 0, d.bounds.width), 0, d.bounds.width, this.props.get('min'), this.props.get('max')), opts);
    }
    onPointerDownOrMove_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: true,
            last: true,
        });
    }
    onKeyDown_(ev) {
        const step = getStepForKey(this.props.get('keyScale'), getHorizontalStepKeys(ev));
        if (step === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue + step, {
            forceEmit: false,
            last: false,
        });
    }
    onKeyUp_(ev) {
        const step = getStepForKey(this.props.get('keyScale'), getHorizontalStepKeys(ev));
        if (step === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
}

const cn$e = ClassName('sldtxt');
class SliderTextView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$e());
        const sliderElem = doc.createElement('div');
        sliderElem.classList.add(cn$e('s'));
        this.sliderView_ = config.sliderView;
        sliderElem.appendChild(this.sliderView_.element);
        this.element.appendChild(sliderElem);
        const textElem = doc.createElement('div');
        textElem.classList.add(cn$e('t'));
        this.textView_ = config.textView;
        textElem.appendChild(this.textView_.element);
        this.element.appendChild(textElem);
    }
}

class SliderTextController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.sliderC_ = new SliderController(doc, {
            props: config.sliderProps,
            value: config.value,
            viewProps: this.viewProps,
        });
        this.textC_ = new NumberTextController(doc, {
            parser: config.parser,
            props: config.textProps,
            sliderProps: config.sliderProps,
            value: config.value,
            viewProps: config.viewProps,
        });
        this.view = new SliderTextView(doc, {
            sliderView: this.sliderC_.view,
            textView: this.textC_.view,
        });
    }
    get sliderController() {
        return this.sliderC_;
    }
    get textController() {
        return this.textC_;
    }
    importProps(state) {
        return importBladeState(state, null, (p) => ({
            max: p.required.number,
            min: p.required.number,
        }), (result) => {
            const sliderProps = this.sliderC_.props;
            sliderProps.set('max', result.max);
            sliderProps.set('min', result.min);
            return true;
        });
    }
    exportProps() {
        const sliderProps = this.sliderC_.props;
        return exportBladeState(null, {
            max: sliderProps.get('max'),
            min: sliderProps.get('min'),
        });
    }
}
function createSliderTextProps(config) {
    return {
        sliderProps: new ValueMap({
            keyScale: config.keyScale,
            max: config.max,
            min: config.min,
        }),
        textProps: new ValueMap({
            formatter: createValue(config.formatter),
            keyScale: config.keyScale,
            pointerScale: createValue(config.pointerScale),
        }),
    };
}

const CSS_VAR_MAP = {
    containerUnitSize: 'cnt-usz',
};
function getCssVar(key) {
    return `--${CSS_VAR_MAP[key]}`;
}

class PlainView {
    constructor(doc, config) {
        const cn = ClassName(config.viewName);
        this.element = doc.createElement('div');
        this.element.classList.add(cn());
        config.viewProps.bindClassModifiers(this.element);
    }
}

function createPointDimensionParser(p) {
    return createNumberTextInputParamsParser(p);
}
function parsePointDimensionParams(value) {
    if (!isRecord(value)) {
        return undefined;
    }
    return parseRecord(value, createPointDimensionParser);
}
function createDimensionConstraint(params, initialValue) {
    if (!params) {
        return undefined;
    }
    const constraints = [];
    const cs = createStepConstraint(params, initialValue);
    if (cs) {
        constraints.push(cs);
    }
    const rs = createRangeConstraint(params);
    if (rs) {
        constraints.push(rs);
    }
    return new CompositeConstraint(constraints);
}

function parsePickerLayout(value) {
    if (value === 'inline' || value === 'popup') {
        return value;
    }
    return undefined;
}

function writePrimitive(target, value) {
    target.write(value);
}

const cn$d = ClassName('ckb');
class CheckboxView {
    constructor(doc, config) {
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$d());
        config.viewProps.bindClassModifiers(this.element);
        const labelElem = doc.createElement('label');
        labelElem.classList.add(cn$d('l'));
        this.element.appendChild(labelElem);
        const inputElem = doc.createElement('input');
        inputElem.classList.add(cn$d('i'));
        inputElem.type = 'checkbox';
        labelElem.appendChild(inputElem);
        this.inputElement = inputElem;
        config.viewProps.bindDisabled(this.inputElement);
        const wrapperElem = doc.createElement('div');
        wrapperElem.classList.add(cn$d('w'));
        labelElem.appendChild(wrapperElem);
        const markElem = createSvgIconElement(doc, 'check');
        wrapperElem.appendChild(markElem);
        config.value.emitter.on('change', this.onValueChange_);
        this.value = config.value;
        this.update_();
    }
    update_() {
        this.inputElement.checked = this.value.rawValue;
    }
    onValueChange_() {
        this.update_();
    }
}

class CheckboxController {
    constructor(doc, config) {
        this.onInputChange_ = this.onInputChange_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new CheckboxView(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.inputElement.addEventListener('change', this.onInputChange_);
    }
    onInputChange_(e) {
        const inputElem = forceCast(e.currentTarget);
        this.value.rawValue = inputElem.checked;
    }
}

function createConstraint$8(params) {
    const constraints = [];
    const lc = createListConstraint(params.options);
    if (lc) {
        constraints.push(lc);
    }
    return new CompositeConstraint(constraints);
}
createPlugin({
    id: 'input-bool',
    type: 'input',
    accept: (value, params) => {
        if (typeof value !== 'boolean') {
            return null;
        }
        const result = parseRecord(params, (p) => ({
            options: p.optional.custom(parseListOptions),
            readonly: p.optional.constant(false),
        }));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => boolFromUnknown,
        constraint: (args) => createConstraint$8(args.params),
        writer: (_args) => writePrimitive,
    },
    controller: (args) => {
        const doc = args.document;
        const value = args.value;
        const c = args.constraint;
        const lc = c && findConstraint(c, ListConstraint);
        if (lc) {
            return new ListController(doc, {
                props: new ValueMap({
                    options: lc.values.value('options'),
                }),
                value: value,
                viewProps: args.viewProps,
            });
        }
        return new CheckboxController(doc, {
            value: value,
            viewProps: args.viewProps,
        });
    },
    api(args) {
        if (typeof args.controller.value.rawValue !== 'boolean') {
            return null;
        }
        if (args.controller.valueController instanceof ListController) {
            return new ListInputBindingApi(args.controller);
        }
        return null;
    },
});

const cn$c = ClassName('col');
class ColorView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$c());
        config.foldable.bindExpandedClass(this.element, cn$c(undefined, 'expanded'));
        bindValueMap(config.foldable, 'completed', valueToClassName(this.element, cn$c(undefined, 'cpl')));
        const headElem = doc.createElement('div');
        headElem.classList.add(cn$c('h'));
        this.element.appendChild(headElem);
        const swatchElem = doc.createElement('div');
        swatchElem.classList.add(cn$c('s'));
        headElem.appendChild(swatchElem);
        this.swatchElement = swatchElem;
        const textElem = doc.createElement('div');
        textElem.classList.add(cn$c('t'));
        headElem.appendChild(textElem);
        this.textElement = textElem;
        if (config.pickerLayout === 'inline') {
            const pickerElem = doc.createElement('div');
            pickerElem.classList.add(cn$c('p'));
            this.element.appendChild(pickerElem);
            this.pickerElement = pickerElem;
        }
        else {
            this.pickerElement = null;
        }
    }
}

function rgbToHslInt(r, g, b) {
    const rp = constrainRange(r / 255, 0, 1);
    const gp = constrainRange(g / 255, 0, 1);
    const bp = constrainRange(b / 255, 0, 1);
    const cmax = Math.max(rp, gp, bp);
    const cmin = Math.min(rp, gp, bp);
    const c = cmax - cmin;
    let h = 0;
    let s = 0;
    const l = (cmin + cmax) / 2;
    if (c !== 0) {
        s = c / (1 - Math.abs(cmax + cmin - 1));
        if (rp === cmax) {
            h = (gp - bp) / c;
        }
        else if (gp === cmax) {
            h = 2 + (bp - rp) / c;
        }
        else {
            h = 4 + (rp - gp) / c;
        }
        h = h / 6 + (h < 0 ? 1 : 0);
    }
    return [h * 360, s * 100, l * 100];
}
function hslToRgbInt(h, s, l) {
    const hp = ((h % 360) + 360) % 360;
    const sp = constrainRange(s / 100, 0, 1);
    const lp = constrainRange(l / 100, 0, 1);
    const c = (1 - Math.abs(2 * lp - 1)) * sp;
    const x = c * (1 - Math.abs(((hp / 60) % 2) - 1));
    const m = lp - c / 2;
    let rp, gp, bp;
    if (hp >= 0 && hp < 60) {
        [rp, gp, bp] = [c, x, 0];
    }
    else if (hp >= 60 && hp < 120) {
        [rp, gp, bp] = [x, c, 0];
    }
    else if (hp >= 120 && hp < 180) {
        [rp, gp, bp] = [0, c, x];
    }
    else if (hp >= 180 && hp < 240) {
        [rp, gp, bp] = [0, x, c];
    }
    else if (hp >= 240 && hp < 300) {
        [rp, gp, bp] = [x, 0, c];
    }
    else {
        [rp, gp, bp] = [c, 0, x];
    }
    return [(rp + m) * 255, (gp + m) * 255, (bp + m) * 255];
}
function rgbToHsvInt(r, g, b) {
    const rp = constrainRange(r / 255, 0, 1);
    const gp = constrainRange(g / 255, 0, 1);
    const bp = constrainRange(b / 255, 0, 1);
    const cmax = Math.max(rp, gp, bp);
    const cmin = Math.min(rp, gp, bp);
    const d = cmax - cmin;
    let h;
    if (d === 0) {
        h = 0;
    }
    else if (cmax === rp) {
        h = 60 * (((((gp - bp) / d) % 6) + 6) % 6);
    }
    else if (cmax === gp) {
        h = 60 * ((bp - rp) / d + 2);
    }
    else {
        h = 60 * ((rp - gp) / d + 4);
    }
    const s = cmax === 0 ? 0 : d / cmax;
    const v = cmax;
    return [h, s * 100, v * 100];
}
function hsvToRgbInt(h, s, v) {
    const hp = loopRange(h, 360);
    const sp = constrainRange(s / 100, 0, 1);
    const vp = constrainRange(v / 100, 0, 1);
    const c = vp * sp;
    const x = c * (1 - Math.abs(((hp / 60) % 2) - 1));
    const m = vp - c;
    let rp, gp, bp;
    if (hp >= 0 && hp < 60) {
        [rp, gp, bp] = [c, x, 0];
    }
    else if (hp >= 60 && hp < 120) {
        [rp, gp, bp] = [x, c, 0];
    }
    else if (hp >= 120 && hp < 180) {
        [rp, gp, bp] = [0, c, x];
    }
    else if (hp >= 180 && hp < 240) {
        [rp, gp, bp] = [0, x, c];
    }
    else if (hp >= 240 && hp < 300) {
        [rp, gp, bp] = [x, 0, c];
    }
    else {
        [rp, gp, bp] = [c, 0, x];
    }
    return [(rp + m) * 255, (gp + m) * 255, (bp + m) * 255];
}
function hslToHsvInt(h, s, l) {
    const sd = l + (s * (100 - Math.abs(2 * l - 100))) / (2 * 100);
    return [
        h,
        sd !== 0 ? (s * (100 - Math.abs(2 * l - 100))) / sd : 0,
        l + (s * (100 - Math.abs(2 * l - 100))) / (2 * 100),
    ];
}
function hsvToHslInt(h, s, v) {
    const sd = 100 - Math.abs((v * (200 - s)) / 100 - 100);
    return [h, sd !== 0 ? (s * v) / sd : 0, (v * (200 - s)) / (2 * 100)];
}
function removeAlphaComponent(comps) {
    return [comps[0], comps[1], comps[2]];
}
function appendAlphaComponent(comps, alpha) {
    return [comps[0], comps[1], comps[2], alpha];
}
const MODE_CONVERTER_MAP = {
    hsl: {
        hsl: (h, s, l) => [h, s, l],
        hsv: hslToHsvInt,
        rgb: hslToRgbInt,
    },
    hsv: {
        hsl: hsvToHslInt,
        hsv: (h, s, v) => [h, s, v],
        rgb: hsvToRgbInt,
    },
    rgb: {
        hsl: rgbToHslInt,
        hsv: rgbToHsvInt,
        rgb: (r, g, b) => [r, g, b],
    },
};
function getColorMaxComponents(mode, type) {
    return [
        type === 'float' ? 1 : mode === 'rgb' ? 255 : 360,
        type === 'float' ? 1 : mode === 'rgb' ? 255 : 100,
        type === 'float' ? 1 : mode === 'rgb' ? 255 : 100,
    ];
}
function loopHueRange(hue, max) {
    return hue === max ? max : loopRange(hue, max);
}
function constrainColorComponents(components, mode, type) {
    var _a;
    const ms = getColorMaxComponents(mode, type);
    return [
        mode === 'rgb'
            ? constrainRange(components[0], 0, ms[0])
            : loopHueRange(components[0], ms[0]),
        constrainRange(components[1], 0, ms[1]),
        constrainRange(components[2], 0, ms[2]),
        constrainRange((_a = components[3]) !== null && _a !== void 0 ? _a : 1, 0, 1),
    ];
}
function convertColorType(comps, mode, from, to) {
    const fms = getColorMaxComponents(mode, from);
    const tms = getColorMaxComponents(mode, to);
    return comps.map((c, index) => (c / fms[index]) * tms[index]);
}
function convertColor(components, from, to) {
    const intComps = convertColorType(components, from.mode, from.type, 'int');
    const result = MODE_CONVERTER_MAP[from.mode][to.mode](...intComps);
    return convertColorType(result, to.mode, 'int', to.type);
}

class IntColor {
    static black() {
        return new IntColor([0, 0, 0], 'rgb');
    }
    constructor(comps, mode) {
        this.type = 'int';
        this.mode = mode;
        this.comps_ = constrainColorComponents(comps, mode, this.type);
    }
    getComponents(opt_mode) {
        return appendAlphaComponent(convertColor(removeAlphaComponent(this.comps_), { mode: this.mode, type: this.type }, { mode: opt_mode !== null && opt_mode !== void 0 ? opt_mode : this.mode, type: this.type }), this.comps_[3]);
    }
    toRgbaObject() {
        const rgbComps = this.getComponents('rgb');
        return {
            r: rgbComps[0],
            g: rgbComps[1],
            b: rgbComps[2],
            a: rgbComps[3],
        };
    }
}

const cn$b = ClassName('colp');
class ColorPickerView {
    constructor(doc, config) {
        this.alphaViews_ = null;
        this.element = doc.createElement('div');
        this.element.classList.add(cn$b());
        config.viewProps.bindClassModifiers(this.element);
        const hsvElem = doc.createElement('div');
        hsvElem.classList.add(cn$b('hsv'));
        const svElem = doc.createElement('div');
        svElem.classList.add(cn$b('sv'));
        this.svPaletteView_ = config.svPaletteView;
        svElem.appendChild(this.svPaletteView_.element);
        hsvElem.appendChild(svElem);
        const hElem = doc.createElement('div');
        hElem.classList.add(cn$b('h'));
        this.hPaletteView_ = config.hPaletteView;
        hElem.appendChild(this.hPaletteView_.element);
        hsvElem.appendChild(hElem);
        this.element.appendChild(hsvElem);
        const rgbElem = doc.createElement('div');
        rgbElem.classList.add(cn$b('rgb'));
        this.textsView_ = config.textsView;
        rgbElem.appendChild(this.textsView_.element);
        this.element.appendChild(rgbElem);
        if (config.alphaViews) {
            this.alphaViews_ = {
                palette: config.alphaViews.palette,
                text: config.alphaViews.text,
            };
            const aElem = doc.createElement('div');
            aElem.classList.add(cn$b('a'));
            const apElem = doc.createElement('div');
            apElem.classList.add(cn$b('ap'));
            apElem.appendChild(this.alphaViews_.palette.element);
            aElem.appendChild(apElem);
            const atElem = doc.createElement('div');
            atElem.classList.add(cn$b('at'));
            atElem.appendChild(this.alphaViews_.text.element);
            aElem.appendChild(atElem);
            this.element.appendChild(aElem);
        }
    }
    get allFocusableElements() {
        const elems = [
            this.svPaletteView_.element,
            this.hPaletteView_.element,
            this.textsView_.modeSelectElement,
            ...this.textsView_.inputViews.map((v) => v.inputElement),
        ];
        if (this.alphaViews_) {
            elems.push(this.alphaViews_.palette.element, this.alphaViews_.text.inputElement);
        }
        return elems;
    }
}

function parseColorType(value) {
    return value === 'int' ? 'int' : value === 'float' ? 'float' : undefined;
}
function parseColorInputParams(params) {
    return parseRecord(params, (p) => ({
        color: p.optional.object({
            alpha: p.optional.boolean,
            type: p.optional.custom(parseColorType),
        }),
        expanded: p.optional.boolean,
        picker: p.optional.custom(parsePickerLayout),
        readonly: p.optional.constant(false),
    }));
}
function getKeyScaleForColor(forAlpha) {
    return forAlpha ? 0.1 : 1;
}
function extractColorType(params) {
    var _a;
    return (_a = params.color) === null || _a === void 0 ? void 0 : _a.type;
}

class FloatColor {
    constructor(comps, mode) {
        this.type = 'float';
        this.mode = mode;
        this.comps_ = constrainColorComponents(comps, mode, this.type);
    }
    getComponents(opt_mode) {
        return appendAlphaComponent(convertColor(removeAlphaComponent(this.comps_), { mode: this.mode, type: this.type }, { mode: opt_mode !== null && opt_mode !== void 0 ? opt_mode : this.mode, type: this.type }), this.comps_[3]);
    }
    toRgbaObject() {
        const rgbComps = this.getComponents('rgb');
        return {
            r: rgbComps[0],
            g: rgbComps[1],
            b: rgbComps[2],
            a: rgbComps[3],
        };
    }
}

const TYPE_TO_CONSTRUCTOR_MAP = {
    int: (comps, mode) => new IntColor(comps, mode),
    float: (comps, mode) => new FloatColor(comps, mode),
};
function createColor(comps, mode, type) {
    return TYPE_TO_CONSTRUCTOR_MAP[type](comps, mode);
}
function isFloatColor(c) {
    return c.type === 'float';
}
function isIntColor(c) {
    return c.type === 'int';
}
function convertFloatToInt(cf) {
    const comps = cf.getComponents();
    const ms = getColorMaxComponents(cf.mode, 'int');
    return new IntColor([
        Math.round(mapRange(comps[0], 0, 1, 0, ms[0])),
        Math.round(mapRange(comps[1], 0, 1, 0, ms[1])),
        Math.round(mapRange(comps[2], 0, 1, 0, ms[2])),
        comps[3],
    ], cf.mode);
}
function convertIntToFloat(ci) {
    const comps = ci.getComponents();
    const ms = getColorMaxComponents(ci.mode, 'int');
    return new FloatColor([
        mapRange(comps[0], 0, ms[0], 0, 1),
        mapRange(comps[1], 0, ms[1], 0, 1),
        mapRange(comps[2], 0, ms[2], 0, 1),
        comps[3],
    ], ci.mode);
}
function mapColorType(c, type) {
    if (c.type === type) {
        return c;
    }
    if (isIntColor(c) && type === 'float') {
        return convertIntToFloat(c);
    }
    if (isFloatColor(c) && type === 'int') {
        return convertFloatToInt(c);
    }
    throw TpError.shouldNeverHappen();
}

function equalsStringColorFormat(f1, f2) {
    return (f1.alpha === f2.alpha &&
        f1.mode === f2.mode &&
        f1.notation === f2.notation &&
        f1.type === f2.type);
}
function parseCssNumberOrPercentage(text, max) {
    const m = text.match(/^(.+)%$/);
    if (!m) {
        return Math.min(parseFloat(text), max);
    }
    return Math.min(parseFloat(m[1]) * 0.01 * max, max);
}
const ANGLE_TO_DEG_MAP = {
    deg: (angle) => angle,
    grad: (angle) => (angle * 360) / 400,
    rad: (angle) => (angle * 360) / (2 * Math.PI),
    turn: (angle) => angle * 360,
};
function parseCssNumberOrAngle(text) {
    const m = text.match(/^([0-9.]+?)(deg|grad|rad|turn)$/);
    if (!m) {
        return parseFloat(text);
    }
    const angle = parseFloat(m[1]);
    const unit = m[2];
    return ANGLE_TO_DEG_MAP[unit](angle);
}
function parseFunctionalRgbColorComponents(text) {
    const m = text.match(/^rgb\(\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*\)$/);
    if (!m) {
        return null;
    }
    const comps = [
        parseCssNumberOrPercentage(m[1], 255),
        parseCssNumberOrPercentage(m[2], 255),
        parseCssNumberOrPercentage(m[3], 255),
    ];
    if (isNaN(comps[0]) || isNaN(comps[1]) || isNaN(comps[2])) {
        return null;
    }
    return comps;
}
function parseFunctionalRgbColor(text) {
    const comps = parseFunctionalRgbColorComponents(text);
    return comps ? new IntColor(comps, 'rgb') : null;
}
function parseFunctionalRgbaColorComponents(text) {
    const m = text.match(/^rgba\(\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*\)$/);
    if (!m) {
        return null;
    }
    const comps = [
        parseCssNumberOrPercentage(m[1], 255),
        parseCssNumberOrPercentage(m[2], 255),
        parseCssNumberOrPercentage(m[3], 255),
        parseCssNumberOrPercentage(m[4], 1),
    ];
    if (isNaN(comps[0]) ||
        isNaN(comps[1]) ||
        isNaN(comps[2]) ||
        isNaN(comps[3])) {
        return null;
    }
    return comps;
}
function parseFunctionalRgbaColor(text) {
    const comps = parseFunctionalRgbaColorComponents(text);
    return comps ? new IntColor(comps, 'rgb') : null;
}
function parseFunctionalHslColorComponents(text) {
    const m = text.match(/^hsl\(\s*([0-9A-Fa-f.]+(?:deg|grad|rad|turn)?)\s*,\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*\)$/);
    if (!m) {
        return null;
    }
    const comps = [
        parseCssNumberOrAngle(m[1]),
        parseCssNumberOrPercentage(m[2], 100),
        parseCssNumberOrPercentage(m[3], 100),
    ];
    if (isNaN(comps[0]) || isNaN(comps[1]) || isNaN(comps[2])) {
        return null;
    }
    return comps;
}
function parseFunctionalHslColor(text) {
    const comps = parseFunctionalHslColorComponents(text);
    return comps ? new IntColor(comps, 'hsl') : null;
}
function parseHslaColorComponents(text) {
    const m = text.match(/^hsla\(\s*([0-9A-Fa-f.]+(?:deg|grad|rad|turn)?)\s*,\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*,\s*([0-9A-Fa-f.]+%?)\s*\)$/);
    if (!m) {
        return null;
    }
    const comps = [
        parseCssNumberOrAngle(m[1]),
        parseCssNumberOrPercentage(m[2], 100),
        parseCssNumberOrPercentage(m[3], 100),
        parseCssNumberOrPercentage(m[4], 1),
    ];
    if (isNaN(comps[0]) ||
        isNaN(comps[1]) ||
        isNaN(comps[2]) ||
        isNaN(comps[3])) {
        return null;
    }
    return comps;
}
function parseFunctionalHslaColor(text) {
    const comps = parseHslaColorComponents(text);
    return comps ? new IntColor(comps, 'hsl') : null;
}
function parseHexRgbColorComponents(text) {
    const mRgb = text.match(/^#([0-9A-Fa-f])([0-9A-Fa-f])([0-9A-Fa-f])$/);
    if (mRgb) {
        return [
            parseInt(mRgb[1] + mRgb[1], 16),
            parseInt(mRgb[2] + mRgb[2], 16),
            parseInt(mRgb[3] + mRgb[3], 16),
        ];
    }
    const mRrggbb = text.match(/^(?:#|0x)([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$/);
    if (mRrggbb) {
        return [
            parseInt(mRrggbb[1], 16),
            parseInt(mRrggbb[2], 16),
            parseInt(mRrggbb[3], 16),
        ];
    }
    return null;
}
function parseHexRgbColor(text) {
    const comps = parseHexRgbColorComponents(text);
    return comps ? new IntColor(comps, 'rgb') : null;
}
function parseHexRgbaColorComponents(text) {
    const mRgb = text.match(/^#?([0-9A-Fa-f])([0-9A-Fa-f])([0-9A-Fa-f])([0-9A-Fa-f])$/);
    if (mRgb) {
        return [
            parseInt(mRgb[1] + mRgb[1], 16),
            parseInt(mRgb[2] + mRgb[2], 16),
            parseInt(mRgb[3] + mRgb[3], 16),
            mapRange(parseInt(mRgb[4] + mRgb[4], 16), 0, 255, 0, 1),
        ];
    }
    const mRrggbb = text.match(/^(?:#|0x)?([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$/);
    if (mRrggbb) {
        return [
            parseInt(mRrggbb[1], 16),
            parseInt(mRrggbb[2], 16),
            parseInt(mRrggbb[3], 16),
            mapRange(parseInt(mRrggbb[4], 16), 0, 255, 0, 1),
        ];
    }
    return null;
}
function parseHexRgbaColor(text) {
    const comps = parseHexRgbaColorComponents(text);
    return comps ? new IntColor(comps, 'rgb') : null;
}
function parseObjectRgbColorComponents(text) {
    const m = text.match(/^\{\s*r\s*:\s*([0-9A-Fa-f.]+%?)\s*,\s*g\s*:\s*([0-9A-Fa-f.]+%?)\s*,\s*b\s*:\s*([0-9A-Fa-f.]+%?)\s*\}$/);
    if (!m) {
        return null;
    }
    const comps = [
        parseFloat(m[1]),
        parseFloat(m[2]),
        parseFloat(m[3]),
    ];
    if (isNaN(comps[0]) || isNaN(comps[1]) || isNaN(comps[2])) {
        return null;
    }
    return comps;
}
function createObjectRgbColorParser(type) {
    return (text) => {
        const comps = parseObjectRgbColorComponents(text);
        return comps ? createColor(comps, 'rgb', type) : null;
    };
}
function parseObjectRgbaColorComponents(text) {
    const m = text.match(/^\{\s*r\s*:\s*([0-9A-Fa-f.]+%?)\s*,\s*g\s*:\s*([0-9A-Fa-f.]+%?)\s*,\s*b\s*:\s*([0-9A-Fa-f.]+%?)\s*,\s*a\s*:\s*([0-9A-Fa-f.]+%?)\s*\}$/);
    if (!m) {
        return null;
    }
    const comps = [
        parseFloat(m[1]),
        parseFloat(m[2]),
        parseFloat(m[3]),
        parseFloat(m[4]),
    ];
    if (isNaN(comps[0]) ||
        isNaN(comps[1]) ||
        isNaN(comps[2]) ||
        isNaN(comps[3])) {
        return null;
    }
    return comps;
}
function createObjectRgbaColorParser(type) {
    return (text) => {
        const comps = parseObjectRgbaColorComponents(text);
        return comps ? createColor(comps, 'rgb', type) : null;
    };
}
const PARSER_AND_RESULT = [
    {
        parser: parseHexRgbColorComponents,
        result: {
            alpha: false,
            mode: 'rgb',
            notation: 'hex',
        },
    },
    {
        parser: parseHexRgbaColorComponents,
        result: {
            alpha: true,
            mode: 'rgb',
            notation: 'hex',
        },
    },
    {
        parser: parseFunctionalRgbColorComponents,
        result: {
            alpha: false,
            mode: 'rgb',
            notation: 'func',
        },
    },
    {
        parser: parseFunctionalRgbaColorComponents,
        result: {
            alpha: true,
            mode: 'rgb',
            notation: 'func',
        },
    },
    {
        parser: parseFunctionalHslColorComponents,
        result: {
            alpha: false,
            mode: 'hsl',
            notation: 'func',
        },
    },
    {
        parser: parseHslaColorComponents,
        result: {
            alpha: true,
            mode: 'hsl',
            notation: 'func',
        },
    },
    {
        parser: parseObjectRgbColorComponents,
        result: {
            alpha: false,
            mode: 'rgb',
            notation: 'object',
        },
    },
    {
        parser: parseObjectRgbaColorComponents,
        result: {
            alpha: true,
            mode: 'rgb',
            notation: 'object',
        },
    },
];
function detectStringColor(text) {
    return PARSER_AND_RESULT.reduce((prev, { parser, result: detection }) => {
        if (prev) {
            return prev;
        }
        return parser(text) ? detection : null;
    }, null);
}
function detectStringColorFormat(text, type = 'int') {
    const r = detectStringColor(text);
    if (!r) {
        return null;
    }
    if (r.notation === 'hex' && type !== 'float') {
        return Object.assign(Object.assign({}, r), { type: 'int' });
    }
    if (r.notation === 'func') {
        return Object.assign(Object.assign({}, r), { type: type });
    }
    return null;
}
function createColorStringParser(type) {
    const parsers = [
        parseHexRgbColor,
        parseHexRgbaColor,
        parseFunctionalRgbColor,
        parseFunctionalRgbaColor,
        parseFunctionalHslColor,
        parseFunctionalHslaColor,
    ];
    if (type === 'int') {
        parsers.push(createObjectRgbColorParser('int'), createObjectRgbaColorParser('int'));
    }
    if (type === 'float') {
        parsers.push(createObjectRgbColorParser('float'), createObjectRgbaColorParser('float'));
    }
    const parser = composeParsers(parsers);
    return (text) => {
        const result = parser(text);
        return result ? mapColorType(result, type) : null;
    };
}
function readIntColorString(value) {
    const parser = createColorStringParser('int');
    if (typeof value !== 'string') {
        return IntColor.black();
    }
    const result = parser(value);
    return result !== null && result !== void 0 ? result : IntColor.black();
}
function zerofill(comp) {
    const hex = constrainRange(Math.floor(comp), 0, 255).toString(16);
    return hex.length === 1 ? `0${hex}` : hex;
}
function colorToHexRgbString(value, prefix = '#') {
    const hexes = removeAlphaComponent(value.getComponents('rgb'))
        .map(zerofill)
        .join('');
    return `${prefix}${hexes}`;
}
function colorToHexRgbaString(value, prefix = '#') {
    const rgbaComps = value.getComponents('rgb');
    const hexes = [rgbaComps[0], rgbaComps[1], rgbaComps[2], rgbaComps[3] * 255]
        .map(zerofill)
        .join('');
    return `${prefix}${hexes}`;
}
function colorToFunctionalRgbString(value) {
    const formatter = createNumberFormatter(0);
    const ci = mapColorType(value, 'int');
    const comps = removeAlphaComponent(ci.getComponents('rgb')).map((comp) => formatter(comp));
    return `rgb(${comps.join(', ')})`;
}
function colorToFunctionalRgbaString(value) {
    const aFormatter = createNumberFormatter(2);
    const rgbFormatter = createNumberFormatter(0);
    const ci = mapColorType(value, 'int');
    const comps = ci.getComponents('rgb').map((comp, index) => {
        const formatter = index === 3 ? aFormatter : rgbFormatter;
        return formatter(comp);
    });
    return `rgba(${comps.join(', ')})`;
}
function colorToFunctionalHslString(value) {
    const formatters = [
        createNumberFormatter(0),
        formatPercentage,
        formatPercentage,
    ];
    const ci = mapColorType(value, 'int');
    const comps = removeAlphaComponent(ci.getComponents('hsl')).map((comp, index) => formatters[index](comp));
    return `hsl(${comps.join(', ')})`;
}
function colorToFunctionalHslaString(value) {
    const formatters = [
        createNumberFormatter(0),
        formatPercentage,
        formatPercentage,
        createNumberFormatter(2),
    ];
    const ci = mapColorType(value, 'int');
    const comps = ci
        .getComponents('hsl')
        .map((comp, index) => formatters[index](comp));
    return `hsla(${comps.join(', ')})`;
}
function colorToObjectRgbString(value, type) {
    const formatter = createNumberFormatter(type === 'float' ? 2 : 0);
    const names = ['r', 'g', 'b'];
    const cc = mapColorType(value, type);
    const comps = removeAlphaComponent(cc.getComponents('rgb')).map((comp, index) => `${names[index]}: ${formatter(comp)}`);
    return `{${comps.join(', ')}}`;
}
function createObjectRgbColorFormatter(type) {
    return (value) => colorToObjectRgbString(value, type);
}
function colorToObjectRgbaString(value, type) {
    const aFormatter = createNumberFormatter(2);
    const rgbFormatter = createNumberFormatter(type === 'float' ? 2 : 0);
    const names = ['r', 'g', 'b', 'a'];
    const cc = mapColorType(value, type);
    const comps = cc.getComponents('rgb').map((comp, index) => {
        const formatter = index === 3 ? aFormatter : rgbFormatter;
        return `${names[index]}: ${formatter(comp)}`;
    });
    return `{${comps.join(', ')}}`;
}
function createObjectRgbaColorFormatter(type) {
    return (value) => colorToObjectRgbaString(value, type);
}
const FORMAT_AND_STRINGIFIERS = [
    {
        format: {
            alpha: false,
            mode: 'rgb',
            notation: 'hex',
            type: 'int',
        },
        stringifier: colorToHexRgbString,
    },
    {
        format: {
            alpha: true,
            mode: 'rgb',
            notation: 'hex',
            type: 'int',
        },
        stringifier: colorToHexRgbaString,
    },
    {
        format: {
            alpha: false,
            mode: 'rgb',
            notation: 'func',
            type: 'int',
        },
        stringifier: colorToFunctionalRgbString,
    },
    {
        format: {
            alpha: true,
            mode: 'rgb',
            notation: 'func',
            type: 'int',
        },
        stringifier: colorToFunctionalRgbaString,
    },
    {
        format: {
            alpha: false,
            mode: 'hsl',
            notation: 'func',
            type: 'int',
        },
        stringifier: colorToFunctionalHslString,
    },
    {
        format: {
            alpha: true,
            mode: 'hsl',
            notation: 'func',
            type: 'int',
        },
        stringifier: colorToFunctionalHslaString,
    },
    ...['int', 'float'].reduce((prev, type) => {
        return [
            ...prev,
            {
                format: {
                    alpha: false,
                    mode: 'rgb',
                    notation: 'object',
                    type: type,
                },
                stringifier: createObjectRgbColorFormatter(type),
            },
            {
                format: {
                    alpha: true,
                    mode: 'rgb',
                    notation: 'object',
                    type: type,
                },
                stringifier: createObjectRgbaColorFormatter(type),
            },
        ];
    }, []),
];
function findColorStringifier(format) {
    return FORMAT_AND_STRINGIFIERS.reduce((prev, fas) => {
        if (prev) {
            return prev;
        }
        return equalsStringColorFormat(fas.format, format)
            ? fas.stringifier
            : null;
    }, null);
}

const cn$a = ClassName('apl');
class APaletteView {
    constructor(doc, config) {
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.value = config.value;
        this.value.emitter.on('change', this.onValueChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$a());
        config.viewProps.bindClassModifiers(this.element);
        config.viewProps.bindTabIndex(this.element);
        const barElem = doc.createElement('div');
        barElem.classList.add(cn$a('b'));
        this.element.appendChild(barElem);
        const colorElem = doc.createElement('div');
        colorElem.classList.add(cn$a('c'));
        barElem.appendChild(colorElem);
        this.colorElem_ = colorElem;
        const markerElem = doc.createElement('div');
        markerElem.classList.add(cn$a('m'));
        this.element.appendChild(markerElem);
        this.markerElem_ = markerElem;
        const previewElem = doc.createElement('div');
        previewElem.classList.add(cn$a('p'));
        this.markerElem_.appendChild(previewElem);
        this.previewElem_ = previewElem;
        this.update_();
    }
    update_() {
        const c = this.value.rawValue;
        const rgbaComps = c.getComponents('rgb');
        const leftColor = new IntColor([rgbaComps[0], rgbaComps[1], rgbaComps[2], 0], 'rgb');
        const rightColor = new IntColor([rgbaComps[0], rgbaComps[1], rgbaComps[2], 255], 'rgb');
        const gradientComps = [
            'to right',
            colorToFunctionalRgbaString(leftColor),
            colorToFunctionalRgbaString(rightColor),
        ];
        this.colorElem_.style.background = `linear-gradient(${gradientComps.join(',')})`;
        this.previewElem_.style.backgroundColor = colorToFunctionalRgbaString(c);
        const left = mapRange(rgbaComps[3], 0, 1, 0, 100);
        this.markerElem_.style.left = `${left}%`;
    }
    onValueChange_() {
        this.update_();
    }
}

class APaletteController {
    constructor(doc, config) {
        this.onKeyDown_ = this.onKeyDown_.bind(this);
        this.onKeyUp_ = this.onKeyUp_.bind(this);
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new APaletteView(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.ptHandler_ = new PointerHandler(this.view.element);
        this.ptHandler_.emitter.on('down', this.onPointerDown_);
        this.ptHandler_.emitter.on('move', this.onPointerMove_);
        this.ptHandler_.emitter.on('up', this.onPointerUp_);
        this.view.element.addEventListener('keydown', this.onKeyDown_);
        this.view.element.addEventListener('keyup', this.onKeyUp_);
    }
    handlePointerEvent_(d, opts) {
        if (!d.point) {
            return;
        }
        const alpha = d.point.x / d.bounds.width;
        const c = this.value.rawValue;
        const [h, s, v] = c.getComponents('hsv');
        this.value.setRawValue(new IntColor([h, s, v, alpha], 'hsv'), opts);
    }
    onPointerDown_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerMove_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: true,
            last: true,
        });
    }
    onKeyDown_(ev) {
        const step = getStepForKey(getKeyScaleForColor(true), getHorizontalStepKeys(ev));
        if (step === 0) {
            return;
        }
        const c = this.value.rawValue;
        const [h, s, v, a] = c.getComponents('hsv');
        this.value.setRawValue(new IntColor([h, s, v, a + step], 'hsv'), {
            forceEmit: false,
            last: false,
        });
    }
    onKeyUp_(ev) {
        const step = getStepForKey(getKeyScaleForColor(true), getHorizontalStepKeys(ev));
        if (step === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
}

const cn$9 = ClassName('coltxt');
function createModeSelectElement(doc) {
    const selectElem = doc.createElement('select');
    const items = [
        { text: 'RGB', value: 'rgb' },
        { text: 'HSL', value: 'hsl' },
        { text: 'HSV', value: 'hsv' },
        { text: 'HEX', value: 'hex' },
    ];
    selectElem.appendChild(items.reduce((frag, item) => {
        const optElem = doc.createElement('option');
        optElem.textContent = item.text;
        optElem.value = item.value;
        frag.appendChild(optElem);
        return frag;
    }, doc.createDocumentFragment()));
    return selectElem;
}
class ColorTextsView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$9());
        config.viewProps.bindClassModifiers(this.element);
        const modeElem = doc.createElement('div');
        modeElem.classList.add(cn$9('m'));
        this.modeElem_ = createModeSelectElement(doc);
        this.modeElem_.classList.add(cn$9('ms'));
        modeElem.appendChild(this.modeSelectElement);
        config.viewProps.bindDisabled(this.modeElem_);
        const modeMarkerElem = doc.createElement('div');
        modeMarkerElem.classList.add(cn$9('mm'));
        modeMarkerElem.appendChild(createSvgIconElement(doc, 'dropdown'));
        modeElem.appendChild(modeMarkerElem);
        this.element.appendChild(modeElem);
        const inputsElem = doc.createElement('div');
        inputsElem.classList.add(cn$9('w'));
        this.element.appendChild(inputsElem);
        this.inputsElem_ = inputsElem;
        this.inputViews_ = config.inputViews;
        this.applyInputViews_();
        bindValue(config.mode, (mode) => {
            this.modeElem_.value = mode;
        });
    }
    get modeSelectElement() {
        return this.modeElem_;
    }
    get inputViews() {
        return this.inputViews_;
    }
    set inputViews(inputViews) {
        this.inputViews_ = inputViews;
        this.applyInputViews_();
    }
    applyInputViews_() {
        removeChildElements(this.inputsElem_);
        const doc = this.element.ownerDocument;
        this.inputViews_.forEach((v) => {
            const compElem = doc.createElement('div');
            compElem.classList.add(cn$9('c'));
            compElem.appendChild(v.element);
            this.inputsElem_.appendChild(compElem);
        });
    }
}

function createFormatter$2(type) {
    return createNumberFormatter(type === 'float' ? 2 : 0);
}
function createConstraint$7(mode, type, index) {
    const max = getColorMaxComponents(mode, type)[index];
    return new DefiniteRangeConstraint({
        min: 0,
        max: max,
    });
}
function createComponentController(doc, config, index) {
    return new NumberTextController(doc, {
        arrayPosition: index === 0 ? 'fst' : index === 3 - 1 ? 'lst' : 'mid',
        parser: config.parser,
        props: ValueMap.fromObject({
            formatter: createFormatter$2(config.colorType),
            keyScale: getKeyScaleForColor(false),
            pointerScale: config.colorType === 'float' ? 0.01 : 1,
        }),
        value: createValue(0, {
            constraint: createConstraint$7(config.colorMode, config.colorType, index),
        }),
        viewProps: config.viewProps,
    });
}
function createComponentControllers(doc, config) {
    const cc = {
        colorMode: config.colorMode,
        colorType: config.colorType,
        parser: parseNumber,
        viewProps: config.viewProps,
    };
    return [0, 1, 2].map((i) => {
        const c = createComponentController(doc, cc, i);
        connectValues({
            primary: config.value,
            secondary: c.value,
            forward(p) {
                const mc = mapColorType(p, config.colorType);
                return mc.getComponents(config.colorMode)[i];
            },
            backward(p, s) {
                const pickedMode = config.colorMode;
                const mc = mapColorType(p, config.colorType);
                const comps = mc.getComponents(pickedMode);
                comps[i] = s;
                const c = createColor(appendAlphaComponent(removeAlphaComponent(comps), comps[3]), pickedMode, config.colorType);
                return mapColorType(c, 'int');
            },
        });
        return c;
    });
}
function createHexController(doc, config) {
    const c = new TextController(doc, {
        parser: createColorStringParser('int'),
        props: ValueMap.fromObject({
            formatter: colorToHexRgbString,
        }),
        value: createValue(IntColor.black()),
        viewProps: config.viewProps,
    });
    connectValues({
        primary: config.value,
        secondary: c.value,
        forward: (p) => new IntColor(removeAlphaComponent(p.getComponents()), p.mode),
        backward: (p, s) => new IntColor(appendAlphaComponent(removeAlphaComponent(s.getComponents(p.mode)), p.getComponents()[3]), p.mode),
    });
    return [c];
}
function isColorMode(mode) {
    return mode !== 'hex';
}
class ColorTextsController {
    constructor(doc, config) {
        this.onModeSelectChange_ = this.onModeSelectChange_.bind(this);
        this.colorType_ = config.colorType;
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.colorMode = createValue(this.value.rawValue.mode);
        this.ccs_ = this.createComponentControllers_(doc);
        this.view = new ColorTextsView(doc, {
            mode: this.colorMode,
            inputViews: [this.ccs_[0].view, this.ccs_[1].view, this.ccs_[2].view],
            viewProps: this.viewProps,
        });
        this.view.modeSelectElement.addEventListener('change', this.onModeSelectChange_);
    }
    createComponentControllers_(doc) {
        const mode = this.colorMode.rawValue;
        if (isColorMode(mode)) {
            return createComponentControllers(doc, {
                colorMode: mode,
                colorType: this.colorType_,
                value: this.value,
                viewProps: this.viewProps,
            });
        }
        return createHexController(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
    }
    onModeSelectChange_(ev) {
        const selectElem = ev.currentTarget;
        this.colorMode.rawValue = selectElem.value;
        this.ccs_ = this.createComponentControllers_(this.view.element.ownerDocument);
        this.view.inputViews = this.ccs_.map((cc) => cc.view);
    }
}

const cn$8 = ClassName('hpl');
class HPaletteView {
    constructor(doc, config) {
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.value = config.value;
        this.value.emitter.on('change', this.onValueChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$8());
        config.viewProps.bindClassModifiers(this.element);
        config.viewProps.bindTabIndex(this.element);
        const colorElem = doc.createElement('div');
        colorElem.classList.add(cn$8('c'));
        this.element.appendChild(colorElem);
        const markerElem = doc.createElement('div');
        markerElem.classList.add(cn$8('m'));
        this.element.appendChild(markerElem);
        this.markerElem_ = markerElem;
        this.update_();
    }
    update_() {
        const c = this.value.rawValue;
        const [h] = c.getComponents('hsv');
        this.markerElem_.style.backgroundColor = colorToFunctionalRgbString(new IntColor([h, 100, 100], 'hsv'));
        const left = mapRange(h, 0, 360, 0, 100);
        this.markerElem_.style.left = `${left}%`;
    }
    onValueChange_() {
        this.update_();
    }
}

class HPaletteController {
    constructor(doc, config) {
        this.onKeyDown_ = this.onKeyDown_.bind(this);
        this.onKeyUp_ = this.onKeyUp_.bind(this);
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new HPaletteView(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.ptHandler_ = new PointerHandler(this.view.element);
        this.ptHandler_.emitter.on('down', this.onPointerDown_);
        this.ptHandler_.emitter.on('move', this.onPointerMove_);
        this.ptHandler_.emitter.on('up', this.onPointerUp_);
        this.view.element.addEventListener('keydown', this.onKeyDown_);
        this.view.element.addEventListener('keyup', this.onKeyUp_);
    }
    handlePointerEvent_(d, opts) {
        if (!d.point) {
            return;
        }
        const hue = mapRange(constrainRange(d.point.x, 0, d.bounds.width), 0, d.bounds.width, 0, 360);
        const c = this.value.rawValue;
        const [, s, v, a] = c.getComponents('hsv');
        this.value.setRawValue(new IntColor([hue, s, v, a], 'hsv'), opts);
    }
    onPointerDown_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerMove_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: true,
            last: true,
        });
    }
    onKeyDown_(ev) {
        const step = getStepForKey(getKeyScaleForColor(false), getHorizontalStepKeys(ev));
        if (step === 0) {
            return;
        }
        const c = this.value.rawValue;
        const [h, s, v, a] = c.getComponents('hsv');
        this.value.setRawValue(new IntColor([h + step, s, v, a], 'hsv'), {
            forceEmit: false,
            last: false,
        });
    }
    onKeyUp_(ev) {
        const step = getStepForKey(getKeyScaleForColor(false), getHorizontalStepKeys(ev));
        if (step === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
}

const cn$7 = ClassName('svp');
const CANVAS_RESOL = 64;
class SvPaletteView {
    constructor(doc, config) {
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.value = config.value;
        this.value.emitter.on('change', this.onValueChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$7());
        config.viewProps.bindClassModifiers(this.element);
        config.viewProps.bindTabIndex(this.element);
        const canvasElem = doc.createElement('canvas');
        canvasElem.height = CANVAS_RESOL;
        canvasElem.width = CANVAS_RESOL;
        canvasElem.classList.add(cn$7('c'));
        this.element.appendChild(canvasElem);
        this.canvasElement = canvasElem;
        const markerElem = doc.createElement('div');
        markerElem.classList.add(cn$7('m'));
        this.element.appendChild(markerElem);
        this.markerElem_ = markerElem;
        this.update_();
    }
    update_() {
        const ctx = getCanvasContext(this.canvasElement);
        if (!ctx) {
            return;
        }
        const c = this.value.rawValue;
        const hsvComps = c.getComponents('hsv');
        const width = this.canvasElement.width;
        const height = this.canvasElement.height;
        const imgData = ctx.getImageData(0, 0, width, height);
        const data = imgData.data;
        for (let iy = 0; iy < height; iy++) {
            for (let ix = 0; ix < width; ix++) {
                const s = mapRange(ix, 0, width, 0, 100);
                const v = mapRange(iy, 0, height, 100, 0);
                const rgbComps = hsvToRgbInt(hsvComps[0], s, v);
                const i = (iy * width + ix) * 4;
                data[i] = rgbComps[0];
                data[i + 1] = rgbComps[1];
                data[i + 2] = rgbComps[2];
                data[i + 3] = 255;
            }
        }
        ctx.putImageData(imgData, 0, 0);
        const left = mapRange(hsvComps[1], 0, 100, 0, 100);
        this.markerElem_.style.left = `${left}%`;
        const top = mapRange(hsvComps[2], 0, 100, 100, 0);
        this.markerElem_.style.top = `${top}%`;
    }
    onValueChange_() {
        this.update_();
    }
}

class SvPaletteController {
    constructor(doc, config) {
        this.onKeyDown_ = this.onKeyDown_.bind(this);
        this.onKeyUp_ = this.onKeyUp_.bind(this);
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new SvPaletteView(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.ptHandler_ = new PointerHandler(this.view.element);
        this.ptHandler_.emitter.on('down', this.onPointerDown_);
        this.ptHandler_.emitter.on('move', this.onPointerMove_);
        this.ptHandler_.emitter.on('up', this.onPointerUp_);
        this.view.element.addEventListener('keydown', this.onKeyDown_);
        this.view.element.addEventListener('keyup', this.onKeyUp_);
    }
    handlePointerEvent_(d, opts) {
        if (!d.point) {
            return;
        }
        const saturation = mapRange(d.point.x, 0, d.bounds.width, 0, 100);
        const value = mapRange(d.point.y, 0, d.bounds.height, 100, 0);
        const [h, , , a] = this.value.rawValue.getComponents('hsv');
        this.value.setRawValue(new IntColor([h, saturation, value, a], 'hsv'), opts);
    }
    onPointerDown_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerMove_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: true,
            last: true,
        });
    }
    onKeyDown_(ev) {
        if (isArrowKey(ev.key)) {
            ev.preventDefault();
        }
        const [h, s, v, a] = this.value.rawValue.getComponents('hsv');
        const keyScale = getKeyScaleForColor(false);
        const ds = getStepForKey(keyScale, getHorizontalStepKeys(ev));
        const dv = getStepForKey(keyScale, getVerticalStepKeys(ev));
        if (ds === 0 && dv === 0) {
            return;
        }
        this.value.setRawValue(new IntColor([h, s + ds, v + dv, a], 'hsv'), {
            forceEmit: false,
            last: false,
        });
    }
    onKeyUp_(ev) {
        const keyScale = getKeyScaleForColor(false);
        const ds = getStepForKey(keyScale, getHorizontalStepKeys(ev));
        const dv = getStepForKey(keyScale, getVerticalStepKeys(ev));
        if (ds === 0 && dv === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
}

class ColorPickerController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.hPaletteC_ = new HPaletteController(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.svPaletteC_ = new SvPaletteController(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.alphaIcs_ = config.supportsAlpha
            ? {
                palette: new APaletteController(doc, {
                    value: this.value,
                    viewProps: this.viewProps,
                }),
                text: new NumberTextController(doc, {
                    parser: parseNumber,
                    props: ValueMap.fromObject({
                        pointerScale: 0.01,
                        keyScale: 0.1,
                        formatter: createNumberFormatter(2),
                    }),
                    value: createValue(0, {
                        constraint: new DefiniteRangeConstraint({ min: 0, max: 1 }),
                    }),
                    viewProps: this.viewProps,
                }),
            }
            : null;
        if (this.alphaIcs_) {
            connectValues({
                primary: this.value,
                secondary: this.alphaIcs_.text.value,
                forward: (p) => p.getComponents()[3],
                backward: (p, s) => {
                    const comps = p.getComponents();
                    comps[3] = s;
                    return new IntColor(comps, p.mode);
                },
            });
        }
        this.textsC_ = new ColorTextsController(doc, {
            colorType: config.colorType,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view = new ColorPickerView(doc, {
            alphaViews: this.alphaIcs_
                ? {
                    palette: this.alphaIcs_.palette.view,
                    text: this.alphaIcs_.text.view,
                }
                : null,
            hPaletteView: this.hPaletteC_.view,
            supportsAlpha: config.supportsAlpha,
            svPaletteView: this.svPaletteC_.view,
            textsView: this.textsC_.view,
            viewProps: this.viewProps,
        });
    }
    get textsController() {
        return this.textsC_;
    }
}

const cn$6 = ClassName('colsw');
class ColorSwatchView {
    constructor(doc, config) {
        this.onValueChange_ = this.onValueChange_.bind(this);
        config.value.emitter.on('change', this.onValueChange_);
        this.value = config.value;
        this.element = doc.createElement('div');
        this.element.classList.add(cn$6());
        config.viewProps.bindClassModifiers(this.element);
        const swatchElem = doc.createElement('div');
        swatchElem.classList.add(cn$6('sw'));
        this.element.appendChild(swatchElem);
        this.swatchElem_ = swatchElem;
        const buttonElem = doc.createElement('button');
        buttonElem.classList.add(cn$6('b'));
        config.viewProps.bindDisabled(buttonElem);
        this.element.appendChild(buttonElem);
        this.buttonElement = buttonElem;
        this.update_();
    }
    update_() {
        const value = this.value.rawValue;
        this.swatchElem_.style.backgroundColor = colorToHexRgbaString(value);
    }
    onValueChange_() {
        this.update_();
    }
}

class ColorSwatchController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new ColorSwatchView(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
    }
}

class ColorController {
    constructor(doc, config) {
        this.onButtonBlur_ = this.onButtonBlur_.bind(this);
        this.onButtonClick_ = this.onButtonClick_.bind(this);
        this.onPopupChildBlur_ = this.onPopupChildBlur_.bind(this);
        this.onPopupChildKeydown_ = this.onPopupChildKeydown_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.foldable_ = Foldable.create(config.expanded);
        this.swatchC_ = new ColorSwatchController(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        const buttonElem = this.swatchC_.view.buttonElement;
        buttonElem.addEventListener('blur', this.onButtonBlur_);
        buttonElem.addEventListener('click', this.onButtonClick_);
        this.textC_ = new TextController(doc, {
            parser: config.parser,
            props: ValueMap.fromObject({
                formatter: config.formatter,
            }),
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view = new ColorView(doc, {
            foldable: this.foldable_,
            pickerLayout: config.pickerLayout,
        });
        this.view.swatchElement.appendChild(this.swatchC_.view.element);
        this.view.textElement.appendChild(this.textC_.view.element);
        this.popC_ =
            config.pickerLayout === 'popup'
                ? new PopupController(doc, {
                    viewProps: this.viewProps,
                })
                : null;
        const pickerC = new ColorPickerController(doc, {
            colorType: config.colorType,
            supportsAlpha: config.supportsAlpha,
            value: this.value,
            viewProps: this.viewProps,
        });
        pickerC.view.allFocusableElements.forEach((elem) => {
            elem.addEventListener('blur', this.onPopupChildBlur_);
            elem.addEventListener('keydown', this.onPopupChildKeydown_);
        });
        this.pickerC_ = pickerC;
        if (this.popC_) {
            this.view.element.appendChild(this.popC_.view.element);
            this.popC_.view.element.appendChild(pickerC.view.element);
            connectValues({
                primary: this.foldable_.value('expanded'),
                secondary: this.popC_.shows,
                forward: (p) => p,
                backward: (_, s) => s,
            });
        }
        else if (this.view.pickerElement) {
            this.view.pickerElement.appendChild(this.pickerC_.view.element);
            bindFoldable(this.foldable_, this.view.pickerElement);
        }
    }
    get textController() {
        return this.textC_;
    }
    onButtonBlur_(e) {
        if (!this.popC_) {
            return;
        }
        const elem = this.view.element;
        const nextTarget = forceCast(e.relatedTarget);
        if (!nextTarget || !elem.contains(nextTarget)) {
            this.popC_.shows.rawValue = false;
        }
    }
    onButtonClick_() {
        this.foldable_.set('expanded', !this.foldable_.get('expanded'));
        if (this.foldable_.get('expanded')) {
            this.pickerC_.view.allFocusableElements[0].focus();
        }
    }
    onPopupChildBlur_(ev) {
        if (!this.popC_) {
            return;
        }
        const elem = this.popC_.view.element;
        const nextTarget = findNextTarget(ev);
        if (nextTarget && elem.contains(nextTarget)) {
            return;
        }
        if (nextTarget &&
            nextTarget === this.swatchC_.view.buttonElement &&
            !supportsTouch(elem.ownerDocument)) {
            return;
        }
        this.popC_.shows.rawValue = false;
    }
    onPopupChildKeydown_(ev) {
        if (this.popC_) {
            if (ev.key === 'Escape') {
                this.popC_.shows.rawValue = false;
            }
        }
        else if (this.view.pickerElement) {
            if (ev.key === 'Escape') {
                this.swatchC_.view.buttonElement.focus();
            }
        }
    }
}

function colorToRgbNumber(value) {
    return removeAlphaComponent(value.getComponents('rgb')).reduce((result, comp) => {
        return (result << 8) | (Math.floor(comp) & 0xff);
    }, 0);
}
function colorToRgbaNumber(value) {
    return (value.getComponents('rgb').reduce((result, comp, index) => {
        const hex = Math.floor(index === 3 ? comp * 255 : comp) & 0xff;
        return (result << 8) | hex;
    }, 0) >>> 0);
}
function numberToRgbColor(num) {
    return new IntColor([(num >> 16) & 0xff, (num >> 8) & 0xff, num & 0xff], 'rgb');
}
function numberToRgbaColor(num) {
    return new IntColor([
        (num >> 24) & 0xff,
        (num >> 16) & 0xff,
        (num >> 8) & 0xff,
        mapRange(num & 0xff, 0, 255, 0, 1),
    ], 'rgb');
}
function colorFromRgbNumber(value) {
    if (typeof value !== 'number') {
        return IntColor.black();
    }
    return numberToRgbColor(value);
}
function colorFromRgbaNumber(value) {
    if (typeof value !== 'number') {
        return IntColor.black();
    }
    return numberToRgbaColor(value);
}

function isRgbColorComponent(obj, key) {
    if (typeof obj !== 'object' || isEmpty(obj)) {
        return false;
    }
    return key in obj && typeof obj[key] === 'number';
}
function isRgbColorObject(obj) {
    return (isRgbColorComponent(obj, 'r') &&
        isRgbColorComponent(obj, 'g') &&
        isRgbColorComponent(obj, 'b'));
}
function isRgbaColorObject(obj) {
    return isRgbColorObject(obj) && isRgbColorComponent(obj, 'a');
}
function isColorObject(obj) {
    return isRgbColorObject(obj);
}
function equalsColor(v1, v2) {
    if (v1.mode !== v2.mode) {
        return false;
    }
    if (v1.type !== v2.type) {
        return false;
    }
    const comps1 = v1.getComponents();
    const comps2 = v2.getComponents();
    for (let i = 0; i < comps1.length; i++) {
        if (comps1[i] !== comps2[i]) {
            return false;
        }
    }
    return true;
}
function createColorComponentsFromRgbObject(obj) {
    return 'a' in obj ? [obj.r, obj.g, obj.b, obj.a] : [obj.r, obj.g, obj.b];
}

function createColorStringWriter(format) {
    const stringify = findColorStringifier(format);
    return stringify
        ? (target, value) => {
            writePrimitive(target, stringify(value));
        }
        : null;
}
function createColorNumberWriter(supportsAlpha) {
    const colorToNumber = supportsAlpha ? colorToRgbaNumber : colorToRgbNumber;
    return (target, value) => {
        writePrimitive(target, colorToNumber(value));
    };
}
function writeRgbaColorObject(target, value, type) {
    const cc = mapColorType(value, type);
    const obj = cc.toRgbaObject();
    target.writeProperty('r', obj.r);
    target.writeProperty('g', obj.g);
    target.writeProperty('b', obj.b);
    target.writeProperty('a', obj.a);
}
function writeRgbColorObject(target, value, type) {
    const cc = mapColorType(value, type);
    const obj = cc.toRgbaObject();
    target.writeProperty('r', obj.r);
    target.writeProperty('g', obj.g);
    target.writeProperty('b', obj.b);
}
function createColorObjectWriter(supportsAlpha, type) {
    return (target, inValue) => {
        if (supportsAlpha) {
            writeRgbaColorObject(target, inValue, type);
        }
        else {
            writeRgbColorObject(target, inValue, type);
        }
    };
}

function shouldSupportAlpha$1(inputParams) {
    var _a;
    if ((_a = inputParams === null || inputParams === void 0 ? void 0 : inputParams.color) === null || _a === void 0 ? void 0 : _a.alpha) {
        return true;
    }
    return false;
}
function createFormatter$1(supportsAlpha) {
    return supportsAlpha
        ? (v) => colorToHexRgbaString(v, '0x')
        : (v) => colorToHexRgbString(v, '0x');
}
function isForColor(params) {
    if ('color' in params) {
        return true;
    }
    if (params.view === 'color') {
        return true;
    }
    return false;
}
createPlugin({
    id: 'input-color-number',
    type: 'input',
    accept: (value, params) => {
        if (typeof value !== 'number') {
            return null;
        }
        if (!isForColor(params)) {
            return null;
        }
        const result = parseColorInputParams(params);
        return result
            ? {
                initialValue: value,
                params: Object.assign(Object.assign({}, result), { supportsAlpha: shouldSupportAlpha$1(params) }),
            }
            : null;
    },
    binding: {
        reader: (args) => {
            return args.params.supportsAlpha
                ? colorFromRgbaNumber
                : colorFromRgbNumber;
        },
        equals: equalsColor,
        writer: (args) => {
            return createColorNumberWriter(args.params.supportsAlpha);
        },
    },
    controller: (args) => {
        var _a, _b;
        return new ColorController(args.document, {
            colorType: 'int',
            expanded: (_a = args.params.expanded) !== null && _a !== void 0 ? _a : false,
            formatter: createFormatter$1(args.params.supportsAlpha),
            parser: createColorStringParser('int'),
            pickerLayout: (_b = args.params.picker) !== null && _b !== void 0 ? _b : 'popup',
            supportsAlpha: args.params.supportsAlpha,
            value: args.value,
            viewProps: args.viewProps,
        });
    },
});

function colorFromObject(value, type) {
    if (!isColorObject(value)) {
        return mapColorType(IntColor.black(), type);
    }
    if (type === 'int') {
        const comps = createColorComponentsFromRgbObject(value);
        return new IntColor(comps, 'rgb');
    }
    if (type === 'float') {
        const comps = createColorComponentsFromRgbObject(value);
        return new FloatColor(comps, 'rgb');
    }
    return mapColorType(IntColor.black(), 'int');
}

function shouldSupportAlpha(initialValue) {
    return isRgbaColorObject(initialValue);
}
function createColorObjectBindingReader(type) {
    return (value) => {
        const c = colorFromObject(value, type);
        return mapColorType(c, 'int');
    };
}
function createColorObjectFormatter(supportsAlpha, type) {
    return (value) => {
        if (supportsAlpha) {
            return colorToObjectRgbaString(value, type);
        }
        return colorToObjectRgbString(value, type);
    };
}
createPlugin({
    id: 'input-color-object',
    type: 'input',
    accept: (value, params) => {
        var _a;
        if (!isColorObject(value)) {
            return null;
        }
        const result = parseColorInputParams(params);
        return result
            ? {
                initialValue: value,
                params: Object.assign(Object.assign({}, result), { colorType: (_a = extractColorType(params)) !== null && _a !== void 0 ? _a : 'int' }),
            }
            : null;
    },
    binding: {
        reader: (args) => createColorObjectBindingReader(args.params.colorType),
        equals: equalsColor,
        writer: (args) => createColorObjectWriter(shouldSupportAlpha(args.initialValue), args.params.colorType),
    },
    controller: (args) => {
        var _a, _b;
        const supportsAlpha = isRgbaColorObject(args.initialValue);
        return new ColorController(args.document, {
            colorType: args.params.colorType,
            expanded: (_a = args.params.expanded) !== null && _a !== void 0 ? _a : false,
            formatter: createColorObjectFormatter(supportsAlpha, args.params.colorType),
            parser: createColorStringParser('int'),
            pickerLayout: (_b = args.params.picker) !== null && _b !== void 0 ? _b : 'popup',
            supportsAlpha: supportsAlpha,
            value: args.value,
            viewProps: args.viewProps,
        });
    },
});

createPlugin({
    id: 'input-color-string',
    type: 'input',
    accept: (value, params) => {
        if (typeof value !== 'string') {
            return null;
        }
        if (params.view === 'text') {
            return null;
        }
        const format = detectStringColorFormat(value, extractColorType(params));
        if (!format) {
            return null;
        }
        const stringifier = findColorStringifier(format);
        if (!stringifier) {
            return null;
        }
        const result = parseColorInputParams(params);
        return result
            ? {
                initialValue: value,
                params: Object.assign(Object.assign({}, result), { format: format, stringifier: stringifier }),
            }
            : null;
    },
    binding: {
        reader: () => readIntColorString,
        equals: equalsColor,
        writer: (args) => {
            const writer = createColorStringWriter(args.params.format);
            if (!writer) {
                throw TpError.notBindable();
            }
            return writer;
        },
    },
    controller: (args) => {
        var _a, _b;
        return new ColorController(args.document, {
            colorType: args.params.format.type,
            expanded: (_a = args.params.expanded) !== null && _a !== void 0 ? _a : false,
            formatter: args.params.stringifier,
            parser: createColorStringParser('int'),
            pickerLayout: (_b = args.params.picker) !== null && _b !== void 0 ? _b : 'popup',
            supportsAlpha: args.params.format.alpha,
            value: args.value,
            viewProps: args.viewProps,
        });
    },
});

class PointNdConstraint {
    constructor(config) {
        this.components = config.components;
        this.asm_ = config.assembly;
    }
    constrain(value) {
        const comps = this.asm_
            .toComponents(value)
            .map((comp, index) => { var _a, _b; return (_b = (_a = this.components[index]) === null || _a === void 0 ? void 0 : _a.constrain(comp)) !== null && _b !== void 0 ? _b : comp; });
        return this.asm_.fromComponents(comps);
    }
}

const cn$5 = ClassName('pndtxt');
class PointNdTextView {
    constructor(doc, config) {
        this.textViews = config.textViews;
        this.element = doc.createElement('div');
        this.element.classList.add(cn$5());
        this.textViews.forEach((v) => {
            const axisElem = doc.createElement('div');
            axisElem.classList.add(cn$5('a'));
            axisElem.appendChild(v.element);
            this.element.appendChild(axisElem);
        });
    }
}

function createAxisController(doc, config, index) {
    return new NumberTextController(doc, {
        arrayPosition: index === 0 ? 'fst' : index === config.axes.length - 1 ? 'lst' : 'mid',
        parser: config.parser,
        props: config.axes[index].textProps,
        value: createValue(0, {
            constraint: config.axes[index].constraint,
        }),
        viewProps: config.viewProps,
    });
}
class PointNdTextController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.acs_ = config.axes.map((_, index) => createAxisController(doc, config, index));
        this.acs_.forEach((c, index) => {
            connectValues({
                primary: this.value,
                secondary: c.value,
                forward: (p) => config.assembly.toComponents(p)[index],
                backward: (p, s) => {
                    const comps = config.assembly.toComponents(p);
                    comps[index] = s;
                    return config.assembly.fromComponents(comps);
                },
            });
        });
        this.view = new PointNdTextView(doc, {
            textViews: this.acs_.map((ac) => ac.view),
        });
    }
    get textControllers() {
        return this.acs_;
    }
}

class SliderInputBindingApi extends BindingApi {
    get max() {
        return this.controller.valueController.sliderController.props.get('max');
    }
    set max(max) {
        this.controller.valueController.sliderController.props.set('max', max);
    }
    get min() {
        return this.controller.valueController.sliderController.props.get('min');
    }
    set min(max) {
        this.controller.valueController.sliderController.props.set('min', max);
    }
}

function createConstraint$6(params, initialValue) {
    const constraints = [];
    const sc = createStepConstraint(params, initialValue);
    if (sc) {
        constraints.push(sc);
    }
    const rc = createRangeConstraint(params);
    if (rc) {
        constraints.push(rc);
    }
    const lc = createListConstraint(params.options);
    if (lc) {
        constraints.push(lc);
    }
    return new CompositeConstraint(constraints);
}
createPlugin({
    id: 'input-number',
    type: 'input',
    accept: (value, params) => {
        if (typeof value !== 'number') {
            return null;
        }
        const result = parseRecord(params, (p) => (Object.assign(Object.assign({}, createNumberTextInputParamsParser(p)), { options: p.optional.custom(parseListOptions), readonly: p.optional.constant(false) })));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => numberFromUnknown,
        constraint: (args) => createConstraint$6(args.params, args.initialValue),
        writer: (_args) => writePrimitive,
    },
    controller: (args) => {
        const value = args.value;
        const c = args.constraint;
        const lc = c && findConstraint(c, ListConstraint);
        if (lc) {
            return new ListController(args.document, {
                props: new ValueMap({
                    options: lc.values.value('options'),
                }),
                value: value,
                viewProps: args.viewProps,
            });
        }
        const textPropsObj = createNumberTextPropsObject(args.params, value.rawValue);
        const drc = c && findConstraint(c, DefiniteRangeConstraint);
        if (drc) {
            return new SliderTextController(args.document, Object.assign(Object.assign({}, createSliderTextProps(Object.assign(Object.assign({}, textPropsObj), { keyScale: createValue(textPropsObj.keyScale), max: drc.values.value('max'), min: drc.values.value('min') }))), { parser: parseNumber, value: value, viewProps: args.viewProps }));
        }
        return new NumberTextController(args.document, {
            parser: parseNumber,
            props: ValueMap.fromObject(textPropsObj),
            value: value,
            viewProps: args.viewProps,
        });
    },
    api(args) {
        if (typeof args.controller.value.rawValue !== 'number') {
            return null;
        }
        if (args.controller.valueController instanceof SliderTextController) {
            return new SliderInputBindingApi(args.controller);
        }
        if (args.controller.valueController instanceof ListController) {
            return new ListInputBindingApi(args.controller);
        }
        return null;
    },
});

class Point2d {
    constructor(x = 0, y = 0) {
        this.x = x;
        this.y = y;
    }
    getComponents() {
        return [this.x, this.y];
    }
    static isObject(obj) {
        if (isEmpty(obj)) {
            return false;
        }
        const x = obj.x;
        const y = obj.y;
        if (typeof x !== 'number' || typeof y !== 'number') {
            return false;
        }
        return true;
    }
    static equals(v1, v2) {
        return v1.x === v2.x && v1.y === v2.y;
    }
    toObject() {
        return {
            x: this.x,
            y: this.y,
        };
    }
}
const Point2dAssembly = {
    toComponents: (p) => p.getComponents(),
    fromComponents: (comps) => new Point2d(...comps),
};

const cn$4 = ClassName('p2d');
class Point2dView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(cn$4());
        config.viewProps.bindClassModifiers(this.element);
        bindValue(config.expanded, valueToClassName(this.element, cn$4(undefined, 'expanded')));
        const headElem = doc.createElement('div');
        headElem.classList.add(cn$4('h'));
        this.element.appendChild(headElem);
        const buttonElem = doc.createElement('button');
        buttonElem.classList.add(cn$4('b'));
        buttonElem.appendChild(createSvgIconElement(doc, 'p2dpad'));
        config.viewProps.bindDisabled(buttonElem);
        headElem.appendChild(buttonElem);
        this.buttonElement = buttonElem;
        const textElem = doc.createElement('div');
        textElem.classList.add(cn$4('t'));
        headElem.appendChild(textElem);
        this.textElement = textElem;
        if (config.pickerLayout === 'inline') {
            const pickerElem = doc.createElement('div');
            pickerElem.classList.add(cn$4('p'));
            this.element.appendChild(pickerElem);
            this.pickerElement = pickerElem;
        }
        else {
            this.pickerElement = null;
        }
    }
}

const cn$3 = ClassName('p2dp');
class Point2dPickerView {
    constructor(doc, config) {
        this.onFoldableChange_ = this.onFoldableChange_.bind(this);
        this.onPropsChange_ = this.onPropsChange_.bind(this);
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.props_ = config.props;
        this.props_.emitter.on('change', this.onPropsChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(cn$3());
        if (config.layout === 'popup') {
            this.element.classList.add(cn$3(undefined, 'p'));
        }
        config.viewProps.bindClassModifiers(this.element);
        const padElem = doc.createElement('div');
        padElem.classList.add(cn$3('p'));
        config.viewProps.bindTabIndex(padElem);
        this.element.appendChild(padElem);
        this.padElement = padElem;
        const svgElem = doc.createElementNS(SVG_NS, 'svg');
        svgElem.classList.add(cn$3('g'));
        this.padElement.appendChild(svgElem);
        this.svgElem_ = svgElem;
        const xAxisElem = doc.createElementNS(SVG_NS, 'line');
        xAxisElem.classList.add(cn$3('ax'));
        xAxisElem.setAttributeNS(null, 'x1', '0');
        xAxisElem.setAttributeNS(null, 'y1', '50%');
        xAxisElem.setAttributeNS(null, 'x2', '100%');
        xAxisElem.setAttributeNS(null, 'y2', '50%');
        this.svgElem_.appendChild(xAxisElem);
        const yAxisElem = doc.createElementNS(SVG_NS, 'line');
        yAxisElem.classList.add(cn$3('ax'));
        yAxisElem.setAttributeNS(null, 'x1', '50%');
        yAxisElem.setAttributeNS(null, 'y1', '0');
        yAxisElem.setAttributeNS(null, 'x2', '50%');
        yAxisElem.setAttributeNS(null, 'y2', '100%');
        this.svgElem_.appendChild(yAxisElem);
        const lineElem = doc.createElementNS(SVG_NS, 'line');
        lineElem.classList.add(cn$3('l'));
        lineElem.setAttributeNS(null, 'x1', '50%');
        lineElem.setAttributeNS(null, 'y1', '50%');
        this.svgElem_.appendChild(lineElem);
        this.lineElem_ = lineElem;
        const markerElem = doc.createElement('div');
        markerElem.classList.add(cn$3('m'));
        this.padElement.appendChild(markerElem);
        this.markerElem_ = markerElem;
        config.value.emitter.on('change', this.onValueChange_);
        this.value = config.value;
        this.update_();
    }
    get allFocusableElements() {
        return [this.padElement];
    }
    update_() {
        const [x, y] = this.value.rawValue.getComponents();
        const max = this.props_.get('max');
        const px = mapRange(x, -max, +max, 0, 100);
        const py = mapRange(y, -max, +max, 0, 100);
        const ipy = this.props_.get('invertsY') ? 100 - py : py;
        this.lineElem_.setAttributeNS(null, 'x2', `${px}%`);
        this.lineElem_.setAttributeNS(null, 'y2', `${ipy}%`);
        this.markerElem_.style.left = `${px}%`;
        this.markerElem_.style.top = `${ipy}%`;
    }
    onValueChange_() {
        this.update_();
    }
    onPropsChange_() {
        this.update_();
    }
    onFoldableChange_() {
        this.update_();
    }
}

function computeOffset(ev, keyScales, invertsY) {
    return [
        getStepForKey(keyScales[0], getHorizontalStepKeys(ev)),
        getStepForKey(keyScales[1], getVerticalStepKeys(ev)) * (invertsY ? 1 : -1),
    ];
}
class Point2dPickerController {
    constructor(doc, config) {
        this.onPadKeyDown_ = this.onPadKeyDown_.bind(this);
        this.onPadKeyUp_ = this.onPadKeyUp_.bind(this);
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.props = config.props;
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new Point2dPickerView(doc, {
            layout: config.layout,
            props: this.props,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.ptHandler_ = new PointerHandler(this.view.padElement);
        this.ptHandler_.emitter.on('down', this.onPointerDown_);
        this.ptHandler_.emitter.on('move', this.onPointerMove_);
        this.ptHandler_.emitter.on('up', this.onPointerUp_);
        this.view.padElement.addEventListener('keydown', this.onPadKeyDown_);
        this.view.padElement.addEventListener('keyup', this.onPadKeyUp_);
    }
    handlePointerEvent_(d, opts) {
        if (!d.point) {
            return;
        }
        const max = this.props.get('max');
        const px = mapRange(d.point.x, 0, d.bounds.width, -max, +max);
        const py = mapRange(this.props.get('invertsY') ? d.bounds.height - d.point.y : d.point.y, 0, d.bounds.height, -max, +max);
        this.value.setRawValue(new Point2d(px, py), opts);
    }
    onPointerDown_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerMove_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        this.handlePointerEvent_(ev.data, {
            forceEmit: true,
            last: true,
        });
    }
    onPadKeyDown_(ev) {
        if (isArrowKey(ev.key)) {
            ev.preventDefault();
        }
        const [dx, dy] = computeOffset(ev, [this.props.get('xKeyScale'), this.props.get('yKeyScale')], this.props.get('invertsY'));
        if (dx === 0 && dy === 0) {
            return;
        }
        this.value.setRawValue(new Point2d(this.value.rawValue.x + dx, this.value.rawValue.y + dy), {
            forceEmit: false,
            last: false,
        });
    }
    onPadKeyUp_(ev) {
        const [dx, dy] = computeOffset(ev, [this.props.get('xKeyScale'), this.props.get('yKeyScale')], this.props.get('invertsY'));
        if (dx === 0 && dy === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
}

class Point2dController {
    constructor(doc, config) {
        var _a, _b;
        this.onPopupChildBlur_ = this.onPopupChildBlur_.bind(this);
        this.onPopupChildKeydown_ = this.onPopupChildKeydown_.bind(this);
        this.onPadButtonBlur_ = this.onPadButtonBlur_.bind(this);
        this.onPadButtonClick_ = this.onPadButtonClick_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.foldable_ = Foldable.create(config.expanded);
        this.popC_ =
            config.pickerLayout === 'popup'
                ? new PopupController(doc, {
                    viewProps: this.viewProps,
                })
                : null;
        const padC = new Point2dPickerController(doc, {
            layout: config.pickerLayout,
            props: new ValueMap({
                invertsY: createValue(config.invertsY),
                max: createValue(config.max),
                xKeyScale: config.axes[0].textProps.value('keyScale'),
                yKeyScale: config.axes[1].textProps.value('keyScale'),
            }),
            value: this.value,
            viewProps: this.viewProps,
        });
        padC.view.allFocusableElements.forEach((elem) => {
            elem.addEventListener('blur', this.onPopupChildBlur_);
            elem.addEventListener('keydown', this.onPopupChildKeydown_);
        });
        this.pickerC_ = padC;
        this.textC_ = new PointNdTextController(doc, {
            assembly: Point2dAssembly,
            axes: config.axes,
            parser: config.parser,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view = new Point2dView(doc, {
            expanded: this.foldable_.value('expanded'),
            pickerLayout: config.pickerLayout,
            viewProps: this.viewProps,
        });
        this.view.textElement.appendChild(this.textC_.view.element);
        (_a = this.view.buttonElement) === null || _a === void 0 ? void 0 : _a.addEventListener('blur', this.onPadButtonBlur_);
        (_b = this.view.buttonElement) === null || _b === void 0 ? void 0 : _b.addEventListener('click', this.onPadButtonClick_);
        if (this.popC_) {
            this.view.element.appendChild(this.popC_.view.element);
            this.popC_.view.element.appendChild(this.pickerC_.view.element);
            connectValues({
                primary: this.foldable_.value('expanded'),
                secondary: this.popC_.shows,
                forward: (p) => p,
                backward: (_, s) => s,
            });
        }
        else if (this.view.pickerElement) {
            this.view.pickerElement.appendChild(this.pickerC_.view.element);
            bindFoldable(this.foldable_, this.view.pickerElement);
        }
    }
    get textController() {
        return this.textC_;
    }
    onPadButtonBlur_(e) {
        if (!this.popC_) {
            return;
        }
        const elem = this.view.element;
        const nextTarget = forceCast(e.relatedTarget);
        if (!nextTarget || !elem.contains(nextTarget)) {
            this.popC_.shows.rawValue = false;
        }
    }
    onPadButtonClick_() {
        this.foldable_.set('expanded', !this.foldable_.get('expanded'));
        if (this.foldable_.get('expanded')) {
            this.pickerC_.view.allFocusableElements[0].focus();
        }
    }
    onPopupChildBlur_(ev) {
        if (!this.popC_) {
            return;
        }
        const elem = this.popC_.view.element;
        const nextTarget = findNextTarget(ev);
        if (nextTarget && elem.contains(nextTarget)) {
            return;
        }
        if (nextTarget &&
            nextTarget === this.view.buttonElement &&
            !supportsTouch(elem.ownerDocument)) {
            return;
        }
        this.popC_.shows.rawValue = false;
    }
    onPopupChildKeydown_(ev) {
        if (this.popC_) {
            if (ev.key === 'Escape') {
                this.popC_.shows.rawValue = false;
            }
        }
        else if (this.view.pickerElement) {
            if (ev.key === 'Escape') {
                this.view.buttonElement.focus();
            }
        }
    }
}

function point2dFromUnknown(value) {
    return Point2d.isObject(value)
        ? new Point2d(value.x, value.y)
        : new Point2d();
}
function writePoint2d(target, value) {
    target.writeProperty('x', value.x);
    target.writeProperty('y', value.y);
}

function createConstraint$5(params, initialValue) {
    return new PointNdConstraint({
        assembly: Point2dAssembly,
        components: [
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.x), initialValue.x),
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.y), initialValue.y),
        ],
    });
}
function getSuitableMaxDimensionValue(params, rawValue) {
    var _a, _b;
    if (!isEmpty(params.min) || !isEmpty(params.max)) {
        return Math.max(Math.abs((_a = params.min) !== null && _a !== void 0 ? _a : 0), Math.abs((_b = params.max) !== null && _b !== void 0 ? _b : 0));
    }
    const step = getSuitableKeyScale(params);
    return Math.max(Math.abs(step) * 10, Math.abs(rawValue) * 10);
}
function getSuitableMax(params, initialValue) {
    var _a, _b;
    const xr = getSuitableMaxDimensionValue(deepMerge(params, ((_a = params.x) !== null && _a !== void 0 ? _a : {})), initialValue.x);
    const yr = getSuitableMaxDimensionValue(deepMerge(params, ((_b = params.y) !== null && _b !== void 0 ? _b : {})), initialValue.y);
    return Math.max(xr, yr);
}
function shouldInvertY(params) {
    if (!('y' in params)) {
        return false;
    }
    const yParams = params.y;
    if (!yParams) {
        return false;
    }
    return 'inverted' in yParams ? !!yParams.inverted : false;
}
createPlugin({
    id: 'input-point2d',
    type: 'input',
    accept: (value, params) => {
        if (!Point2d.isObject(value)) {
            return null;
        }
        const result = parseRecord(params, (p) => (Object.assign(Object.assign({}, createPointDimensionParser(p)), { expanded: p.optional.boolean, picker: p.optional.custom(parsePickerLayout), readonly: p.optional.constant(false), x: p.optional.custom(parsePointDimensionParams), y: p.optional.object(Object.assign(Object.assign({}, createPointDimensionParser(p)), { inverted: p.optional.boolean })) })));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: () => point2dFromUnknown,
        constraint: (args) => createConstraint$5(args.params, args.initialValue),
        equals: Point2d.equals,
        writer: () => writePoint2d,
    },
    controller: (args) => {
        var _a, _b;
        const doc = args.document;
        const value = args.value;
        const c = args.constraint;
        const dParams = [args.params.x, args.params.y];
        return new Point2dController(doc, {
            axes: value.rawValue.getComponents().map((comp, i) => {
                var _a;
                return createPointAxis({
                    constraint: c.components[i],
                    initialValue: comp,
                    params: deepMerge(args.params, ((_a = dParams[i]) !== null && _a !== void 0 ? _a : {})),
                });
            }),
            expanded: (_a = args.params.expanded) !== null && _a !== void 0 ? _a : false,
            invertsY: shouldInvertY(args.params),
            max: getSuitableMax(args.params, value.rawValue),
            parser: parseNumber,
            pickerLayout: (_b = args.params.picker) !== null && _b !== void 0 ? _b : 'popup',
            value: value,
            viewProps: args.viewProps,
        });
    },
});

class Point3d {
    constructor(x = 0, y = 0, z = 0) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    getComponents() {
        return [this.x, this.y, this.z];
    }
    static isObject(obj) {
        if (isEmpty(obj)) {
            return false;
        }
        const x = obj.x;
        const y = obj.y;
        const z = obj.z;
        if (typeof x !== 'number' ||
            typeof y !== 'number' ||
            typeof z !== 'number') {
            return false;
        }
        return true;
    }
    static equals(v1, v2) {
        return v1.x === v2.x && v1.y === v2.y && v1.z === v2.z;
    }
    toObject() {
        return {
            x: this.x,
            y: this.y,
            z: this.z,
        };
    }
}
const Point3dAssembly = {
    toComponents: (p) => p.getComponents(),
    fromComponents: (comps) => new Point3d(...comps),
};

function point3dFromUnknown(value) {
    return Point3d.isObject(value)
        ? new Point3d(value.x, value.y, value.z)
        : new Point3d();
}
function writePoint3d(target, value) {
    target.writeProperty('x', value.x);
    target.writeProperty('y', value.y);
    target.writeProperty('z', value.z);
}

function createConstraint$4(params, initialValue) {
    return new PointNdConstraint({
        assembly: Point3dAssembly,
        components: [
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.x), initialValue.x),
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.y), initialValue.y),
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.z), initialValue.z),
        ],
    });
}
createPlugin({
    id: 'input-point3d',
    type: 'input',
    accept: (value, params) => {
        if (!Point3d.isObject(value)) {
            return null;
        }
        const result = parseRecord(params, (p) => (Object.assign(Object.assign({}, createPointDimensionParser(p)), { readonly: p.optional.constant(false), x: p.optional.custom(parsePointDimensionParams), y: p.optional.custom(parsePointDimensionParams), z: p.optional.custom(parsePointDimensionParams) })));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => point3dFromUnknown,
        constraint: (args) => createConstraint$4(args.params, args.initialValue),
        equals: Point3d.equals,
        writer: (_args) => writePoint3d,
    },
    controller: (args) => {
        const value = args.value;
        const c = args.constraint;
        const dParams = [args.params.x, args.params.y, args.params.z];
        return new PointNdTextController(args.document, {
            assembly: Point3dAssembly,
            axes: value.rawValue.getComponents().map((comp, i) => {
                var _a;
                return createPointAxis({
                    constraint: c.components[i],
                    initialValue: comp,
                    params: deepMerge(args.params, ((_a = dParams[i]) !== null && _a !== void 0 ? _a : {})),
                });
            }),
            parser: parseNumber,
            value: value,
            viewProps: args.viewProps,
        });
    },
});

class Point4d {
    constructor(x = 0, y = 0, z = 0, w = 0) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
    getComponents() {
        return [this.x, this.y, this.z, this.w];
    }
    static isObject(obj) {
        if (isEmpty(obj)) {
            return false;
        }
        const x = obj.x;
        const y = obj.y;
        const z = obj.z;
        const w = obj.w;
        if (typeof x !== 'number' ||
            typeof y !== 'number' ||
            typeof z !== 'number' ||
            typeof w !== 'number') {
            return false;
        }
        return true;
    }
    static equals(v1, v2) {
        return v1.x === v2.x && v1.y === v2.y && v1.z === v2.z && v1.w === v2.w;
    }
    toObject() {
        return {
            x: this.x,
            y: this.y,
            z: this.z,
            w: this.w,
        };
    }
}
const Point4dAssembly = {
    toComponents: (p) => p.getComponents(),
    fromComponents: (comps) => new Point4d(...comps),
};

function point4dFromUnknown(value) {
    return Point4d.isObject(value)
        ? new Point4d(value.x, value.y, value.z, value.w)
        : new Point4d();
}
function writePoint4d(target, value) {
    target.writeProperty('x', value.x);
    target.writeProperty('y', value.y);
    target.writeProperty('z', value.z);
    target.writeProperty('w', value.w);
}

function createConstraint$3(params, initialValue) {
    return new PointNdConstraint({
        assembly: Point4dAssembly,
        components: [
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.x), initialValue.x),
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.y), initialValue.y),
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.z), initialValue.z),
            createDimensionConstraint(Object.assign(Object.assign({}, params), params.w), initialValue.w),
        ],
    });
}
createPlugin({
    id: 'input-point4d',
    type: 'input',
    accept: (value, params) => {
        if (!Point4d.isObject(value)) {
            return null;
        }
        const result = parseRecord(params, (p) => (Object.assign(Object.assign({}, createPointDimensionParser(p)), { readonly: p.optional.constant(false), w: p.optional.custom(parsePointDimensionParams), x: p.optional.custom(parsePointDimensionParams), y: p.optional.custom(parsePointDimensionParams), z: p.optional.custom(parsePointDimensionParams) })));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => point4dFromUnknown,
        constraint: (args) => createConstraint$3(args.params, args.initialValue),
        equals: Point4d.equals,
        writer: (_args) => writePoint4d,
    },
    controller: (args) => {
        const value = args.value;
        const c = args.constraint;
        const dParams = [
            args.params.x,
            args.params.y,
            args.params.z,
            args.params.w,
        ];
        return new PointNdTextController(args.document, {
            assembly: Point4dAssembly,
            axes: value.rawValue.getComponents().map((comp, i) => {
                var _a;
                return createPointAxis({
                    constraint: c.components[i],
                    initialValue: comp,
                    params: deepMerge(args.params, ((_a = dParams[i]) !== null && _a !== void 0 ? _a : {})),
                });
            }),
            parser: parseNumber,
            value: value,
            viewProps: args.viewProps,
        });
    },
});

function createConstraint$2(params) {
    const constraints = [];
    const lc = createListConstraint(params.options);
    if (lc) {
        constraints.push(lc);
    }
    return new CompositeConstraint(constraints);
}
createPlugin({
    id: 'input-string',
    type: 'input',
    accept: (value, params) => {
        if (typeof value !== 'string') {
            return null;
        }
        const result = parseRecord(params, (p) => ({
            readonly: p.optional.constant(false),
            options: p.optional.custom(parseListOptions),
        }));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => stringFromUnknown,
        constraint: (args) => createConstraint$2(args.params),
        writer: (_args) => writePrimitive,
    },
    controller: (args) => {
        const doc = args.document;
        const value = args.value;
        const c = args.constraint;
        const lc = c && findConstraint(c, ListConstraint);
        if (lc) {
            return new ListController(doc, {
                props: new ValueMap({
                    options: lc.values.value('options'),
                }),
                value: value,
                viewProps: args.viewProps,
            });
        }
        return new TextController(doc, {
            parser: (v) => v,
            props: ValueMap.fromObject({
                formatter: formatString,
            }),
            value: value,
            viewProps: args.viewProps,
        });
    },
    api(args) {
        if (typeof args.controller.value.rawValue !== 'string') {
            return null;
        }
        if (args.controller.valueController instanceof ListController) {
            return new ListInputBindingApi(args.controller);
        }
        return null;
    },
});

const Constants = {
    monitor: {
        defaultInterval: 200,
        defaultRows: 3,
    },
};

const cn$2 = ClassName('mll');
class MultiLogView {
    constructor(doc, config) {
        this.onValueUpdate_ = this.onValueUpdate_.bind(this);
        this.formatter_ = config.formatter;
        this.element = doc.createElement('div');
        this.element.classList.add(cn$2());
        config.viewProps.bindClassModifiers(this.element);
        const textareaElem = doc.createElement('textarea');
        textareaElem.classList.add(cn$2('i'));
        textareaElem.style.height = `calc(var(${getCssVar('containerUnitSize')}) * ${config.rows})`;
        textareaElem.readOnly = true;
        config.viewProps.bindDisabled(textareaElem);
        this.element.appendChild(textareaElem);
        this.textareaElem_ = textareaElem;
        config.value.emitter.on('change', this.onValueUpdate_);
        this.value = config.value;
        this.update_();
    }
    update_() {
        const elem = this.textareaElem_;
        const shouldScroll = elem.scrollTop === elem.scrollHeight - elem.clientHeight;
        const lines = [];
        this.value.rawValue.forEach((value) => {
            if (value !== undefined) {
                lines.push(this.formatter_(value));
            }
        });
        elem.textContent = lines.join('\n');
        if (shouldScroll) {
            elem.scrollTop = elem.scrollHeight;
        }
    }
    onValueUpdate_() {
        this.update_();
    }
}

class MultiLogController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new MultiLogView(doc, {
            formatter: config.formatter,
            rows: config.rows,
            value: this.value,
            viewProps: this.viewProps,
        });
    }
}

const cn$1 = ClassName('sgl');
class SingleLogView {
    constructor(doc, config) {
        this.onValueUpdate_ = this.onValueUpdate_.bind(this);
        this.formatter_ = config.formatter;
        this.element = doc.createElement('div');
        this.element.classList.add(cn$1());
        config.viewProps.bindClassModifiers(this.element);
        const inputElem = doc.createElement('input');
        inputElem.classList.add(cn$1('i'));
        inputElem.readOnly = true;
        inputElem.type = 'text';
        config.viewProps.bindDisabled(inputElem);
        this.element.appendChild(inputElem);
        this.inputElement = inputElem;
        config.value.emitter.on('change', this.onValueUpdate_);
        this.value = config.value;
        this.update_();
    }
    update_() {
        const values = this.value.rawValue;
        const lastValue = values[values.length - 1];
        this.inputElement.value =
            lastValue !== undefined ? this.formatter_(lastValue) : '';
    }
    onValueUpdate_() {
        this.update_();
    }
}

class SingleLogController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new SingleLogView(doc, {
            formatter: config.formatter,
            value: this.value,
            viewProps: this.viewProps,
        });
    }
}

createPlugin({
    id: 'monitor-bool',
    type: 'monitor',
    accept: (value, params) => {
        if (typeof value !== 'boolean') {
            return null;
        }
        const result = parseRecord(params, (p) => ({
            readonly: p.required.constant(true),
            rows: p.optional.number,
        }));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => boolFromUnknown,
    },
    controller: (args) => {
        var _a;
        if (args.value.rawValue.length === 1) {
            return new SingleLogController(args.document, {
                formatter: BooleanFormatter,
                value: args.value,
                viewProps: args.viewProps,
            });
        }
        return new MultiLogController(args.document, {
            formatter: BooleanFormatter,
            rows: (_a = args.params.rows) !== null && _a !== void 0 ? _a : Constants.monitor.defaultRows,
            value: args.value,
            viewProps: args.viewProps,
        });
    },
});

class GraphLogMonitorBindingApi extends BindingApi {
    get max() {
        return this.controller.valueController.props.get('max');
    }
    set max(max) {
        this.controller.valueController.props.set('max', max);
    }
    get min() {
        return this.controller.valueController.props.get('min');
    }
    set min(min) {
        this.controller.valueController.props.set('min', min);
    }
}

const cn = ClassName('grl');
class GraphLogView {
    constructor(doc, config) {
        this.onCursorChange_ = this.onCursorChange_.bind(this);
        this.onValueUpdate_ = this.onValueUpdate_.bind(this);
        this.element = doc.createElement('div');
        this.element.classList.add(cn());
        config.viewProps.bindClassModifiers(this.element);
        this.formatter_ = config.formatter;
        this.props_ = config.props;
        this.cursor_ = config.cursor;
        this.cursor_.emitter.on('change', this.onCursorChange_);
        const svgElem = doc.createElementNS(SVG_NS, 'svg');
        svgElem.classList.add(cn('g'));
        svgElem.style.height = `calc(var(${getCssVar('containerUnitSize')}) * ${config.rows})`;
        this.element.appendChild(svgElem);
        this.svgElem_ = svgElem;
        const lineElem = doc.createElementNS(SVG_NS, 'polyline');
        this.svgElem_.appendChild(lineElem);
        this.lineElem_ = lineElem;
        const tooltipElem = doc.createElement('div');
        tooltipElem.classList.add(cn('t'), ClassName('tt')());
        this.element.appendChild(tooltipElem);
        this.tooltipElem_ = tooltipElem;
        config.value.emitter.on('change', this.onValueUpdate_);
        this.value = config.value;
        this.update_();
    }
    get graphElement() {
        return this.svgElem_;
    }
    update_() {
        const bounds = this.svgElem_.getBoundingClientRect();
        const maxIndex = this.value.rawValue.length - 1;
        const min = this.props_.get('min');
        const max = this.props_.get('max');
        const points = [];
        this.value.rawValue.forEach((v, index) => {
            if (v === undefined) {
                return;
            }
            const x = mapRange(index, 0, maxIndex, 0, bounds.width);
            const y = mapRange(v, min, max, bounds.height, 0);
            points.push([x, y].join(','));
        });
        this.lineElem_.setAttributeNS(null, 'points', points.join(' '));
        const tooltipElem = this.tooltipElem_;
        const value = this.value.rawValue[this.cursor_.rawValue];
        if (value === undefined) {
            tooltipElem.classList.remove(cn('t', 'a'));
            return;
        }
        const tx = mapRange(this.cursor_.rawValue, 0, maxIndex, 0, bounds.width);
        const ty = mapRange(value, min, max, bounds.height, 0);
        tooltipElem.style.left = `${tx}px`;
        tooltipElem.style.top = `${ty}px`;
        tooltipElem.textContent = `${this.formatter_(value)}`;
        if (!tooltipElem.classList.contains(cn('t', 'a'))) {
            tooltipElem.classList.add(cn('t', 'a'), cn('t', 'in'));
            forceReflow(tooltipElem);
            tooltipElem.classList.remove(cn('t', 'in'));
        }
    }
    onValueUpdate_() {
        this.update_();
    }
    onCursorChange_() {
        this.update_();
    }
}

class GraphLogController {
    constructor(doc, config) {
        this.onGraphMouseMove_ = this.onGraphMouseMove_.bind(this);
        this.onGraphMouseLeave_ = this.onGraphMouseLeave_.bind(this);
        this.onGraphPointerDown_ = this.onGraphPointerDown_.bind(this);
        this.onGraphPointerMove_ = this.onGraphPointerMove_.bind(this);
        this.onGraphPointerUp_ = this.onGraphPointerUp_.bind(this);
        this.props = config.props;
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.cursor_ = createValue(-1);
        this.view = new GraphLogView(doc, {
            cursor: this.cursor_,
            formatter: config.formatter,
            rows: config.rows,
            props: this.props,
            value: this.value,
            viewProps: this.viewProps,
        });
        if (!supportsTouch(doc)) {
            this.view.element.addEventListener('mousemove', this.onGraphMouseMove_);
            this.view.element.addEventListener('mouseleave', this.onGraphMouseLeave_);
        }
        else {
            const ph = new PointerHandler(this.view.element);
            ph.emitter.on('down', this.onGraphPointerDown_);
            ph.emitter.on('move', this.onGraphPointerMove_);
            ph.emitter.on('up', this.onGraphPointerUp_);
        }
    }
    importProps(state) {
        return importBladeState(state, null, (p) => ({
            max: p.required.number,
            min: p.required.number,
        }), (result) => {
            this.props.set('max', result.max);
            this.props.set('min', result.min);
            return true;
        });
    }
    exportProps() {
        return exportBladeState(null, {
            max: this.props.get('max'),
            min: this.props.get('min'),
        });
    }
    onGraphMouseLeave_() {
        this.cursor_.rawValue = -1;
    }
    onGraphMouseMove_(ev) {
        const bounds = this.view.element.getBoundingClientRect();
        this.cursor_.rawValue = Math.floor(mapRange(ev.offsetX, 0, bounds.width, 0, this.value.rawValue.length));
    }
    onGraphPointerDown_(ev) {
        this.onGraphPointerMove_(ev);
    }
    onGraphPointerMove_(ev) {
        if (!ev.data.point) {
            this.cursor_.rawValue = -1;
            return;
        }
        this.cursor_.rawValue = Math.floor(mapRange(ev.data.point.x, 0, ev.data.bounds.width, 0, this.value.rawValue.length));
    }
    onGraphPointerUp_() {
        this.cursor_.rawValue = -1;
    }
}

function createFormatter(params) {
    return !isEmpty(params.format) ? params.format : createNumberFormatter(2);
}
function createTextMonitor(args) {
    var _a;
    if (args.value.rawValue.length === 1) {
        return new SingleLogController(args.document, {
            formatter: createFormatter(args.params),
            value: args.value,
            viewProps: args.viewProps,
        });
    }
    return new MultiLogController(args.document, {
        formatter: createFormatter(args.params),
        rows: (_a = args.params.rows) !== null && _a !== void 0 ? _a : Constants.monitor.defaultRows,
        value: args.value,
        viewProps: args.viewProps,
    });
}
function createGraphMonitor(args) {
    var _a, _b, _c;
    return new GraphLogController(args.document, {
        formatter: createFormatter(args.params),
        rows: (_a = args.params.rows) !== null && _a !== void 0 ? _a : Constants.monitor.defaultRows,
        props: ValueMap.fromObject({
            max: (_b = args.params.max) !== null && _b !== void 0 ? _b : 100,
            min: (_c = args.params.min) !== null && _c !== void 0 ? _c : 0,
        }),
        value: args.value,
        viewProps: args.viewProps,
    });
}
function shouldShowGraph(params) {
    return params.view === 'graph';
}
createPlugin({
    id: 'monitor-number',
    type: 'monitor',
    accept: (value, params) => {
        if (typeof value !== 'number') {
            return null;
        }
        const result = parseRecord(params, (p) => ({
            format: p.optional.function,
            max: p.optional.number,
            min: p.optional.number,
            readonly: p.required.constant(true),
            rows: p.optional.number,
            view: p.optional.string,
        }));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        defaultBufferSize: (params) => (shouldShowGraph(params) ? 64 : 1),
        reader: (_args) => numberFromUnknown,
    },
    controller: (args) => {
        if (shouldShowGraph(args.params)) {
            return createGraphMonitor(args);
        }
        return createTextMonitor(args);
    },
    api: (args) => {
        if (args.controller.valueController instanceof GraphLogController) {
            return new GraphLogMonitorBindingApi(args.controller);
        }
        return null;
    },
});

createPlugin({
    id: 'monitor-string',
    type: 'monitor',
    accept: (value, params) => {
        if (typeof value !== 'string') {
            return null;
        }
        const result = parseRecord(params, (p) => ({
            multiline: p.optional.boolean,
            readonly: p.required.constant(true),
            rows: p.optional.number,
        }));
        return result
            ? {
                initialValue: value,
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => stringFromUnknown,
    },
    controller: (args) => {
        var _a;
        const value = args.value;
        const multiline = value.rawValue.length > 1 || args.params.multiline;
        if (multiline) {
            return new MultiLogController(args.document, {
                formatter: formatString,
                rows: (_a = args.params.rows) !== null && _a !== void 0 ? _a : Constants.monitor.defaultRows,
                value: value,
                viewProps: args.viewProps,
            });
        }
        return new SingleLogController(args.document, {
            formatter: formatString,
            value: value,
            viewProps: args.viewProps,
        });
    },
});

class ButtonCellApi {
    constructor(controller) {
        this.controller_ = controller;
    }
    get disabled() {
        return this.controller_.viewProps.get('disabled');
    }
    set disabled(disabled) {
        this.controller_.viewProps.set('disabled', disabled);
    }
    get title() {
        var _a;
        return (_a = this.controller_.props.get('title')) !== null && _a !== void 0 ? _a : '';
    }
    set title(title) {
        this.controller_.props.set('title', title);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        const emitter = this.controller_.emitter;
        emitter.on(eventName, () => {
            bh(new TpEvent(this));
        });
        return this;
    }
}

class TpButtonGridEvent extends TpEvent {
    constructor(target, cell, index) {
        super(target);
        this.cell = cell;
        this.index = index;
    }
}

class ButtonGridApi extends BladeApi {
    constructor(controller) {
        super(controller);
        this.cellToApiMap_ = new Map();
        this.emitter_ = new Emitter();
        const gc = this.controller.valueController;
        gc.cellControllers.forEach((cc, i) => {
            const api = new ButtonCellApi(cc);
            this.cellToApiMap_.set(cc, api);
            cc.emitter.on('click', () => {
                const x = i % gc.size[0];
                const y = Math.floor(i / gc.size[0]);
                this.emitter_.emit('click', {
                    event: new TpButtonGridEvent(this, api, [x, y]),
                });
            });
        });
    }
    cell(x, y) {
        const gc = this.controller.valueController;
        const cc = gc.cellControllers[y * gc.size[0] + x];
        return this.cellToApiMap_.get(cc);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.emitter_.on(eventName, (ev) => {
            bh(ev.event);
        });
        return this;
    }
}

class ButtonGridController {
    constructor(doc, config) {
        this.size = config.size;
        const [w, h] = this.size;
        const bcs = [];
        for (let y = 0; y < h; y++) {
            for (let x = 0; x < w; x++) {
                const bc = new ButtonController(doc, {
                    props: ValueMap.fromObject(Object.assign({}, config.cellConfig(x, y))),
                    viewProps: ViewProps.create(),
                });
                bcs.push(bc);
            }
        }
        this.cellCs_ = bcs;
        this.viewProps = ViewProps.create();
        this.viewProps.handleDispose(() => {
            this.cellCs_.forEach((c) => {
                c.viewProps.set('disposed', true);
            });
        });
        this.view = new PlainView(doc, {
            viewProps: this.viewProps,
            viewName: 'btngrid',
        });
        this.view.element.style.gridTemplateColumns = `repeat(${w}, 1fr)`;
        this.cellCs_.forEach((bc) => {
            this.view.element.appendChild(bc.view.element);
        });
    }
    get cellControllers() {
        return this.cellCs_;
    }
}

class ButtonGridBladeController extends BladeController {
    constructor(doc, config) {
        const bc = config.valueController;
        const lc = new LabelController(doc, {
            blade: config.blade,
            props: config.labelProps,
            valueController: bc,
        });
        super({
            blade: config.blade,
            view: lc.view,
            viewProps: bc.viewProps,
        });
        this.valueController = bc;
        this.labelController = lc;
    }
}

const ButtonGridBladePlugin = createPlugin({
    id: 'buttongrid',
    type: 'blade',
    accept(params) {
        const result = parseRecord(params, (p) => ({
            cells: p.required.function,
            size: p.required.array(p.required.number),
            view: p.required.constant('buttongrid'),
            label: p.optional.string,
        }));
        return result ? { params: result } : null;
    },
    controller(args) {
        return new ButtonGridBladeController(args.document, {
            blade: args.blade,
            labelProps: ValueMap.fromObject({
                label: args.params.label,
            }),
            valueController: new ButtonGridController(args.document, {
                cellConfig: args.params.cells,
                size: args.params.size,
            }),
        });
    },
    api(args) {
        if (args.controller instanceof ButtonGridBladeController) {
            return new ButtonGridApi(args.controller);
        }
        return null;
    },
});

class CubicBezierApi extends BladeApi {
    get label() {
        return this.controller.labelController.props.get('label');
    }
    set label(label) {
        this.controller.labelController.props.set('label', label);
    }
    get value() {
        return this.controller.valueController.value.rawValue;
    }
    set value(value) {
        this.controller.valueController.value.rawValue = value;
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.controller.valueController.value.emitter.on(eventName, (ev) => {
            bh(new TpChangeEvent(this, ev.rawValue, ev.options.last));
        });
        return this;
    }
}

function interpolate(x1, x2, t) {
    return x1 * (1 - t) + x2 * t;
}
const MAX_ITERATION = 20;
const X_DELTA = 0.001;
const CACHE_RESOLUTION = 100;
function y(cb, x) {
    let dt = 0.25;
    let t = 0.5;
    let y = -1;
    for (let i = 0; i < MAX_ITERATION; i++) {
        const [tx, ty] = cb.curve(t);
        t += dt * (tx < x ? +1 : -1);
        y = ty;
        dt *= 0.5;
        if (Math.abs(x - tx) < X_DELTA) {
            break;
        }
    }
    return y;
}
class CubicBezier {
    constructor(x1 = 0, y1 = 0, x2 = 1, y2 = 1) {
        this.cache_ = [];
        this.comps_ = [x1, y1, x2, y2];
    }
    get x1() {
        return this.comps_[0];
    }
    get y1() {
        return this.comps_[1];
    }
    get x2() {
        return this.comps_[2];
    }
    get y2() {
        return this.comps_[3];
    }
    static isObject(obj) {
        if (isEmpty(obj)) {
            return false;
        }
        if (!Array.isArray(obj)) {
            return false;
        }
        return (typeof obj[0] === 'number' &&
            typeof obj[1] === 'number' &&
            typeof obj[2] === 'number' &&
            typeof obj[3] === 'number');
    }
    static equals(v1, v2) {
        return (v1.x1 === v2.x1 && v1.y1 === v2.y1 && v1.x2 === v2.x2 && v1.y2 === v2.y2);
    }
    curve(t) {
        const x01 = interpolate(0, this.x1, t);
        const y01 = interpolate(0, this.y1, t);
        const x12 = interpolate(this.x1, this.x2, t);
        const y12 = interpolate(this.y1, this.y2, t);
        const x23 = interpolate(this.x2, 1, t);
        const y23 = interpolate(this.y2, 1, t);
        const xr0 = interpolate(x01, x12, t);
        const yr0 = interpolate(y01, y12, t);
        const xr1 = interpolate(x12, x23, t);
        const yr1 = interpolate(y12, y23, t);
        return [interpolate(xr0, xr1, t), interpolate(yr0, yr1, t)];
    }
    y(x) {
        if (this.cache_.length === 0) {
            const cache = [];
            for (let i = 0; i < CACHE_RESOLUTION; i++) {
                cache.push(y(this, mapRange(i, 0, CACHE_RESOLUTION - 1, 0, 1)));
            }
            this.cache_ = cache;
        }
        return this.cache_[Math.round(mapRange(constrainRange(x, 0, 1), 0, 1, 0, CACHE_RESOLUTION - 1))];
    }
    toObject() {
        return [this.comps_[0], this.comps_[1], this.comps_[2], this.comps_[3]];
    }
}
const CubicBezierAssembly = {
    toComponents: (p) => p.toObject(),
    fromComponents: (comps) => new CubicBezier(...comps),
};

function cubicBezierToString(cb) {
    const formatter = createNumberFormatter(2);
    const comps = cb.toObject().map((c) => formatter(c));
    return `cubic-bezier(${comps.join(', ')})`;
}
const COMPS_EMPTY = [0, 0.5, 0.5, 1];
function cubicBezierFromString(text) {
    const m = text.match(/^cubic-bezier\s*\(\s*([0-9.]+)\s*,\s*([0-9.]+)\s*,\s*([0-9.]+)\s*,\s*([0-9.]+)\s*\)$/);
    if (!m) {
        return new CubicBezier(...COMPS_EMPTY);
    }
    const comps = [m[1], m[2], m[3], m[4]].reduce((comps, comp) => {
        if (!comps) {
            return null;
        }
        const n = Number(comp);
        if (isNaN(n)) {
            return null;
        }
        return [...comps, n];
    }, []);
    return new CubicBezier(...(comps !== null && comps !== void 0 ? comps : COMPS_EMPTY));
}

const className$7 = ClassName('cbz');
class CubicBezierView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(className$7());
        config.viewProps.bindClassModifiers(this.element);
        config.foldable.bindExpandedClass(this.element, className$7(undefined, 'expanded'));
        bindValueMap(config.foldable, 'completed', valueToClassName(this.element, className$7(undefined, 'cpl')));
        const headElem = doc.createElement('div');
        headElem.classList.add(className$7('h'));
        this.element.appendChild(headElem);
        const buttonElem = doc.createElement('button');
        buttonElem.classList.add(className$7('b'));
        config.viewProps.bindDisabled(buttonElem);
        const iconElem = doc.createElementNS(SVG_NS, 'svg');
        iconElem.innerHTML = '<path d="M2 13C8 13 8 3 14 3"/>';
        buttonElem.appendChild(iconElem);
        headElem.appendChild(buttonElem);
        this.buttonElement = buttonElem;
        const textElem = doc.createElement('div');
        textElem.classList.add(className$7('t'));
        headElem.appendChild(textElem);
        this.textElement = textElem;
        if (config.pickerLayout === 'inline') {
            const pickerElem = doc.createElement('div');
            pickerElem.classList.add(className$7('p'));
            this.element.appendChild(pickerElem);
            this.pickerElement = pickerElem;
        }
        else {
            this.pickerElement = null;
        }
    }
}

const className$6 = ClassName('cbzp');
class CubicBezierPickerView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(className$6());
        config.viewProps.bindClassModifiers(this.element);
        const graphElem = doc.createElement('div');
        graphElem.classList.add(className$6('g'));
        this.element.appendChild(graphElem);
        this.graphElement = graphElem;
        const textElem = doc.createElement('div');
        textElem.classList.add(className$6('t'));
        this.element.appendChild(textElem);
        this.textElement = textElem;
    }
}

function waitToBeAddedToDom(elem, callback) {
    const ob = new MutationObserver((ml) => {
        for (const m of ml) {
            if (m.type !== 'childList') {
                continue;
            }
            m.addedNodes.forEach((elem) => {
                if (!elem.contains(elem)) {
                    return;
                }
                callback();
                ob.disconnect();
            });
        }
    });
    const doc = elem.ownerDocument;
    ob.observe(doc.body, {
        attributes: true,
        childList: true,
        subtree: true,
    });
}

const className$5 = ClassName('cbzg');
// TODO: Apply to core
function compose(h1, h2) {
    return (input) => h2(h1(input));
}
class CubicBezierGraphView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(className$5());
        config.viewProps.bindClassModifiers(this.element);
        config.viewProps.bindTabIndex(this.element);
        const previewElem = doc.createElement('div');
        previewElem.classList.add(className$5('p'));
        this.element.appendChild(previewElem);
        this.previewElement = previewElem;
        const svgElem = doc.createElementNS(SVG_NS, 'svg');
        svgElem.classList.add(className$5('g'));
        this.element.appendChild(svgElem);
        this.svgElem_ = svgElem;
        const guideElem = doc.createElementNS(SVG_NS, 'path');
        guideElem.classList.add(className$5('u'));
        this.svgElem_.appendChild(guideElem);
        this.guideElem_ = guideElem;
        const lineElem = doc.createElementNS(SVG_NS, 'polyline');
        lineElem.classList.add(className$5('l'));
        this.svgElem_.appendChild(lineElem);
        this.lineElem_ = lineElem;
        this.handleElems_ = [doc.createElement('div'), doc.createElement('div')];
        this.handleElems_.forEach((elem) => {
            elem.classList.add(className$5('h'));
            this.element.appendChild(elem);
        });
        this.vectorElems_ = [
            doc.createElementNS(SVG_NS, 'line'),
            doc.createElementNS(SVG_NS, 'line'),
        ];
        this.vectorElems_.forEach((elem) => {
            elem.classList.add(className$5('v'));
            this.svgElem_.appendChild(elem);
        });
        this.value_ = config.value;
        this.value_.emitter.on('change', this.onValueChange_.bind(this));
        this.sel_ = config.selection;
        this.handleElems_.forEach((elem, index) => {
            bindValue(this.sel_, compose((selection) => selection === index, valueToClassName(elem, className$5('h', 'sel'))));
        });
        waitToBeAddedToDom(this.element, () => {
            this.refresh();
        });
    }
    getVertMargin_(h) {
        return h * 0.25;
    }
    valueToPosition(x, y) {
        const { clientWidth: w, clientHeight: h } = this.element;
        const vm = this.getVertMargin_(h);
        return {
            x: mapRange(x, 0, 1, 0, w),
            y: mapRange(y, 0, 1, h - vm, vm),
        };
    }
    positionToValue(x, y) {
        const bounds = this.element.getBoundingClientRect();
        const w = bounds.width;
        const h = bounds.height;
        const vm = this.getVertMargin_(h);
        return {
            x: constrainRange(mapRange(x, 0, w, 0, 1), 0, 1),
            y: mapRange(y, h - vm, vm, 0, 1),
        };
    }
    refresh() {
        this.guideElem_.setAttributeNS(null, 'd', [0, 1]
            .map((index) => {
            const p1 = this.valueToPosition(0, index);
            const p2 = this.valueToPosition(1, index);
            return [`M ${p1.x},${p1.y}`, `L ${p2.x},${p2.y}`].join(' ');
        })
            .join(' '));
        const bezier = this.value_.rawValue;
        const points = [];
        let t = 0;
        for (;;) {
            const p = this.valueToPosition(...bezier.curve(t));
            points.push([p.x, p.y].join(','));
            if (t >= 1) {
                break;
            }
            t = Math.min(t + 0.05, 1);
        }
        this.lineElem_.setAttributeNS(null, 'points', points.join(' '));
        const obj = bezier.toObject();
        [0, 1].forEach((index) => {
            const p1 = this.valueToPosition(index, index);
            const p2 = this.valueToPosition(obj[index * 2], obj[index * 2 + 1]);
            const vElem = this.vectorElems_[index];
            vElem.setAttributeNS(null, 'x1', String(p1.x));
            vElem.setAttributeNS(null, 'y1', String(p1.y));
            vElem.setAttributeNS(null, 'x2', String(p2.x));
            vElem.setAttributeNS(null, 'y2', String(p2.y));
            const hElem = this.handleElems_[index];
            hElem.style.left = `${p2.x}px`;
            hElem.style.top = `${p2.y}px`;
        });
    }
    onValueChange_() {
        this.refresh();
    }
}

const TICK_COUNT = 24;
const PREVIEW_DELAY = 400;
const PREVIEW_DURATION = 1000;
const className$4 = ClassName('cbzprv');
class CubicBezierPreviewView {
    constructor(doc, config) {
        this.stopped_ = true;
        this.startTime_ = -1;
        this.onDispose_ = this.onDispose_.bind(this);
        this.onTimer_ = this.onTimer_.bind(this);
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.element = doc.createElement('div');
        this.element.classList.add(className$4());
        config.viewProps.bindClassModifiers(this.element);
        const svgElem = doc.createElementNS(SVG_NS, 'svg');
        svgElem.classList.add(className$4('g'));
        this.element.appendChild(svgElem);
        this.svgElem_ = svgElem;
        const ticksElem = doc.createElementNS(SVG_NS, 'path');
        ticksElem.classList.add(className$4('t'));
        this.svgElem_.appendChild(ticksElem);
        this.ticksElem_ = ticksElem;
        const markerElem = doc.createElement('div');
        markerElem.classList.add(className$4('m'));
        this.element.appendChild(markerElem);
        this.markerElem_ = markerElem;
        this.value_ = config.value;
        this.value_.emitter.on('change', this.onValueChange_);
        config.viewProps.handleDispose(this.onDispose_);
        waitToBeAddedToDom(this.element, () => {
            this.refresh();
        });
    }
    play() {
        this.stop();
        this.updateMarker_(0);
        this.markerElem_.classList.add(className$4('m', 'a'));
        this.startTime_ = new Date().getTime() + PREVIEW_DELAY;
        this.stopped_ = false;
        requestAnimationFrame(this.onTimer_);
    }
    stop() {
        this.stopped_ = true;
        this.markerElem_.classList.remove(className$4('m', 'a'));
    }
    onDispose_() {
        this.stop();
    }
    updateMarker_(progress) {
        const p = this.value_.rawValue.y(constrainRange(progress, 0, 1));
        this.markerElem_.style.left = `${p * 100}%`;
    }
    refresh() {
        const { clientWidth: w, clientHeight: h } = this.svgElem_;
        const ds = [];
        const bezier = this.value_.rawValue;
        for (let i = 0; i < TICK_COUNT; i++) {
            const px = mapRange(i, 0, TICK_COUNT - 1, 0, 1);
            const x = mapRange(bezier.y(px), 0, 1, 0, w);
            ds.push(`M ${x},0 v${h}`);
        }
        this.ticksElem_.setAttributeNS(null, 'd', ds.join(' '));
    }
    onTimer_() {
        if (this.startTime_ === null) {
            return;
        }
        const dt = new Date().getTime() - this.startTime_;
        const p = dt / PREVIEW_DURATION;
        this.updateMarker_(p);
        if (dt > PREVIEW_DURATION + PREVIEW_DELAY) {
            this.stop();
        }
        if (!this.stopped_) {
            requestAnimationFrame(this.onTimer_);
        }
    }
    onValueChange_() {
        this.refresh();
        this.play();
    }
}

function getDistance(x1, y1, x2, y2) {
    const dx = x2 - x1;
    const dy = y2 - y1;
    return Math.sqrt(dx * dx + dy * dy);
}
function lockAngle(x1, y1, x2, y2) {
    const d = getDistance(x1, y1, x2, y2);
    const a = Math.atan2(y2 - y1, x2 - x1);
    const la = (Math.round(a / (Math.PI / 4)) * Math.PI) / 4;
    return {
        x: x1 + Math.cos(la) * d,
        y: y1 + Math.sin(la) * d,
    };
}
class CubicBezierGraphController {
    constructor(doc, config) {
        this.onKeyDown_ = this.onKeyDown_.bind(this);
        this.onKeyUp_ = this.onKeyUp_.bind(this);
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.keyScale_ = config.keyScale;
        this.value = config.value;
        this.sel_ = createValue(0);
        this.viewProps = config.viewProps;
        this.view = new CubicBezierGraphView(doc, {
            selection: this.sel_,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.element.addEventListener('keydown', this.onKeyDown_);
        this.view.element.addEventListener('keyup', this.onKeyUp_);
        this.prevView_ = new CubicBezierPreviewView(doc, {
            value: this.value,
            viewProps: this.viewProps,
        });
        this.prevView_.element.addEventListener('mousedown', (ev) => {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.prevView_.play();
        });
        this.view.previewElement.appendChild(this.prevView_.element);
        const ptHandler = new PointerHandler(this.view.element);
        ptHandler.emitter.on('down', this.onPointerDown_);
        ptHandler.emitter.on('move', this.onPointerMove_);
        ptHandler.emitter.on('up', this.onPointerUp_);
    }
    refresh() {
        this.view.refresh();
        this.prevView_.refresh();
        this.prevView_.play();
    }
    updateValue_(point, locksAngle, opts) {
        const index = this.sel_.rawValue;
        const comps = this.value.rawValue.toObject();
        const vp = this.view.positionToValue(point.x, point.y);
        const v = locksAngle ? lockAngle(index, index, vp.x, vp.y) : vp;
        comps[index * 2] = v.x;
        comps[index * 2 + 1] = v.y;
        this.value.setRawValue(new CubicBezier(...comps), opts);
    }
    onPointerDown_(ev) {
        const data = ev.data;
        if (!data.point) {
            return;
        }
        const bezier = this.value.rawValue;
        const p1 = this.view.valueToPosition(bezier.x1, bezier.y1);
        const d1 = getDistance(data.point.x, data.point.y, p1.x, p1.y);
        const p2 = this.view.valueToPosition(bezier.x2, bezier.y2);
        const d2 = getDistance(data.point.x, data.point.y, p2.x, p2.y);
        this.sel_.rawValue = d1 <= d2 ? 0 : 1;
        this.updateValue_(data.point, ev.shiftKey, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerMove_(ev) {
        const data = ev.data;
        if (!data.point) {
            return;
        }
        this.updateValue_(data.point, ev.shiftKey, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        const data = ev.data;
        if (!data.point) {
            return;
        }
        this.updateValue_(data.point, ev.shiftKey, {
            forceEmit: true,
            last: true,
        });
    }
    onKeyDown_(ev) {
        if (isArrowKey(ev.key)) {
            ev.preventDefault();
        }
        const index = this.sel_.rawValue;
        const comps = this.value.rawValue.toObject();
        const keyScale = this.keyScale_.rawValue;
        comps[index * 2] += getStepForKey(keyScale, getHorizontalStepKeys(ev));
        comps[index * 2 + 1] += getStepForKey(keyScale, getVerticalStepKeys(ev));
        this.value.setRawValue(new CubicBezier(...comps), {
            forceEmit: false,
            last: false,
        });
    }
    onKeyUp_(ev) {
        if (isArrowKey(ev.key)) {
            ev.preventDefault();
        }
        const keyScale = this.keyScale_.rawValue;
        const xStep = getStepForKey(keyScale, getHorizontalStepKeys(ev));
        const yStep = getStepForKey(keyScale, getVerticalStepKeys(ev));
        if (xStep === 0 && yStep === 0) {
            return;
        }
        this.value.setRawValue(this.value.rawValue, {
            forceEmit: true,
            last: true,
        });
    }
}

class CubicBezierPickerController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.view = new CubicBezierPickerView(doc, {
            viewProps: this.viewProps,
        });
        this.gc_ = new CubicBezierGraphController(doc, {
            keyScale: config.axis.textProps.value('keyScale'),
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.graphElement.appendChild(this.gc_.view.element);
        const xAxis = Object.assign(Object.assign({}, config.axis), { constraint: new RangeConstraint({ max: 1, min: 0 }) });
        const yAxis = Object.assign(Object.assign({}, config.axis), { constraint: undefined });
        this.tc_ = new PointNdTextController(doc, {
            assembly: CubicBezierAssembly,
            axes: [xAxis, yAxis, xAxis, yAxis],
            parser: parseNumber,
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.textElement.appendChild(this.tc_.view.element);
    }
    get allFocusableElements() {
        return [
            this.gc_.view.element,
            ...this.tc_.view.textViews.map((v) => v.inputElement),
        ];
    }
    refresh() {
        this.gc_.refresh();
    }
}

class CubicBezierController {
    constructor(doc, config) {
        this.onButtonBlur_ = this.onButtonBlur_.bind(this);
        this.onButtonClick_ = this.onButtonClick_.bind(this);
        this.onPopupChildBlur_ = this.onPopupChildBlur_.bind(this);
        this.onPopupChildKeydown_ = this.onPopupChildKeydown_.bind(this);
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.foldable_ = Foldable.create(config.expanded);
        this.view = new CubicBezierView(doc, {
            foldable: this.foldable_,
            pickerLayout: config.pickerLayout,
            viewProps: this.viewProps,
        });
        this.view.buttonElement.addEventListener('blur', this.onButtonBlur_);
        this.view.buttonElement.addEventListener('click', this.onButtonClick_);
        this.tc_ = new TextController(doc, {
            parser: cubicBezierFromString,
            props: ValueMap.fromObject({
                formatter: cubicBezierToString,
            }),
            value: this.value,
            viewProps: this.viewProps,
        });
        this.view.textElement.appendChild(this.tc_.view.element);
        this.popC_ =
            config.pickerLayout === 'popup'
                ? new PopupController(doc, {
                    viewProps: this.viewProps,
                })
                : null;
        const pickerC = new CubicBezierPickerController(doc, {
            axis: config.axis,
            value: this.value,
            viewProps: this.viewProps,
        });
        pickerC.allFocusableElements.forEach((elem) => {
            elem.addEventListener('blur', this.onPopupChildBlur_);
            elem.addEventListener('keydown', this.onPopupChildKeydown_);
        });
        this.pickerC_ = pickerC;
        if (this.popC_) {
            this.view.element.appendChild(this.popC_.view.element);
            this.popC_.view.element.appendChild(this.pickerC_.view.element);
            bindValue(this.popC_.shows, (shows) => {
                if (shows) {
                    pickerC.refresh();
                }
            });
            connectValues({
                primary: this.foldable_.value('expanded'),
                secondary: this.popC_.shows,
                forward: (p) => p,
                backward: (_, s) => s,
            });
        }
        else if (this.view.pickerElement) {
            this.view.pickerElement.appendChild(this.pickerC_.view.element);
            bindFoldable(this.foldable_, this.view.pickerElement);
        }
    }
    onButtonBlur_(ev) {
        if (!this.popC_) {
            return;
        }
        const nextTarget = forceCast(ev.relatedTarget);
        if (!nextTarget || !this.popC_.view.element.contains(nextTarget)) {
            this.popC_.shows.rawValue = false;
        }
    }
    onButtonClick_() {
        this.foldable_.set('expanded', !this.foldable_.get('expanded'));
        if (this.foldable_.get('expanded')) {
            this.pickerC_.allFocusableElements[0].focus();
        }
    }
    onPopupChildBlur_(ev) {
        if (!this.popC_) {
            return;
        }
        const elem = this.popC_.view.element;
        const nextTarget = findNextTarget(ev);
        if (nextTarget && elem.contains(nextTarget)) {
            // Next target is in the popup
            return;
        }
        if (nextTarget &&
            nextTarget === this.view.buttonElement &&
            !supportsTouch(elem.ownerDocument)) {
            // Next target is the trigger button
            return;
        }
        this.popC_.shows.rawValue = false;
    }
    onPopupChildKeydown_(ev) {
        if (!this.popC_) {
            return;
        }
        if (ev.key === 'Escape') {
            this.popC_.shows.rawValue = false;
        }
    }
}

function createConstraint$1() {
    return new PointNdConstraint({
        assembly: CubicBezierAssembly,
        components: [0, 1, 2, 3].map((index) => index % 2 === 0
            ? new RangeConstraint({
                min: 0,
                max: 1,
            })
            : undefined),
    });
}
const CubicBezierBladePlugin = createPlugin({
    id: 'cubicbezier',
    type: 'blade',
    accept(params) {
        const result = parseRecord(params, (p) => ({
            value: p.required.array(p.required.number),
            view: p.required.constant('cubicbezier'),
            expanded: p.optional.boolean,
            label: p.optional.string,
            picker: p.optional.custom((v) => {
                return v === 'inline' || v === 'popup' ? v : undefined;
            }),
        }));
        return result ? { params: result } : null;
    },
    controller(args) {
        var _a, _b;
        const rv = new CubicBezier(...args.params.value);
        const v = createValue(rv, {
            constraint: createConstraint$1(),
            equals: CubicBezier.equals,
        });
        const vc = new CubicBezierController(args.document, {
            axis: {
                textProps: ValueMap.fromObject({
                    keyScale: 0.1,
                    pointerScale: 0.01,
                    formatter: createNumberFormatter(2),
                }),
            },
            expanded: (_a = args.params.expanded) !== null && _a !== void 0 ? _a : false,
            pickerLayout: (_b = args.params.picker) !== null && _b !== void 0 ? _b : 'popup',
            value: v,
            viewProps: args.viewProps,
        });
        return new LabeledValueBladeController(args.document, {
            blade: args.blade,
            props: ValueMap.fromObject({
                label: args.params.label,
            }),
            value: v,
            valueController: vc,
        });
    },
    api(args) {
        if (!(args.controller instanceof LabeledValueBladeController)) {
            return null;
        }
        if (!(args.controller.valueController instanceof CubicBezierController)) {
            return null;
        }
        return new CubicBezierApi(args.controller);
    },
});

class FpsGraphBladeApi extends BladeApi {
    get fps() {
        return this.controller.valueController.fps;
    }
    get max() {
        return this.controller.valueController.props.get('max');
    }
    set max(max) {
        this.controller.valueController.props.set('max', max);
    }
    get min() {
        return this.controller.valueController.props.get('min');
    }
    set min(min) {
        this.controller.valueController.props.set('min', min);
    }
    begin() {
        this.controller.valueController.begin();
    }
    end() {
        this.controller.valueController.end();
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        const emitter = this.controller.valueController.ticker.emitter;
        emitter.on(eventName, () => {
            bh(new TpEvent(this));
        });
        return this;
    }
}

const MAX_TIMESTAMPS = 20;
class Fpswatch {
    constructor() {
        this.start_ = null;
        this.duration_ = 0;
        this.fps_ = null;
        this.frameCount_ = 0;
        this.timestamps_ = [];
    }
    get duration() {
        return this.duration_;
    }
    get fps() {
        return this.fps_;
    }
    begin(now) {
        this.start_ = now.getTime();
    }
    calculateFps_(nowTime) {
        if (this.timestamps_.length === 0) {
            return null;
        }
        const ts = this.timestamps_[0];
        return (1000 * (this.frameCount_ - ts.frameCount)) / (nowTime - ts.time);
    }
    compactTimestamps_() {
        if (this.timestamps_.length <= MAX_TIMESTAMPS) {
            return;
        }
        const len = this.timestamps_.length - MAX_TIMESTAMPS;
        this.timestamps_.splice(0, len);
        const df = this.timestamps_[0].frameCount;
        this.timestamps_.forEach((ts) => {
            ts.frameCount -= df;
        });
        this.frameCount_ -= df;
    }
    end(now) {
        if (this.start_ === null) {
            return;
        }
        const t = now.getTime();
        this.duration_ = t - this.start_;
        this.start_ = null;
        this.fps_ = this.calculateFps_(t);
        this.timestamps_.push({
            frameCount: this.frameCount_,
            time: t,
        });
        ++this.frameCount_;
        this.compactTimestamps_();
    }
}

const className$3 = ClassName('fps');
class FpsView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(className$3());
        config.viewProps.bindClassModifiers(this.element);
        this.graphElement = doc.createElement('div');
        this.graphElement.classList.add(className$3('g'));
        this.element.appendChild(this.graphElement);
        const labelElement = doc.createElement('div');
        labelElement.classList.add(className$3('l'));
        this.element.appendChild(labelElement);
        const valueElement = doc.createElement('span');
        valueElement.classList.add(className$3('v'));
        valueElement.textContent = '--';
        labelElement.appendChild(valueElement);
        this.valueElement = valueElement;
        const unitElement = doc.createElement('span');
        unitElement.classList.add(className$3('u'));
        unitElement.textContent = 'FPS';
        labelElement.appendChild(unitElement);
    }
}

class FpsGraphController {
    constructor(doc, config) {
        this.stopwatch_ = new Fpswatch();
        this.onTick_ = this.onTick_.bind(this);
        this.ticker = config.ticker;
        this.ticker.emitter.on('tick', this.onTick_);
        this.props = config.props;
        this.value_ = config.value;
        this.viewProps = config.viewProps;
        this.view = new FpsView(doc, {
            viewProps: this.viewProps,
        });
        this.graphC_ = new GraphLogController(doc, {
            formatter: createNumberFormatter(0),
            props: this.props,
            rows: config.rows,
            value: this.value_,
            viewProps: this.viewProps,
        });
        this.view.graphElement.appendChild(this.graphC_.view.element);
        this.viewProps.handleDispose(() => {
            this.graphC_.viewProps.set('disposed', true);
            this.ticker.dispose();
        });
    }
    get fps() {
        return this.stopwatch_.fps;
    }
    begin() {
        this.stopwatch_.begin(new Date());
    }
    end() {
        this.stopwatch_.end(new Date());
    }
    onTick_() {
        const fps = this.fps;
        if (fps !== null) {
            const buffer = this.value_.rawValue;
            this.value_.rawValue = createPushedBuffer(buffer, fps);
            this.view.valueElement.textContent = fps.toFixed(0);
        }
    }
}

class FpsGraphBladeController extends BladeController {
    constructor(doc, config) {
        const fc = config.valueController;
        const lc = new LabelController(doc, {
            blade: config.blade,
            props: config.labelProps,
            valueController: fc,
        });
        super({
            blade: config.blade,
            view: lc.view,
            viewProps: fc.viewProps,
        });
        this.valueController = fc;
        this.labelController = lc;
    }
}

function createTicker(document, interval) {
    return interval === 0
        ? new ManualTicker()
        : new IntervalTicker(document, interval !== null && interval !== void 0 ? interval : Constants.monitor.defaultInterval);
}
const FpsGraphBladePlugin = createPlugin({
    id: 'fpsgraph',
    type: 'blade',
    accept(params) {
        const result = parseRecord(params, (p) => ({
            view: p.required.constant('fpsgraph'),
            interval: p.optional.number,
            label: p.optional.string,
            rows: p.optional.number,
            max: p.optional.number,
            min: p.optional.number,
        }));
        return result ? { params: result } : null;
    },
    controller(args) {
        var _a, _b, _c, _d;
        const interval = (_a = args.params.interval) !== null && _a !== void 0 ? _a : 500;
        return new FpsGraphBladeController(args.document, {
            blade: args.blade,
            labelProps: ValueMap.fromObject({
                label: args.params.label,
            }),
            valueController: new FpsGraphController(args.document, {
                props: ValueMap.fromObject({
                    max: (_b = args.params.max) !== null && _b !== void 0 ? _b : 90,
                    min: (_c = args.params.min) !== null && _c !== void 0 ? _c : 0,
                }),
                rows: (_d = args.params.rows) !== null && _d !== void 0 ? _d : 2,
                ticker: createTicker(args.document, interval),
                value: createValue(initializeBuffer(80)),
                viewProps: args.viewProps,
            }),
        });
    },
    api(args) {
        if (!(args.controller instanceof FpsGraphBladeController)) {
            return null;
        }
        return new FpsGraphBladeApi(args.controller);
    },
});

class Interval {
    constructor(min, max) {
        this.min = min;
        this.max = max;
    }
    static isObject(obj) {
        if (typeof obj !== 'object' || obj === null) {
            return false;
        }
        const min = obj.min;
        const max = obj.max;
        if (typeof min !== 'number' || typeof max !== 'number') {
            return false;
        }
        return true;
    }
    static equals(v1, v2) {
        return v1.min === v2.min && v1.max === v2.max;
    }
    get length() {
        return this.max - this.min;
    }
    toObject() {
        return {
            min: this.min,
            max: this.max,
        };
    }
}
const IntervalAssembly = {
    fromComponents: (comps) => new Interval(comps[0], comps[1]),
    toComponents: (p) => [p.min, p.max],
};

class IntervalConstraint {
    constructor(edge) {
        this.edge = edge;
    }
    constrain(value) {
        var _a, _b, _c, _d, _e, _f, _g, _h;
        if (value.min <= value.max) {
            return new Interval((_b = (_a = this.edge) === null || _a === void 0 ? void 0 : _a.constrain(value.min)) !== null && _b !== void 0 ? _b : value.min, (_d = (_c = this.edge) === null || _c === void 0 ? void 0 : _c.constrain(value.max)) !== null && _d !== void 0 ? _d : value.max);
        }
        const c = (value.min + value.max) / 2;
        return new Interval((_f = (_e = this.edge) === null || _e === void 0 ? void 0 : _e.constrain(c)) !== null && _f !== void 0 ? _f : c, (_h = (_g = this.edge) === null || _g === void 0 ? void 0 : _g.constrain(c)) !== null && _h !== void 0 ? _h : c);
    }
}

const className$2 = ClassName('rsltxt');
class RangeSliderTextView {
    constructor(doc, config) {
        this.sliderView_ = config.sliderView;
        this.textView_ = config.textView;
        this.element = doc.createElement('div');
        this.element.classList.add(className$2());
        const sliderElem = doc.createElement('div');
        sliderElem.classList.add(className$2('s'));
        sliderElem.appendChild(this.sliderView_.element);
        this.element.appendChild(sliderElem);
        const textElem = doc.createElement('div');
        textElem.classList.add(className$2('t'));
        textElem.appendChild(this.textView_.element);
        this.element.appendChild(textElem);
    }
}

const className$1 = ClassName('rsl');
class RangeSliderView {
    constructor(doc, config) {
        this.onSliderPropsChange_ = this.onSliderPropsChange_.bind(this);
        this.onValueChange_ = this.onValueChange_.bind(this);
        this.sliderProps_ = config.sliderProps;
        this.sliderProps_.emitter.on('change', this.onSliderPropsChange_);
        this.element = doc.createElement('div');
        this.element.classList.add(className$1());
        config.viewProps.bindClassModifiers(this.element);
        this.value_ = config.value;
        this.value_.emitter.on('change', this.onValueChange_);
        const trackElem = doc.createElement('div');
        trackElem.classList.add(className$1('t'));
        this.element.appendChild(trackElem);
        this.trackElement = trackElem;
        const barElem = doc.createElement('div');
        barElem.classList.add(className$1('b'));
        trackElem.appendChild(barElem);
        this.barElement = barElem;
        const knobElems = ['min', 'max'].map((modifier) => {
            const elem = doc.createElement('div');
            elem.classList.add(className$1('k'), className$1('k', modifier));
            trackElem.appendChild(elem);
            return elem;
        });
        this.knobElements = [knobElems[0], knobElems[1]];
        this.update_();
    }
    valueToX_(value) {
        const min = this.sliderProps_.get('min');
        const max = this.sliderProps_.get('max');
        return constrainRange(mapRange(value, min, max, 0, 1), 0, 1) * 100;
    }
    update_() {
        const v = this.value_.rawValue;
        if (v.length === 0) {
            this.element.classList.add(className$1(undefined, 'zero'));
        }
        else {
            this.element.classList.remove(className$1(undefined, 'zero'));
        }
        const xs = [this.valueToX_(v.min), this.valueToX_(v.max)];
        this.barElement.style.left = `${xs[0]}%`;
        this.barElement.style.right = `${100 - xs[1]}%`;
        this.knobElements.forEach((elem, index) => {
            elem.style.left = `${xs[index]}%`;
        });
    }
    onSliderPropsChange_() {
        this.update_();
    }
    onValueChange_() {
        this.update_();
    }
}

class RangeSliderController {
    constructor(doc, config) {
        this.grabbing_ = null;
        this.grabOffset_ = 0;
        this.onPointerDown_ = this.onPointerDown_.bind(this);
        this.onPointerMove_ = this.onPointerMove_.bind(this);
        this.onPointerUp_ = this.onPointerUp_.bind(this);
        this.sliderProps = config.sliderProps;
        this.viewProps = config.viewProps;
        this.value = config.value;
        this.view = new RangeSliderView(doc, {
            sliderProps: this.sliderProps,
            value: this.value,
            viewProps: config.viewProps,
        });
        const ptHandler = new PointerHandler(this.view.trackElement);
        ptHandler.emitter.on('down', this.onPointerDown_);
        ptHandler.emitter.on('move', this.onPointerMove_);
        ptHandler.emitter.on('up', this.onPointerUp_);
    }
    ofs_() {
        if (this.grabbing_ === 'min') {
            return this.view.knobElements[0].getBoundingClientRect().width / 2;
        }
        if (this.grabbing_ === 'max') {
            return -this.view.knobElements[1].getBoundingClientRect().width / 2;
        }
        return 0;
    }
    valueFromData_(data) {
        if (!data.point) {
            return null;
        }
        const p = (data.point.x + this.ofs_()) / data.bounds.width;
        const min = this.sliderProps.get('min');
        const max = this.sliderProps.get('max');
        return mapRange(p, 0, 1, min, max);
    }
    onPointerDown_(ev) {
        if (!ev.data.point) {
            return;
        }
        const p = ev.data.point.x / ev.data.bounds.width;
        const v = this.value.rawValue;
        const min = this.sliderProps.get('min');
        const max = this.sliderProps.get('max');
        const pmin = mapRange(v.min, min, max, 0, 1);
        const pmax = mapRange(v.max, min, max, 0, 1);
        if (Math.abs(pmax - p) <= 0.025) {
            this.grabbing_ = 'max';
        }
        else if (Math.abs(pmin - p) <= 0.025) {
            this.grabbing_ = 'min';
        }
        else if (p >= pmin && p <= pmax) {
            this.grabbing_ = 'length';
            this.grabOffset_ = mapRange(p - pmin, 0, 1, 0, max - min);
        }
        else if (p < pmin) {
            this.grabbing_ = 'min';
            this.onPointerMove_(ev);
        }
        else if (p > pmax) {
            this.grabbing_ = 'max';
            this.onPointerMove_(ev);
        }
    }
    applyPointToValue_(data, opts) {
        const v = this.valueFromData_(data);
        if (v === null) {
            return;
        }
        const rmin = this.sliderProps.get('min');
        const rmax = this.sliderProps.get('max');
        if (this.grabbing_ === 'min') {
            this.value.setRawValue(new Interval(v, this.value.rawValue.max), opts);
        }
        else if (this.grabbing_ === 'max') {
            this.value.setRawValue(new Interval(this.value.rawValue.min, v), opts);
        }
        else if (this.grabbing_ === 'length') {
            const len = this.value.rawValue.length;
            let min = v - this.grabOffset_;
            let max = min + len;
            if (min < rmin) {
                min = rmin;
                max = rmin + len;
            }
            else if (max > rmax) {
                min = rmax - len;
                max = rmax;
            }
            this.value.setRawValue(new Interval(min, max), opts);
        }
    }
    onPointerMove_(ev) {
        this.applyPointToValue_(ev.data, {
            forceEmit: false,
            last: false,
        });
    }
    onPointerUp_(ev) {
        this.applyPointToValue_(ev.data, {
            forceEmit: true,
            last: true,
        });
        this.grabbing_ = null;
    }
}

class RangeSliderTextController {
    constructor(doc, config) {
        this.value = config.value;
        this.viewProps = config.viewProps;
        this.sc_ = new RangeSliderController(doc, config);
        const axis = {
            constraint: config.constraint,
            textProps: config.textProps,
        };
        this.tc_ = new PointNdTextController(doc, {
            assembly: IntervalAssembly,
            axes: [axis, axis],
            parser: config.parser,
            value: this.value,
            viewProps: config.viewProps,
        });
        this.view = new RangeSliderTextView(doc, {
            sliderView: this.sc_.view,
            textView: this.tc_.view,
        });
    }
    get textController() {
        return this.tc_;
    }
}

function intervalFromUnknown(value) {
    return Interval.isObject(value)
        ? new Interval(value.min, value.max)
        : new Interval(0, 0);
}
function writeInterval(target, value) {
    target.writeProperty('max', value.max);
    target.writeProperty('min', value.min);
}

function createConstraint(params) {
    const constraints = [];
    const rc = createRangeConstraint(params);
    if (rc) {
        constraints.push(rc);
    }
    const sc = createStepConstraint(params);
    if (sc) {
        constraints.push(sc);
    }
    return new IntervalConstraint(new CompositeConstraint(constraints));
}
const IntervalInputPlugin = createPlugin({
    id: 'input-interval',
    type: 'input',
    accept: (exValue, params) => {
        if (!Interval.isObject(exValue)) {
            return null;
        }
        const result = parseRecord(params, (p) => (Object.assign(Object.assign({}, createNumberTextInputParamsParser(p)), { readonly: p.optional.constant(false) })));
        return result
            ? {
                initialValue: new Interval(exValue.min, exValue.max),
                params: result,
            }
            : null;
    },
    binding: {
        reader: (_args) => intervalFromUnknown,
        constraint: (args) => createConstraint(args.params),
        equals: Interval.equals,
        writer: (_args) => writeInterval,
    },
    controller(args) {
        const v = args.value;
        const c = args.constraint;
        if (!(c instanceof IntervalConstraint)) {
            throw TpError.shouldNeverHappen();
        }
        const midValue = (v.rawValue.min + v.rawValue.max) / 2;
        const textProps = ValueMap.fromObject(createNumberTextPropsObject(args.params, midValue));
        const drc = c.edge && findConstraint(c.edge, DefiniteRangeConstraint);
        if (drc) {
            return new RangeSliderTextController(args.document, {
                constraint: c.edge,
                parser: parseNumber,
                sliderProps: new ValueMap({
                    keyScale: textProps.value('keyScale'),
                    max: drc.values.value('max'),
                    min: drc.values.value('min'),
                }),
                textProps: textProps,
                value: v,
                viewProps: args.viewProps,
            });
        }
        const axis = {
            constraint: c.edge,
            textProps: textProps,
        };
        return new PointNdTextController(args.document, {
            assembly: IntervalAssembly,
            axes: [axis, axis],
            parser: parseNumber,
            value: v,
            viewProps: args.viewProps,
        });
    },
});

class RadioCellApi {
    constructor(controller) {
        this.controller_ = controller;
    }
    get disabled() {
        return this.controller_.viewProps.get('disabled');
    }
    set disabled(disabled) {
        this.controller_.viewProps.set('disabled', disabled);
    }
    get title() {
        var _a;
        return (_a = this.controller_.props.get('title')) !== null && _a !== void 0 ? _a : '';
    }
    set title(title) {
        this.controller_.props.set('title', title);
    }
}

class TpRadioGridChangeEvent extends TpChangeEvent {
    constructor(target, cell, index, value, last) {
        super(target, value, last);
        this.cell = cell;
        this.index = index;
    }
}

class RadioGridApi extends BladeApi {
    constructor(controller) {
        super(controller);
        this.cellToApiMap_ = new Map();
        const gc = this.controller.valueController;
        gc.cellControllers.forEach((cc) => {
            const api = new RadioCellApi(cc);
            this.cellToApiMap_.set(cc, api);
        });
    }
    get value() {
        return this.controller.value;
    }
    cell(x, y) {
        const gc = this.controller.valueController;
        const cc = gc.cellControllers[y * gc.size[0] + x];
        return this.cellToApiMap_.get(cc);
    }
    on(eventName, handler) {
        const bh = handler.bind(this);
        this.controller.value.emitter.on(eventName, (ev) => {
            const gc = this.controller.valueController;
            const cc = gc.findCellByValue(ev.rawValue);
            if (!cc) {
                return;
            }
            const capi = this.cellToApiMap_.get(cc);
            if (!capi) {
                return;
            }
            const i = gc.cellControllers.indexOf(cc);
            bh(new TpRadioGridChangeEvent(this, capi, [i % gc.size[0], Math.floor(i / gc.size[0])], ev.rawValue));
        });
    }
}

const className = ClassName('rad');
class RadioView {
    constructor(doc, config) {
        this.element = doc.createElement('div');
        this.element.classList.add(className());
        config.viewProps.bindClassModifiers(this.element);
        const labelElem = doc.createElement('label');
        labelElem.classList.add(className('l'));
        this.element.appendChild(labelElem);
        const inputElem = doc.createElement('input');
        inputElem.classList.add(className('i'));
        inputElem.name = config.name;
        inputElem.type = 'radio';
        config.viewProps.bindDisabled(inputElem);
        labelElem.appendChild(inputElem);
        this.inputElement = inputElem;
        const bodyElem = doc.createElement('div');
        bodyElem.classList.add(className('b'));
        labelElem.appendChild(bodyElem);
        const titleElem = doc.createElement('div');
        titleElem.classList.add(className('t'));
        bodyElem.appendChild(titleElem);
        bindValueMap(config.props, 'title', (title) => {
            titleElem.textContent = title;
        });
    }
}

class RadioController {
    constructor(doc, config) {
        this.props = config.props;
        this.viewProps = config.viewProps;
        this.view = new RadioView(doc, {
            name: config.name,
            props: this.props,
            viewProps: this.viewProps,
        });
    }
}

class RadioGridController {
    constructor(doc, config) {
        this.cellCs_ = [];
        this.cellValues_ = [];
        this.onCellInputChange_ = this.onCellInputChange_.bind(this);
        this.size = config.size;
        const [w, h] = this.size;
        for (let y = 0; y < h; y++) {
            for (let x = 0; x < w; x++) {
                const bc = new RadioController(doc, {
                    name: config.groupName,
                    props: ValueMap.fromObject(Object.assign({}, config.cellConfig(x, y))),
                    viewProps: ViewProps.create(),
                });
                this.cellCs_.push(bc);
                this.cellValues_.push(config.cellConfig(x, y).value);
            }
        }
        this.value = config.value;
        bindValue(this.value, (value) => {
            const cc = this.findCellByValue(value);
            if (!cc) {
                return;
            }
            cc.view.inputElement.checked = true;
        });
        this.viewProps = ViewProps.create();
        this.view = new PlainView(doc, {
            viewProps: this.viewProps,
            viewName: 'radgrid',
        });
        this.view.element.style.gridTemplateColumns = `repeat(${w}, 1fr)`;
        this.cellCs_.forEach((bc) => {
            bc.view.inputElement.addEventListener('change', this.onCellInputChange_);
            this.view.element.appendChild(bc.view.element);
        });
    }
    get cellControllers() {
        return this.cellCs_;
    }
    findCellByValue(value) {
        const index = this.cellValues_.findIndex((v) => v === value);
        if (index < 0) {
            return null;
        }
        return this.cellCs_[index];
    }
    onCellInputChange_(ev) {
        const inputElem = ev.currentTarget;
        const index = this.cellCs_.findIndex((c) => c.view.inputElement === inputElem);
        if (index < 0) {
            return;
        }
        this.value.rawValue = this.cellValues_[index];
    }
}

const RadioGridBladePlugin = (function () {
    return createPlugin({
        id: 'radiogrid',
        type: 'blade',
        accept(params) {
            const result = parseRecord(params, (p) => ({
                cells: p.required.function,
                groupName: p.required.string,
                size: p.required.array(p.required.number),
                value: p.required.raw,
                view: p.required.constant('radiogrid'),
                label: p.optional.string,
            }));
            return result ? { params: result } : null;
        },
        controller(args) {
            const value = createValue(args.params.value);
            return new LabeledValueBladeController(args.document, {
                blade: args.blade,
                props: ValueMap.fromObject({
                    label: args.params.label,
                }),
                value: value,
                valueController: new RadioGridController(args.document, {
                    groupName: args.params.groupName,
                    cellConfig: args.params.cells,
                    size: args.params.size,
                    value: value,
                }),
            });
        },
        api(args) {
            if (!(args.controller instanceof LabeledValueBladeController)) {
                return null;
            }
            if (!(args.controller.valueController instanceof RadioGridController)) {
                return null;
            }
            return new RadioGridApi(args.controller);
        },
    });
})();

function createRadioGridInputPlugin(config) {
    return createPlugin({
        id: 'input-radiogrid',
        type: 'input',
        accept(value, params) {
            if (!config.isType(value)) {
                return null;
            }
            const result = parseRecord(params, (p) => ({
                cells: p.required.function,
                groupName: p.required.string,
                size: p.required.array(p.required.number),
                view: p.required.constant('radiogrid'),
            }));
            return result
                ? {
                    initialValue: value,
                    params: result,
                }
                : null;
        },
        binding: config.binding,
        controller: (args) => {
            return new RadioGridController(args.document, {
                cellConfig: args.params.cells,
                groupName: args.params.groupName,
                size: args.params.size,
                value: args.value,
            });
        },
    });
}
const RadioGruidNumberInputPlugin = createRadioGridInputPlugin({
    isType: (value) => {
        return typeof value === 'number';
    },
    binding: {
        reader: (_args) => numberFromUnknown,
        writer: (_args) => writePrimitive,
    },
});
const RadioGruidStringInputPlugin = createRadioGridInputPlugin({
    isType: (value) => {
        return typeof value === 'string';
    },
    binding: {
        reader: (_args) => stringFromUnknown,
        writer: (_args) => writePrimitive,
    },
});
const RadioGruidBooleanInputPlugin = createRadioGridInputPlugin({
    isType: (value) => {
        return typeof value === 'boolean';
    },
    binding: {
        reader: (_args) => boolFromUnknown,
        writer: (_args) => writePrimitive,
    },
});

const id = 'essentials';
const css = '.tp-cbzgv,.tp-radv_b,.tp-rslv_k,.tp-cbzv_b{-webkit-appearance:none;-moz-appearance:none;appearance:none;background-color:rgba(0,0,0,0);border-width:0;font-family:inherit;font-size:inherit;font-weight:inherit;margin:0;outline:none;padding:0}.tp-radv_b,.tp-rslv_k,.tp-cbzv_b{background-color:var(--btn-bg);border-radius:var(--bld-br);color:var(--btn-fg);cursor:pointer;display:block;font-weight:bold;height:var(--cnt-usz);line-height:var(--cnt-usz);overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.tp-radv_b:hover,.tp-rslv_k:hover,.tp-cbzv_b:hover{background-color:var(--btn-bg-h)}.tp-radv_b:focus,.tp-rslv_k:focus,.tp-cbzv_b:focus{background-color:var(--btn-bg-f)}.tp-radv_b:active,.tp-rslv_k:active,.tp-cbzv_b:active{background-color:var(--btn-bg-a)}.tp-radv_b:disabled,.tp-rslv_k:disabled,.tp-cbzv_b:disabled{opacity:.5}.tp-cbzgv{background-color:var(--in-bg);border-radius:var(--bld-br);box-sizing:border-box;color:var(--in-fg);font-family:inherit;height:var(--cnt-usz);line-height:var(--cnt-usz);min-width:0;width:100%}.tp-cbzgv:hover{background-color:var(--in-bg-h)}.tp-cbzgv:focus{background-color:var(--in-bg-f)}.tp-cbzgv:active{background-color:var(--in-bg-a)}.tp-cbzgv:disabled{opacity:.5}.tp-btngridv{border-radius:var(--bld-br);display:grid;overflow:hidden;gap:2px}.tp-btngridv.tp-v-disabled{opacity:.5}.tp-btngridv .tp-btnv_b:disabled{opacity:1}.tp-btngridv .tp-btnv_b:disabled .tp-btnv_t{opacity:.5}.tp-btngridv .tp-btnv_b{border-radius:0}.tp-cbzv{position:relative}.tp-cbzv_h{display:flex}.tp-cbzv_b{margin-right:4px;position:relative;width:var(--cnt-usz)}.tp-cbzv_b svg{display:block;height:16px;left:50%;margin-left:-8px;margin-top:-8px;position:absolute;top:50%;width:16px}.tp-cbzv_b svg path{stroke:var(--bs-bg);stroke-width:2}.tp-cbzv_t{flex:1}.tp-cbzv_p{height:0;margin-top:0;opacity:0;overflow:hidden;transition:height .2s ease-in-out,opacity .2s linear,margin .2s ease-in-out}.tp-cbzv.tp-cbzv-expanded .tp-cbzv_p{margin-top:var(--cnt-usp);opacity:1}.tp-cbzv.tp-cbzv-cpl .tp-cbzv_p{overflow:visible}.tp-cbzv .tp-popv{left:calc(-1 * var(--cnt-hp));position:absolute;right:calc(-1 * var(--cnt-hp));top:var(--cnt-usz)}.tp-cbzpv_t{margin-top:var(--cnt-usp)}.tp-cbzgv{height:auto;overflow:hidden;position:relative}.tp-cbzgv.tp-v-disabled{opacity:.5}.tp-cbzgv_p{left:16px;position:absolute;right:16px;top:0}.tp-cbzgv_g{cursor:pointer;display:block;height:calc(var(--cnt-usz) * 5);width:100%}.tp-cbzgv_u{opacity:.1;stroke:var(--in-fg);stroke-dasharray:1}.tp-cbzgv_l{fill:rgba(0,0,0,0);stroke:var(--in-fg)}.tp-cbzgv_v{opacity:.5;stroke:var(--in-fg);stroke-dasharray:1}.tp-cbzgv_h{border:var(--in-fg) solid 1px;border-radius:50%;box-sizing:border-box;height:4px;margin-left:-2px;margin-top:-2px;pointer-events:none;position:absolute;width:4px}.tp-cbzgv:focus .tp-cbzgv_h-sel{background-color:var(--in-fg);border-width:0}.tp-cbzprvv{cursor:pointer;height:4px;padding:4px 0;position:relative}.tp-cbzprvv_g{display:block;height:100%;overflow:visible;width:100%}.tp-cbzprvv_t{opacity:.5;stroke:var(--mo-fg)}.tp-cbzprvv_m{background-color:var(--mo-fg);border-radius:50%;height:4px;margin-left:-2px;margin-top:-2px;opacity:0;position:absolute;top:50%;transition:opacity .2s ease-out;width:4px}.tp-cbzprvv_m.tp-cbzprvv_m-a{opacity:1}.tp-fpsv{position:relative}.tp-fpsv_l{bottom:4px;color:var(--mo-fg);line-height:1;right:4px;pointer-events:none;position:absolute}.tp-fpsv_u{margin-left:.2em;opacity:.7}.tp-rslv{cursor:pointer;padding-left:8px;padding-right:8px}.tp-rslv.tp-v-disabled{opacity:.5}.tp-rslv_t{height:calc(var(--cnt-usz));position:relative}.tp-rslv_t::before{background-color:var(--in-bg);border-radius:1px;content:"";height:2px;margin-top:-1px;position:absolute;top:50%;left:-4px;right:-4px}.tp-rslv_b{bottom:0;top:0;position:absolute}.tp-rslv_b::before{background-color:var(--in-fg);content:"";height:2px;margin-top:-1px;position:absolute;top:50%;left:0;right:0}.tp-rslv_k{height:calc(var(--cnt-usz) - 8px);margin-top:calc((var(--cnt-usz) - 8px)/-2);position:absolute;top:50%;width:8px}.tp-rslv_k.tp-rslv_k-min{margin-left:-8px}.tp-rslv_k.tp-rslv_k-max{margin-left:0}.tp-rslv.tp-rslv-zero .tp-rslv_k.tp-rslv_k-min{border-bottom-right-radius:0;border-top-right-radius:0}.tp-rslv.tp-rslv-zero .tp-rslv_k.tp-rslv_k-max{border-bottom-left-radius:0;border-top-left-radius:0}.tp-rsltxtv{display:flex}.tp-rsltxtv_s{flex:1}.tp-rsltxtv_t{flex:1;margin-left:4px}.tp-radv_l{display:block;position:relative}.tp-radv_i{left:0;opacity:0;position:absolute;top:0}.tp-radv_b{opacity:.5}.tp-radv_i:hover+.tp-radv_b{background-color:var(--btn-bg-h)}.tp-radv_i:focus+.tp-radv_b{background-color:var(--btn-bg-f)}.tp-radv_i:active+.tp-radv_b{background-color:var(--btn-bg-a)}.tp-radv_i:checked+.tp-radv_b{opacity:1}.tp-radv_t{bottom:0;color:inherit;left:0;overflow:hidden;position:absolute;right:0;text-align:center;text-overflow:ellipsis;top:0}.tp-radv_i:disabled+.tp-radv_b>.tp-radv_t{opacity:.5}.tp-radgridv{border-radius:var(--bld-br);display:grid;overflow:hidden;gap:2px}.tp-radgridv.tp-v-disabled{opacity:.5}.tp-radgridv .tp-radv_b{border-radius:0}';
const plugins = [
    ButtonGridBladePlugin,
    CubicBezierBladePlugin,
    FpsGraphBladePlugin,
    IntervalInputPlugin,
    RadioGridBladePlugin,
    RadioGruidBooleanInputPlugin,
    RadioGruidNumberInputPlugin,
    RadioGruidStringInputPlugin,
];

export { ButtonCellApi, ButtonGridApi, ButtonGridController, CubicBezier, CubicBezierApi, CubicBezierAssembly, CubicBezierController, CubicBezierGraphController, CubicBezierGraphView, CubicBezierPickerController, CubicBezierPickerView, CubicBezierPreviewView, CubicBezierView, FpsGraphBladeApi, FpsGraphController, FpsView, Fpswatch, Interval, IntervalAssembly, IntervalConstraint, RadioCellApi, RadioController, RadioGridApi, RadioGridController, RadioView, RangeSliderController, RangeSliderTextController, RangeSliderTextView, RangeSliderView, TpRadioGridChangeEvent, css, id, plugins };
