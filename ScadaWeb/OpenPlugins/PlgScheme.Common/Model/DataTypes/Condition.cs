// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Xml;
using System.ComponentModel;
using Scada.Lang;
using System.Text.Json.Serialization;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// The base class defining the condition.
    /// <para>Базовый класс, определяющий условие.</para>
    /// </summary>
    [Serializable]
    public abstract class Condition : ISchemeViewAvailable, ICloneable
    {
        /// <summary>
        /// Наименование категории условия.
        /// </summary>
        protected const string ConditionCat = "Condition";

        /// <summary>
        /// Ссылка на представление схемы.
        /// </summary>
        [NonSerialized]
        protected SchemeView schemeView;


        /// <summary>
        /// Конструктор.
        /// </summary>
        public Condition()
        {
            CompareOperator1 = CompareOperators.LessThan;
            CompareArgument1 = 0.0;
            LogicalOperator = LogicalOperators.None;
            CompareOperator2 = CompareOperators.LessThan;
            CompareArgument2 = 0.0;
            SchemeView = null;
        }


        /// <summary>
        /// Получить или установить 1-й оператор сравнения.
        /// </summary>
        #region Attributes
        [DisplayName("Compare oper. 1"), Category(ConditionCat)]
        #endregion
        public CompareOperators CompareOperator1 { get; set; }

        /// <summary>
        /// Получить или установить аргумент для сравнения 1-м оператором.
        /// </summary>
        #region Attributes
        [DisplayName("Argument 1"), Category(ConditionCat)]
        [DefaultValue(0.0)]
        #endregion
        public double CompareArgument1 { get; set; }

        /// <summary>
        /// Получить или установить логический оператор, применяемый к результатам сравнения.
        /// </summary>
        #region Attributes
        [DisplayName("Logical oper."), Category(ConditionCat)]
        [DefaultValue(LogicalOperators.None)]
        #endregion
        public LogicalOperators LogicalOperator { get; set; }

        /// <summary>
        /// Получить или установить 2-й оператор сравнения.
        /// </summary>
        #region Attributes
        [DisplayName("Compare oper. 2"), Category(ConditionCat)]
        #endregion
        public CompareOperators CompareOperator2 { get; set; }

        /// <summary>
        /// Получить или установить аргумент для сравнения 2-м оператором.
        /// </summary>
        #region Attributes
        [DisplayName("Argument 2"), Category(ConditionCat)]
        [DefaultValue(0.0)]
        #endregion
        public double CompareArgument2 { get; set; }

        /// <summary>
        /// Получить или установить ссылку на представление схемы.
        /// </summary>
        [Browsable(false), JsonIgnore]
        public SchemeView SchemeView
        {
            get
            {
                return schemeView;
            }
            set
            {
                schemeView = value;
            }
        }


        /// <summary>
        /// Преобразовать оператор сравнения в строку.
        /// </summary>
        private static string OperToString(CompareOperators oper)
        {
            return oper switch
            {
                CompareOperators.Equal => " = ",
                CompareOperators.NotEqual => " <> ",
                CompareOperators.LessThan => " < ",
                CompareOperators.LessThanEqual => " <= ",
                CompareOperators.GreaterThan => " > ",
                CompareOperators.GreaterThanEqual => " >= ",
                _ => ""
            };
        }

        /// <summary>
        /// Преобразовать логический оператор в строку.
        /// </summary>
        private static string OperToString(LogicalOperators oper)
        {
            return oper switch
            {
                LogicalOperators.And => Locale.IsRussian ? " И " : " And ",
                LogicalOperators.Or => Locale.IsRussian ? " Или " : " Or ",
                _ => ""
            };
        }


        /// <summary>
        /// Загрузить условие из XML-узла.
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            CompareOperator1 = xmlNode.GetChildAsEnum<CompareOperators>("CompareOperator1");
            CompareArgument1 = xmlNode.GetChildAsDouble("CompareArgument1");
            CompareOperator2 = xmlNode.GetChildAsEnum<CompareOperators>("CompareOperator2");
            CompareArgument2 = xmlNode.GetChildAsDouble("CompareArgument2");
            LogicalOperator = xmlNode.GetChildAsEnum<LogicalOperators>("LogicalOperator");
        }

        /// <summary>
        /// Сохранить условие в XML-узле.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("CompareOperator1", CompareOperator1);
            xmlElem.AppendElem("CompareArgument1", CompareArgument1);
            xmlElem.AppendElem("CompareOperator2", CompareOperator2);
            xmlElem.AppendElem("CompareArgument2", CompareArgument2);
            xmlElem.AppendElem("LogicalOperator", LogicalOperator);
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        public virtual object Clone()
        {
            Condition clonedCondition = ScadaUtils.DeepClone(this);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }

        /// <summary>
        /// Получить строковое представление объекта.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(Locale.IsRussian ? "Знач." : "Value");
            sb.Append(OperToString(CompareOperator1));
            sb.Append(CompareArgument1);
            if (LogicalOperator != LogicalOperators.None)
            {
                sb.Append(OperToString(LogicalOperator));
                sb.Append(Locale.IsRussian ? "Знач." : "Value");
                sb.Append(OperToString(CompareOperator2));
                sb.Append(CompareArgument2);
            }
            return sb.ToString();
        }
    }
}
