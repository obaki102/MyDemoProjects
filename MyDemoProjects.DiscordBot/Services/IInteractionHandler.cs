﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.DiscordBot.Services
{
     public interface IInteractionHandler
    {
        Task InitializeAsync();
    }
}
