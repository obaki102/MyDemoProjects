namespace MyDemoProjects.UI.Services.Shared.Utility.Interface;

public interface IRetrieveAuthState
{
    Task<Dictionary<string, string>> GetClaimValues();
}
