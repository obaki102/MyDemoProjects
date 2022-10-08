using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using MyDemoProjects.Application.Infastructure.Identity.Models;

namespace MyDemoProjectsUnitTests.Services.Identity
{
    public class FakeRoleManager : RoleManager<ApplicationRole>
    {
        public FakeRoleManager()
            : base(Mock.Of<IRoleStore<ApplicationRole>>(),
                  new IRoleValidator<ApplicationRole>[0],
                  Mock.Of<ILookupNormalizer>(),
                  Mock.Of<IdentityErrorDescriber>(),
                  Mock.Of<ILogger<RoleManager<ApplicationRole>>>())
        {
        }
    }
}
