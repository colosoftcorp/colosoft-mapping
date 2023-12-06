using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Colosoft.Mapping
{
    public static class MappingExtensionsDependencyInjectionExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services, Assembly assembly)
        {
            var mapperType = typeof(Mapping.IMapper);
            var mapperGenericType = typeof(Mapping.IMapper<,>);

            foreach (var type in assembly.GetTypes()
                .Where(type => mapperType.IsAssignableFrom(type)))
            {
                foreach (var @interface in type.GetInterfaces().Where(f => f.IsGenericType))
                {
                    if (@interface.GetGenericTypeDefinition() != mapperGenericType)
                    {
                        continue;
                    }

                    services.AddTransient(@interface, type);
                    break;
                }
            }

            return services;
        }
    }
}
