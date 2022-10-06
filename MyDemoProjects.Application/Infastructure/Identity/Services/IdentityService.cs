using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyDemoProjects.Application.Shared.DTOs.Request;

namespace MyDemoProjects.Application.Infastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;


    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
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

    public async Task<ApplicationResponse<TokenResponse>> LoginUserAsync(string email, string password)
    {
        await _semaphore.WaitAsync();
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return ApplicationResponse<TokenResponse>.Fail("Please check your username and password.");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (isPasswordValid is false)
            {
                return ApplicationResponse<TokenResponse>.Fail("Invalid Credentials.");
            }
            var identityCreatedFromUser = await GenerateClaimsIdentityFromUser(user);
            var token = new TokenResponse(CreateToken(identityCreatedFromUser));

            return ApplicationResponse<TokenResponse>.Success(token);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<ApplicationResponse<TokenResponse>> LoginExternalUserAsync(LoginExternalUserRequset externalUser)
    {
        await _semaphore.WaitAsync();
        try
        {
            var user = await _userManager.FindByEmailAsync(externalUser.EmailAddress);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    EmailConfirmed = true,
                    IsActive = true,
                    IsLive = true,
                    UserName = externalUser.EmailAddress,
                    Email = externalUser.EmailAddress,
                    Provider = externalUser.Provider,
                    DisplayName = externalUser.UserName,
                    ProfilePictureDataUrl = externalUser.PictureUrl
                };
                var isNewUserCreated = await _userManager.CreateAsync(user);
                if (isNewUserCreated.Succeeded is false)
                {
                    return ApplicationResponse<TokenResponse>.Fail("External login failed.");
                }
                //TODO:Implement roles
                //await _userManager.AddToRoleAsync(user, "");
                await _userManager.AddLoginAsync(user, new UserLoginInfo(externalUser.Provider, externalUser.EmailAddress, externalUser.AccessToken));
            }
            var identityCreatedFromUser = await GenerateClaimsIdentityFromUser(user);
            var token = new TokenResponse(CreateToken(identityCreatedFromUser));

            return ApplicationResponse<TokenResponse>.Success(token);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<ClaimsIdentity> GenerateClaimsIdentityFromUser(ApplicationUser user)
    {
        //TODO: Store key to users-secrets
        var claimsIdentity = new ClaimsIdentity(AppSecrets.Bearer);
        claimsIdentity.AddClaim(new(ClaimTypes.NameIdentifier, user.Id));
        claimsIdentity.AddClaim(new(ApplicationClaimTypes.Status, user.IsActive.ToString()));
        if (!string.IsNullOrEmpty(user.UserName))
        {
            claimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.Name, user.UserName)
            });
        }
        if (!string.IsNullOrEmpty(user.Email))
        {
            claimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.Email, user.Email)
            });
        }
        if (!string.IsNullOrEmpty(user.ProfilePictureDataUrl))
        {
            claimsIdentity.AddClaims(new[] {
                new Claim(ApplicationClaimTypes.ProfilePictureDataUrl, user.ProfilePictureDataUrl)
            });
        }
        if (!string.IsNullOrEmpty(user.DisplayName))
        {
            claimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            });
        }
        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            claimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            });
        }
        claimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(30).ToShortTimeString())  });
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                claimsIdentity.AddClaim(claim);
            }
            claimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.Role, roleName) });

        }

        return claimsIdentity;
    }

    private string CreateToken(ClaimsIdentity claimsIdentity)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8
               .GetBytes(_configuration.GetSection(AppSecrets.TokenKey).Value));
        var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredential);
        var generatedJwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return generatedJwtToken;
    }

    public ApplicationResponse<ClaimsPrincipal> ValidateTokenAndGetClaimsPrincipal(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection(AppSecrets.TokenKey).Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
            StringComparison.InvariantCultureIgnoreCase))
        {
            return ApplicationResponse<ClaimsPrincipal>.Fail("Invalid token");
        }

        return ApplicationResponse<ClaimsPrincipal>.Success(principal);
    }
}
