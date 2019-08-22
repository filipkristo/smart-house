using Microsoft.Extensions.DependencyInjection;

namespace SmartHouseCommon
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfireSmartHouseCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<AppSettings>();
        }
    }
}
