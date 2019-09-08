using Microsoft.Extensions.DependencyInjection;
using SmartHouseCore.Commands;
using SmartHouseCoreAbstraction.Commands;

namespace SmartHouseCore
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfireSmartHouseCoreServices(this IServiceCollection services)
        {
            services.AddTransient<ICommandInvoker, CommandInvoker>();
        }
    }
}