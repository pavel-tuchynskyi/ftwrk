using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Common;
using FTWRK.Infrastructure.Idenity.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace FTWRK.Infrastructure.Identity
{
    public class RoleManagerService : IRoleManager
    {
        private readonly RoleManager<ApplicationUserRole> _roleManager;

        public RoleManagerService(RoleManager<ApplicationUserRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var isExist = await _roleManager.RoleExistsAsync(roleName);

            if (isExist)
            {
                Log.Error("This role allready exist: {name}", roleName);
                throw new NotFoundException("This role allready exist");
            }

            var result = await _roleManager.CreateAsync(new ApplicationUserRole() { Name = roleName });

            return result.Succeeded;
        }

        public async Task<IRole> GetRoleByName(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);

            if(role == null)
            {
                Log.Error("Can't find this role: {name}", name);
                throw new NotFoundException("Can't find this role");
            }

            return role;
        }
    }
}
