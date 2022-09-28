﻿using System.Security.Claims;
using IdentityModel;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Application.Infastructure.Services.Identity;

public class IdentyService : AuthenticationStateProvider,IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentyService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationResponse<bool>> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return ApplicationResponse<bool>.Fail("User not found.Please check your username and password.");
        }
        var isPasswordChanged = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (isPasswordChanged.Succeeded is false)
        {
            return ApplicationResponse<bool>.Fail(isPasswordChanged.Errors.Select(s => s.Description).ToList());
        }
        return ApplicationResponse<bool>.Success(isPasswordChanged.Succeeded);
    }

    public async Task<ApplicationResponse<bool>> CreateUserAsync(ApplicationUser newUser, string pasword)
    {
        var user = await _userManager.FindByEmailAsync(newUser.Email);
        if (user is not null)
        {
            return ApplicationResponse<bool>.Fail("User already exist");
        }
        var isNewUserCreated = await _userManager.CreateAsync(newUser, pasword);
        if (isNewUserCreated.Succeeded is false)
        {
            ApplicationResponse<bool>.Fail(isNewUserCreated.Errors.Select(s => s.Description).ToList());
        }
        return ApplicationResponse<bool>.Success(isNewUserCreated.Succeeded);

    }

    public async Task<ApplicationResponse<ApplicationUser>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        if (users is null)
        {
            return ApplicationResponse<ApplicationUser>.Success("No users found");
        }
        return ApplicationResponse<ApplicationUser>.Success(users);
    }

    public async Task<ApplicationResponse<bool>> LoginUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return ApplicationResponse<bool>.Fail("User not found.Please check your username and password.");
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (isPasswordValid is false)
        {
            return ApplicationResponse<bool>.Fail("Invalid Credentials.");
        }
        var identityCreatedFromUser = await GenerateClaimsIdentityFromUser(user);
        var pricipalCreatedFromIdentity = new ClaimsPrincipal(identityCreatedFromUser);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(pricipalCreatedFromIdentity)));
        return ApplicationResponse<bool>.Success();
    }

    public async Task<ClaimsIdentity> GenerateClaimsIdentityFromUser(ApplicationUser user)
    {
        //TODO: Store it to users-secrets
        var result = new ClaimsIdentity("JoshPogi");
        result.AddClaim(new(ClaimTypes.NameIdentifier, user.Id));
        result.AddClaim(new(ApplicationClaimTypes.Status, user.IsActive.ToString()));
        if (!string.IsNullOrEmpty(user.UserName))
        {
            result.AddClaims(new[] {
                new Claim(ClaimTypes.Name, user.UserName)
            });
        }
        if (!string.IsNullOrEmpty(user.Email))
        {
            result.AddClaims(new[] {
                new Claim(ClaimTypes.Email, user.Email)
            });
        }
        if (!string.IsNullOrEmpty(user.ProfilePictureDataUrl))
        {
            result.AddClaims(new[] {
                new Claim(ApplicationClaimTypes.ProfilePictureDataUrl, user.ProfilePictureDataUrl)
            });
        }
        if (!string.IsNullOrEmpty(user.DisplayName))
        {
            result.AddClaims(new[] {
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            });
        }
        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            result.AddClaims(new[] {
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            });
        }
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                result.AddClaim(claim);
            }
            result.AddClaims(new[] {
                new Claim(ClaimTypes.Role, roleName) });

        }
        return result;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}
