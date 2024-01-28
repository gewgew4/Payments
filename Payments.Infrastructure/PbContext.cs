using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;

namespace Payments.Infrastructure
{
    public class PbContext(DbContextOptions<PbContext> options) : DbContext(options)
    {
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<ApprovedAuthorization> ApprovedAuthorizations { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public void Migrate()
        {
            base.Database.Migrate();
        }
    }
}
