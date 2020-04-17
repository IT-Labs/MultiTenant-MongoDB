using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// Service to get the tenant data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TenantService<T> where T : Tenant
    {
        private readonly ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private readonly ITenantStorage<T> _tenantStorage;

        public TenantService(ITenantIdentificationStrategy tenantIdentificationStrategy, ITenantStorage<T> tenantStorage)
        {
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _tenantStorage = tenantStorage;
        }

        /// <summary>
        /// Get Tenant data 
        /// Use the tenant identification strategy to get the tenant identifier
        /// Use the tenant storage and tenant identifier to get the tenant data
        /// </summary>
        /// <returns>The Tenant</returns>
        public async Task<T> GetTenantAsync()
        {
            var identifier = await _tenantIdentificationStrategy.GetTenantIdentifierAsync();
            var tenant = await _tenantStorage.GetTenantAsync(identifier);

            return tenant;
        }
    }
}
