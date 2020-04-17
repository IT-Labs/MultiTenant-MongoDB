using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// Tenant Identification/Resolution interface
    /// </summary>
    public interface ITenantIdentificationStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}
