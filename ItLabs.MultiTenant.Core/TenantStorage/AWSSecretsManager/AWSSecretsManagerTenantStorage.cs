using Amazon;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// AWS Secrets Manager storage for tenant data (managed AWS service)
    /// </summary>
    public class AWSSecretsManagerTenantStorage : ITenantStorage<Tenant>
    {
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;

        public AWSSecretsManagerTenantStorage(ICache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        /// <summary>
        /// Get the tenant data from AWS Secrets Manager by tenant identifier 
        /// Check cache before getting the data
        /// </summary>
        /// <param name="identifier">The tenant identifier</param>
        /// <returns>The Tenant</returns>
        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = _cache.GetOrSet(identifier, () => GetTenantFromAWSSecretsManager(secretTagKeyIdentifier: identifier));
            return await Task.FromResult(tenant);
        }


        /// <summary>
        /// Get tenant data by AWS secret tag (as tenant identifier)
        /// Get the key names only returned from AWS Secret Manager (by removing the secret name)
        /// AWS credentials needed. Use any approach given here: https://aws.amazon.com/blogs/security/a-new-and-standardized-way-to-manage-credentials-in-the-aws-sdks/
        /// Can be modified depending on the secrets organization at AWS Secrets Manager
        /// </summary>
        /// <param name="identifier">The tenant identifier</param>
        /// <returns>The Tenant</returns>
        private Tenant GetTenantFromAWSSecretsManager(string secretTagKeyIdentifier)
        {
            var awsSecrets = new ConfigurationBuilder().AddSecretsManager(
                region: RegionEndpoint.GetBySystemName(_configuration["AWSRegion"]),
                configurator: opts =>
                {
                    opts.SecretFilter = entry => entry.Name == secretTagKeyIdentifier;
                    opts.KeyGenerator = (entry, key) => key.Split(":").LastOrDefault();
                }).Build();


            var tenantId = awsSecrets.GetValue("Id", defaultValue: string.Empty);
            var connectionString = awsSecrets.GetValue("ConnectionString", defaultValue: string.Empty);
            var database = awsSecrets.GetValue("Database", defaultValue: string.Empty);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Tenant {secretTagKeyIdentifier} has no ConnectionString defined");
            }

            if (string.IsNullOrEmpty(database))
            {
                throw new Exception($"Tenant {secretTagKeyIdentifier} has no Database defined");
            }

            var tenant = new Tenant
            {
                Id = tenantId,
                Identifier = secretTagKeyIdentifier,
                ConnectionString = connectionString,
                Database = database
            };

            return tenant;
        }
    }
}
