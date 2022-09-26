﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Infastructure.Services.Identity
{
    public interface IIdentityService
    {
        Task<ApplicationResponse<bool>> LoginUserAsync(string email, string password);
        Task<ApplicationResponse<bool>> CreateUserAsync(ApplicationUser newUser, string password);
        Task<ApplicationResponse<bool>> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    }
}