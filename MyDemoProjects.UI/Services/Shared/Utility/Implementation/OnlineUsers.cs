using MyDemoProjects.Application.Shared.Models;
using MyDemoProjects.UI.Services.Shared.Utility.Interface;
using System.Collections.Concurrent;
namespace MyDemoProjects.UI.Services.Shared.Utility.Implementation
{
    public class OnlineUsers : IOnlineUsers
    {
        public ConcurrentDictionary<string, User> UsersByNameIdentifier { get; } = new ConcurrentDictionary<string, User>();

        public event Action?  OnChange;

        public void TryRemove(string nameIdentifier)
        {
            UsersByNameIdentifier.TryRemove(nameIdentifier, out var _);
        }

        public void AddOrUpdate(string nameIdentifier, User? userSettings)
        {
            UsersByNameIdentifier.AddOrUpdate(nameIdentifier, userSettings ?? new User(), (key, oldValue) => userSettings ?? new User());
           
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
