﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Shared.DTO.Response
{
    public class ServerResponse<T>
    {
        public T Data { get; set; }

        public string Message { get; set; }

        public bool Status { get; set; }
    }
}
