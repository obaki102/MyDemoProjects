using MyDemoProjects.Application.Shared.Events;
using MyDemoProjects.Application.Shared.Models;
using MyDemoProjects.UI.Services.Shared.Utility.Interface;
using System.Collections.Concurrent;
namespace MyDemoProjects.UI.Services.Shared.Utility.Implementation
{
    public class OnlineUsers : IOnlineUsers
    {
        public ConcurrentDictionary<string, UserSettings> UsersByCircuitId { get; } = new ConcurrentDictionary<string, UserSettings>();

        public event Action?  OnChange;

        public void TryRemove(string circuitId)
        {
            UsersByCircuitId.TryRemove(circuitId, out var _);
            NotifyStateChanged();
        }

        public void AddOrUpdate(string circuitId, UserSettings? userSettings)
        {
            UsersByCircuitId.AddOrUpdate(circuitId, userSettings ?? new UserSettings(), (key, oldValue) => userSettings ?? new UserSettings());
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
