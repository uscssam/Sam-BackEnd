using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;
using SAM.Services;
using SAM.Services.Dto;
using SAM.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SAM.Service.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddCollectionMappers(), Assembly.GetExecutingAssembly());
            services.AddTransient<IService<MachineDto>, MachineService>();
            services.AddTransient<IService<UserDto>, UserService>();
            services.AddTransient<IService<UnitDto>, UnitService>();
            services.AddTransient<IService<OrderServiceDto>, OrderServiceService>();
            return services;
        }
    }
}
