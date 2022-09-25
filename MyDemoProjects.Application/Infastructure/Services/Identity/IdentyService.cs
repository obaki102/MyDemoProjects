using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Infastructure.Services.Identity;

public class IdentyService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentyService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationResponse<bool>> CreateUserAsync(ApplicationUser newUser, string pasword)
    {
        var user = await _userManager.FindByEmailAsync(newUser.Email);
        if (user is not null)
        {

            return ApplicationResponse<bool>.Fail("User already exist");
        }

        var result = await _userManager.CreateAsync(newUser, pasword);

        return result.Succeeded == true ? ApplicationResponse<bool>.Success(result.Succeeded) : ApplicationResponse<bool>.Fail(result.Errors.Select(s => s.Description).ToList());

    }

    public async Task<ApplicationResponse<bool>> LoginUserAsync(string email, string password)
    {
        var user = _userManager.FindByEmailAsync(email);
        if (user.Result is null)
        {
            return ApplicationResponse<bool>.Fail("User not found.Please check your username and password.");

        }
        var result = await _userManager.CheckPasswordAsync(user.Result, password);

        if (result is false)
        {
            return ApplicationResponse<bool>.Fail("Invalid Credentials.");
        }

        return ApplicationResponse<bool>.Success();
    }
}
