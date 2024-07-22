using Routify.Gateway.Models.Api;

namespace Routify.Gateway.Models.Data;

internal record AppData
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    private Dictionary<string, RouteData> Routes { get; set; } = [];
    private Dictionary<string, AppProviderData> Providers { get; set; } = [];
    private Dictionary<string, ApiKeyData> ApiKeys { get; set; } = [];
    private Dictionary<string, ConsumerData> Consumers { get; set; } = [];
    private Dictionary<string, string> ConsumerByAliases { get; set; } = [];

    public AppData(
        ApiAppOutput output)
    {
        Id = output.Id;
        Name = output.Name;
        Routes = output
            .Routes
            .ToDictionary(route => route.Path, route => new RouteData
            {
                Id = route.Id,
                Name = route.Name,
                Path = route.Path,
                Type = route.Type,
                Schema = route.Schema,
                IsLoadBalanceEnabled = route.IsLoadBalanceEnabled,
                IsFailoverEnabled = route.IsFailoverEnabled,
                Timeout = route.Timeout,
                Providers = route
                    .Providers
                    .Select(provider => new RouteProviderData
                    {
                        Id = provider.Id,
                        AppProviderId = provider.AppProviderId,
                        Index = provider.Index,
                        Model = provider.Model,
                        Attrs = provider.Attrs,
                        Weight = Math.Max(provider.Weight, 1),
                    })
                    .ToList(),
                CacheConfig = route.CacheConfig,
                CostLimitConfig = route.CostLimitConfig
            });

        Providers = output
            .Providers
            .ToDictionary(provider => provider.Id, provider => new AppProviderData
            {
                Id = provider.Id,
                Provider = provider.Provider,
                Alias = provider.Alias,
                Attrs = provider.Attrs
            });

        ApiKeys = output
            .ApiKeys
            .ToDictionary(apiKey => apiKey.Id, apiKey => new ApiKeyData
            {
                Id = apiKey.Id,
                Hash = apiKey.Hash,
                Salt = apiKey.Salt,
                Prefix = apiKey.Prefix,
                Algorithm = apiKey.Algorithm,
                ExpiresAt = apiKey.ExpiresAt,
                CostLimitConfig = apiKey.CostLimitConfig
            });

        Consumers = new Dictionary<string, ConsumerData>();
        ConsumerByAliases = new Dictionary<string, string>();
        foreach (var consumerOutput in output.Consumers)
        {
            Consumers[consumerOutput.Id] = new ConsumerData
            {
                Id = consumerOutput.Id,
                Alias = consumerOutput.Alias,
                CostLimitConfig = consumerOutput.CostLimitConfig
            };

            if (!string.IsNullOrEmpty(consumerOutput.Alias))
            {
                ConsumerByAliases[consumerOutput.Alias] = consumerOutput.Id;
            }
        }
    }

    public RouteData? GetRoute(
        string path)
    {
        Routes.TryGetValue(path, out var route);
        return route;
    }

    public AppProviderData? GetProviderById(
        string id)
    {
        Providers.TryGetValue(id, out var provider);
        return provider;
    }

    public ApiKeyData? GetApiKeyById(
        string id)
    {
        ApiKeys.TryGetValue(id, out var apiKey);
        return apiKey;
    }

    public ConsumerData? GetConsumer(
        string idOrAlias)
    {
        if (Consumers.TryGetValue(idOrAlias, out var consumer))
        {
            return consumer;
        }

        if (ConsumerByAliases.TryGetValue(idOrAlias, out var consumerId)
            && Consumers.TryGetValue(consumerId, out consumer))
        {
            return consumer;
        }

        return null;
    }
}