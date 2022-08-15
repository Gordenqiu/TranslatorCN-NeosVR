using Furion;
using Furion.DatabaseAccessor;
using Microsoft.Extensions.DependencyInjection;

namespace translator.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>(DbProvider.SqlServer, connectionMetadata: "Server=(local);Database=Translator;User=User;Password=Password;MultipleActiveResultSets=True;");
            });
        }
    }
}