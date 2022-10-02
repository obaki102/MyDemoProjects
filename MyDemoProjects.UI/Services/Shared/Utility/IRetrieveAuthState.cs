namespace MyDemoProjects.UI.Services.Shared.Utility;

public interface IRetrieveAuthState
{ 
    Task<Dictionary<string, string>> GetClaimValues();
 }
