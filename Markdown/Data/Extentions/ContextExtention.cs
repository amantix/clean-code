using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extentions
{
    public static class ContextExtention
    {
        public static IServiceCollection AddDataBase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            serviceCollection.AddDbContext<ApplicationContext>(o =>
            {
                o.UseNpgsql(connectionString);
            });
            return serviceCollection;
        }
    }
}
