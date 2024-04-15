using SignalRAPI.SubscribeTableDependencies;

namespace SignalRAPI.MiddlewareExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder)
            where T : ISubscribeTableDependency
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<T>();
            service?.SubscribeTableDependency();
        }
    }
}
