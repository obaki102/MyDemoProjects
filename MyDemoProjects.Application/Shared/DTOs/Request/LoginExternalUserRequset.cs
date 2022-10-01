using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.DTOs.Request;
public  class LoginExternalUserRequset
{
    public string Provider { get; init; } = string.Empty;
    public string EmailAddress { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
}
