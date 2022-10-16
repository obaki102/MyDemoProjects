using MyDemoProjects.Application.Shared.Models;
using MyDemoProjects.UI.Services.Shared.Utility.Interface;
using System.Collections.Concurrent;
namespace MyDemoProjects.UI.Services.Shared.Utility.Implementation
{
    public class OnlineUsers : IOnlineUsers
    {
        public ConcurrentDictionary<string, UserSettings> UsersByNameIdentifier { get; } = new ConcurrentDictionary<string, UserSettings>();

        public event Action?  OnChange;

        public void TryRemove(string nameIdentifier)
        {
            UsersByNameIdentifier.TryRemove(nameIdentifier, out var _);
            NotifyStateChanged();
        }

        public void AddOrUpdate(string nameIdentifier, UserSettings? userSettings)
        {
            UsersByNameIdentifier.AddOrUpdate(nameIdentifier, userSettings ?? new UserSettings(), (key, oldValue) => userSettings ?? new UserSettings());
           
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
