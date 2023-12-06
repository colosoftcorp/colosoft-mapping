using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    internal class MappingDataSourceRecordApplier<TTarget> : IMappingDataSourceRecordApplier<TTarget>
    {
        private readonly IMappingConfiguration mappingConfiguration;

        public MappingDataSourceRecordApplier(IMappingConfiguration mappingConfiguration)
        {
            this.mappingConfiguration = mappingConfiguration;
        }

        public async Task Apply(IMappingDataSourceRecord record, TTarget target, IMappingContext context, CancellationToken cancellationToken)
        {
            foreach (var field in record.Schema.Fields)
            {
                var mappingField = this.mappingConfiguration
                    .Fields
                    .FirstOrDefault(f => StringComparer.InvariantCultureIgnoreCase.Equals(f.Name, field.Name));

                if (mappingField != null)
                {
                    await mappingField.SetValue(target, record.GetValue(field.Name), context);
                }
            }
        }
    }
}
