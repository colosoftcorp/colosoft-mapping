namespace Colosoft.Mapping
{
    public interface IMappingConfigurationTemplateExporterProvider
    {
        IMappingConfigurationTemplateExporter Get(string name);
    }
}
