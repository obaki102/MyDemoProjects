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

    public async Task<ApplicationResponse<bool>> CreateUserAsync(ApplicationUser newUser)
    {
        var user = await _userManager.FindByEmailAsync(newUser.Email);
        if(user is not null)
        {

            return ApplicationResponse<bool>.Fail("User already exist");
        }

        var result =  await _userManager.CreateAsync(newUser);

        return result.Succeeded == true ? ApplicationResponse<bool>.Success(result.Succeeded) : ApplicationResponse<bool>.Fail(result.Errors.Select(s=> s.Description).ToList());

    }

    public Task<ApplicationResponse<bool>> LoginUserAsync()
    {
        throw new NotImplementedException();
    }
}
