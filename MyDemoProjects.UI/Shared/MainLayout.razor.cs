using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyDemoProjects.UI.Pages.Login;

namespace MyDemoProjects.UI.Shared;

partial class MainLayout
{
    private MudTheme _theme = new();
    private bool _isDarkMode = true;
    [Inject] private IDialogService _dialogService { get; set; } = default!;
    [Inject] private NavigationManager _navigationManager { get; set; } = default!;
    private void OpenDialog()
    {
        _dialogService.Show<Login>("Login", new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, NoHeader = true });
    }

    bool _drawerOpen = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    void ToHomePage()
    {
        _navigationManager.NavigateTo("/");
    }

}