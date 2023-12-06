using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Xml
{
    internal class XmlMappingValidator : IMappingValidator
    {
        private readonly IMappingDataSourceSchema schema;

        public XmlMappingValidator(IMappingDataSourceSchema schema)
        {
            this.schema = schema;
        }

        public Task<IMappingValidationResult> Validate(IMappingDataSourceRecord record, IMappingContext context, CancellationToken cancellationToken)
        {
            var success = true;
            var list = new List<IMappingValidationResultItem>();
            if (this.schema.Fields.Count() != record.Schema.Fields.Count())
            {
                success = false;
            }

            foreach (var field in this.schema.Fields)
            {
                var configurateField = this.schema.Fields.FirstOrDefault(f => StringComparer.InvariantCultureIgnoreCase.Equals(f.Name, field.Name));

                if (configurateField == null)
                {
                    success = false;
                    list.Add(new XmlMappingValidationResultItem($"Field '{field.Name}' is invalid"));
                    break;
                }

                if (configurateField.Name != field.Name)
                {
                    success = false;
                    list.Add(new XmlMappingValidationResultItem($"Field '{field.Name}' not compatible"));
                }

                if (!field.IsOptional)
                {
                    if (string.IsNullOrWhiteSpace(record.GetValue(field.Name)?.ToString())
                        || record.GetValue(field.Name)?.ToString().ToLower() == "null")
                    {
                        success = false;
                        list.Add(new XmlMappingValidationResultItem($"Field '{field.Name}' cannot be null"));
                    }

                    if (record.GetValue(field.Name)?.ToString() == "0")
                    {
                        success = false;
                        list.Add(new XmlMappingValidationResultItem($"Field '{field.Name}' is invalid"));
                    }
                }
            }

            return Task.FromResult<IMappingValidationResult>(new XmlMappingValidationResult(success, list));
        }
    }
}
