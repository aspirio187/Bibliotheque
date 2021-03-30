using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public abstract class Model<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        protected static System.Globalization.CultureInfo EnglishCulture
        {
            get
            {
                return System.Globalization.CultureInfo.GetCultureInfo("EN-US");
            }
        }
    }

    public class Model<TKey, TModel, TPropertiesEnum> : Model<TKey>
        where TKey : IEquatable<TKey>
        where TModel : Model<TKey, TModel, TPropertiesEnum>
        where TPropertiesEnum : struct, IConvertible, IComparable
    {
        public delegate void ChangeMethod(ChangeResult result);

        public static event ChangeMethod _OnChange;

        public event ChangeMethod OnChange;

        protected void TriggerOnChange(ChangeResult result)
        {
            if (_OnChange is not null) _OnChange(result);
            if (OnChange is not null) OnChange(result);
        }

        public class ChangeResult
        {
            public TModel Model { get; private set; }

            public TPropertiesEnum Property { get; private set; }

            public bool Success { get; private set; }

            public string ErrorMessage { get; private set; }

            protected ChangeResult(TModel model, TPropertiesEnum properties, bool success, string errorMessage)
            {
                Model = model;
                Property = properties;
                Success = success;
                ErrorMessage = errorMessage;
            }
        }

        public class ChangeResult<TType> : ChangeResult
        {
            public TType InitialValue { get; private set; }

            public TType AffectedValue { get; private set; }

            private ChangeResult(TModel model, TPropertiesEnum property, bool success, string errorMessage, TType initialValue, TType affectedValue)
                : base(model, property, success, errorMessage)
            {
                InitialValue = initialValue;
                AffectedValue = affectedValue;
            }

            public static ChangeResult<TType> Succeded(TModel model, TPropertiesEnum property, TType initialValue, TType affectedValue, bool triggerOnChange = true)
            {
                model.DefineValidity(property, true);
                var result = new ChangeResult<TType>(model, property, true, null, initialValue, affectedValue);
                if (triggerOnChange) model.TriggerOnChange(result);
                return result;
            }

            public static ChangeResult<TType> Failed(TModel model, TPropertiesEnum property, string errorMessage, TType initialValue, TType affectedValue, bool triggerOnChange = true)
            {
                model.DefineValidity(property, false);
                var result = new ChangeResult<TType>(model, property, false, errorMessage, initialValue, affectedValue);
                if (triggerOnChange) model.TriggerOnChange(result);
                return result;
            }
        }

        private Dictionary<TPropertiesEnum, bool> m_Properties;

        public bool IsValid()
        {
            return m_Properties.Values.All(isValid => isValid);
        }

        public IEnumerable<TPropertiesEnum> ValidProperties => m_Properties
                                                                .Where(x => x.Value == true)
                                                                .Select(x => x.Key);

        public IEnumerable<TPropertiesEnum> InvalidProperties => m_Properties
                                                                    .Where(x => x.Value == false)
                                                                    .Select(x => x.Key);

        public bool IsValid(TPropertiesEnum property)
        {
            return m_Properties[property];
        }

        private bool DefineValidity(TPropertiesEnum property, bool isValid)
        {
            try
            {
                if (m_Properties.ContainsKey(property)) m_Properties[property] = isValid;
                else m_Properties.Add(property, isValid);
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error during validity definition : {e.Message}");
                return false;
            }
        }

        public Model()
        {
            m_Properties = new Dictionary<TPropertiesEnum, bool>();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"{{ ID : {Id} }}\n");
            foreach (var property in m_Properties)
            {
                stringBuilder.Append($"\t{{ Property : {property.Key} | Valid : {property.Value}\n");
            }
            return stringBuilder.ToString();
        }
    }
}

