using Microsoft.Extensions.DependencyInjection;
using SAM.Entities;
using SAM.Repositories.Database.Context;
using SAM.Repositories.Interfaces;

namespace SAM.Repositories.Database.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseRepository(this IServiceCollection services)
        {
            services.AddDbContext<MySqlContext>();
            services.AddTransient<IRepositoryDatabase<Machine>, MachineRepository>();
            services.AddTransient<IRepositoryDatabase<OrderService>, OrderServiceRepository>();
            services.AddTransient<IRepositoryDatabase<Unit>, UnitRepository>();
            services.AddTransient<IRepositoryDatabase<User>, UserRepository>();
            return services;
        }
    }
}
