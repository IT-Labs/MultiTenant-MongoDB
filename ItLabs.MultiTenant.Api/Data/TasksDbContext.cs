using ItLabs.MultiTenant.Core;
using Microsoft.EntityFrameworkCore;

namespace ItLabs.MultiTenant.Api
{
    /// <summary>
    /// The EF DBContext used for storing tasks per tenant
    /// </summary>
    public class TasksDbContext : DbContext
    {
        private readonly TenantService<Tenant> _tenantService;

        public TasksDbContext(DbContextOptions<TasksDbContext> options, TenantService<Tenant> tenantService)
          : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// On each DbContext request, the tenant is resolved and the tenant connection string is used to make the DB calls
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var tenant = _tenantService.GetTenantAsync().GetAwaiter().GetResult();

                optionsBuilder.UseSqlServer(tenant.ConnectionString);
            }
        }
    }
}
