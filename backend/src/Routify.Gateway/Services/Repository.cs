using System.Collections.Concurrent;
using Routify.Gateway.Models.Api;
using Routify.Gateway.Models.Data;

namespace Routify.Gateway.Services;

internal class Repository
{
    private readonly ConcurrentDictionary<string, AppData> _apps = new();
    public bool IsLoaded { get; private set; }
    
    public void UpdateApps(
        List<ApiAppPayload> apps)
    {
        foreach (var app in apps)
        {
            _apps.AddOrUpdate(app.Id,
                new AppData(app),
                (_, appModel) => new AppData(app));
        }

        var appIds = new HashSet<string>(apps.Select(x => x.Id));
        var toRemoveIds = new HashSet<string>(_apps.Keys).Except(appIds);

        foreach (var id in toRemoveIds)
        {
            _apps.TryRemove(id, out _);
        }
        
        IsLoaded = true;
    }
    
    public AppData? GetApp(
        string appId)
    {
        _apps.TryGetValue(appId, out var app);
        return app;
    }
}
