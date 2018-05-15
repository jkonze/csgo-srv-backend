using Microsoft.EntityFrameworkCore;
using SrvBackend.Models;

namespace SrvBackend.Helpers
{
    public class CSGOContext : DbContext
    {
        public CSGOContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ServerCredentials> Servers { get; set; }
    }
}