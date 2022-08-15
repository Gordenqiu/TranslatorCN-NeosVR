using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace translator.EntityFramework.Core
{
    [AppDbContext("TranslatorConnectionString", DbProvider.SqlServer)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}