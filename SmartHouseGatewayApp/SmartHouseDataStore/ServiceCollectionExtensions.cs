using Microsoft.Extensions.DependencyInjection;
using SmartHouseDataStore.Service;
using SmartHouseDataStoreAbstraction.Commands;

namespace SmartHouseDataStore
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfireSmartHouseDataStoreServices(this IServiceCollection services)
        {
            services.AddTransient<ICommandService, CommandService>();
        }
    }
}
