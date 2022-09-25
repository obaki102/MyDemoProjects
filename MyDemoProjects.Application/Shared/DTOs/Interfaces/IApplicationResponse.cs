using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.DTOs.Interfaces
{
    public interface IApplicationResponse
    {
        List<string> Messages { get; set; }

        bool IsSuccess { get; set; }
    }

    public interface IApplicationResponse<out T> : IApplicationResponse
    {
        T? Data { get; }

    }
}
