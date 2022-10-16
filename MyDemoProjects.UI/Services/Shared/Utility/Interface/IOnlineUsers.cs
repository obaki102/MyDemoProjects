using MyDemoProjects.Application.Shared.Events;
using MyDemoProjects.Application.Shared.Models;
using System.Collections.Concurrent;

namespace MyDemoProjects.UI.Services.Shared.Utility.Interface
{
    public interface IOnlineUsers
    {
        public ConcurrentDictionary<string, UserSettings> UsersByNameIdentifier { get; }

        public event Action? OnChange;
        public void AddOrUpdate(string nameIdentifier, UserSettings? userSettings);
        public void TryRemove(string nameIdentifier);
    }
}
