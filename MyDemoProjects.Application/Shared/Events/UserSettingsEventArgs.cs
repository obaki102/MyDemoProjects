using EllipticCurve.Utils;
using MyDemoProjects.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Events
{
    public class UserSettingsEventArgs : EventArgs
    {
        public User UserSettings { get; set; }
    }
}
