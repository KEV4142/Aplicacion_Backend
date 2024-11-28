using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Persistencia;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistencia(
        this IServiceCollection services,
        IConfiguration configuration
    ){
        services.AddDbContext<BackendContext>(opt => {
            /* opt.LogTo(Console.WriteLine, new [] {
                DbLoggerCategory.Database.Command.Name
            }, LogLevel.Information).EnableSensitiveDataLogging(); */

            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });


        return services;
    }

}
