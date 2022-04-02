// Data sample
// { "item1": 1.2, "item2": 3.4 }

log("Hello from JS. Topic: " + topic);

let data = JSON.parse(payload);
setValue(0, data.item1);
setValue(1, data.item2);
