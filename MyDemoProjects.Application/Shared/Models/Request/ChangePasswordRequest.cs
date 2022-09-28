﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Models.Request
{
    public record ChangePasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
