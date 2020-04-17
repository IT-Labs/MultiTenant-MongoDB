using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// In-Memory storage for tenant data, used for showcase only
    /// </summary>
    public class InMemoryTenantStorage : ITenantStorage<Tenant>
    {
        private readonly ICache _cache;

        public InMemoryTenantStorage(ICache cache)
        {
            _cache = cache;
            LoadCache();
        }

        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            //Add the tenant if it doesn't exists for showcase usage
            var tenant = _cache.GetOrSet(identifier, () =>
                new Tenant
                {
                    Id = $"Tenant{_cache.Count() + 1}",
                    Identifier = identifier,
                    ConnectionString = "",
                    Database = ""
                });

            return await Task.FromResult(tenant);
        }

        /// <summary>
        /// Load the cache with dummy values
        /// </summary>
        public void LoadCache()
        {
            _cache.GetOrSet("localhost:5000", () =>
                new Tenant
                {
                    Id = $"Tenant1",
                    Identifier = "localhost:5000",
                    ConnectionString = "",
                    Database = ""
                });

            _cache.GetOrSet("localhost:5001", () =>
               new Tenant
               {
                   Id = $"Tenant2",
                   Identifier = "localhost:5001",
                   ConnectionString = "",
                    Database = ""
               });

            _cache.GetOrSet("localhost:5002", () =>
               new Tenant
               {
                   Id = $"Tenant3",
                   Identifier = "localhost:5002",
                   ConnectionString = "",
                    Database = ""
               });
        }
    }
}
