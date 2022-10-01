using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Application.Infastructure.Services.Identity;

public interface IIdentityService
{
    /// <summary>
    /// Verify credentials and return a token if user is authenticated.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<ApplicationResponse<TokenResponse>> LoginUserAsync(string email, string password);
    Task<ApplicationResponse<bool>> CreateUserAsync(ApplicationUser newUser, string password);
    Task<ApplicationResponse<bool>> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    Task<ApplicationResponse<ApplicationUser>> GetAllUsersAsync();
   
}
