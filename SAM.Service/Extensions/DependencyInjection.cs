using Microsoft.Extensions.DependencyInjection;
using SAM.Entities;
using SAM.Services;
using SAM.Services.Interfaces;

namespace SAM.Service.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IService<Machine>, MachineService>();
            services.AddTransient<IService<User>, UserService>();
            services.AddTransient<IService<Unit>, UnitService>();
            services.AddTransient<IService<OrderService>, OrderServiceService>();
            return services;
        }
    }
}
