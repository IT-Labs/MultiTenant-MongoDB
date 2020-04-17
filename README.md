# MultiTenant
Showcase for tenant resolution in .NET Core API, using HTTP Request Host to identify the tenant and a AWS Secrets Manager as tenant configuration storage

## Overview
These are the main parts in order to achieve multi-tenancy in .NET Core API:

#### 1. Tenant Identification
Multiple strategies can be used in order to identify which tenant to use when running the API.
In this showcase, the HTTP Request Host is used to identify tenants and switch between them.
 ```csharp
     services.TryAddSingleton<ITenantIdentificationStrategy, RequestHostTenantIdentificationStrategy>();
```
    
Other implementations can be added and used, like resolving tenants by HTTP header value or by JWT token claim.

#### 2. Tenant Storage
Multiple data stores can be used to isolate the tenant's configuration data.
In this showcase, the AWS Secrets Manager is used to store each tenant's data.
 ```csharp
    services.TryAddSingleton<ITenantStorage<Tenant>, AWSSecretsManagerTenantStorage>();
```

Additionally, another tenant storage implementation is done with an SQL database as tenant storage: 
 ```csharp 
    SQLDatabaseTenantStorage 
 ```

Other implementations can be added and used, like storing tenant's data in json files, an object storage or Azure App Configuration with Azure Key Vault.

#### 3. Caching
Tenant's data will be read on each API request and therefore some cache mechanism is needed in order to improve the API performance.
In this showcase, a concurrent dictionary is used to cache each tenant's data.
```csharp
       services.TryAddSingleton<ICache>(cache => new ConcurrentDictionaryCache(new ConcurrentDictionary<string, object>()));
```

Other implementations can be added and used, like caching tenant's data in Redis or Memcached.

## Configuration

- Setup each tenant with it's own SQL database
- Use **TasksDbContext** EF migrations to create the database schema
- If AWS Secrets Manager is used as tenant storage
    - setup AWS credentials using any approach from [here](https://aws.amazon.com/blogs/security/a-new-and-standardized-way-to-manage-credentials-in-the-aws-sdks/)
    - setup **AWSRegion** config in app.settings json
    - Organize your secrets by taging each tenant secret with the tenant identifier
    - Store the tenant's data (Id, ConnectionString)
- If SQL Database is used as tenant storage
    - Use **TenantsDbContext** EF migrations to create the database schema
    - Store the tenant's data (Id, ConnectionString)
    - setup **ConnectionStrings:MasterDatabase** config in app.settings json