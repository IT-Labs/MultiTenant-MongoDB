using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// Tenant storage interface
    /// </summary>
    public interface ITenantStorage<T> where T : Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}
