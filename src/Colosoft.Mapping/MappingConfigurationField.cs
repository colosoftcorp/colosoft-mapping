using System;
using System.Linq;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    internal class MappingConfigurationField<TTarget, TPropertyValue, TContext> : IMappingConfigurationField
        where TContext : IMappingContext
    {
        private readonly Func<TTarget, TPropertyValue, TContext, Task> setter;

        public MappingConfigurationField(string name, Func<TTarget, TPropertyValue, TContext, Task> setter)
        {
            this.setter = setter;
            this.Name = name;
        }

        public string Name { get; }

        public Task SetValue(object instance, object value, IMappingContext context)
        {
            if (!(instance is TTarget))
            {
                throw new ArgumentException($"O valor da instancia deve ser do tipo {typeof(TTarget).Name}", nameof(instance));
            }

            return this.InternalSetValue(instance, value, context);
        }

        internal async Task InternalSetValue(object instance, object value, IMappingContext context)
        {
            var type = typeof(TPropertyValue);

            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = type.GenericTypeArguments.FirstOrDefault();
            }

            object valueConverted = null;

            if (type != null && type.IsEnum && value != null)
            {
                var current = value.ToString();
                if (!string.IsNullOrWhiteSpace(current))
                {
                    valueConverted = (TPropertyValue)Enum.Parse(type, current);
                }
            }
            else if (type == typeof(string) && value != null)
            {
                valueConverted = value.ToString();
            }
            else if (type == typeof(int)
                && (value == null || (value is string textValueInt && (StringComparer.InvariantCultureIgnoreCase.Equals(textValueInt, "null") || string.IsNullOrWhiteSpace(textValueInt)))))
            {
                valueConverted = 0;
            }
            else if (type == typeof(decimal)
                && (value == null || (value is string textValueDecimal && (StringComparer.InvariantCultureIgnoreCase.Equals(textValueDecimal, "null") || string.IsNullOrWhiteSpace(textValueDecimal)))))
            {
                valueConverted = 0M;
            }
            else if (type == typeof(float)
                && (value == null || (value is string textValueFloat && (StringComparer.InvariantCultureIgnoreCase.Equals(textValueFloat, "null") || string.IsNullOrWhiteSpace(textValueFloat)))))
            {
                valueConverted = 0f;
            }
            else if (type == typeof(DateTime) && value is double)
            {
                valueConverted = DateTime.FromOADate((double)value);
            }
            else if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                valueConverted = (TPropertyValue)Convert.ChangeType(value, type);
            }

            await this.setter((TTarget)instance, (TPropertyValue)valueConverted, (TContext)context);
        }
    }
}
