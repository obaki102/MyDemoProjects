using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.DTO.Response
{
    public record ApplicationResponse<T> 
    {
        public T Data { get; set; }

        public string Message { get; set; }

        public bool Status { get; set; }
    }
}


