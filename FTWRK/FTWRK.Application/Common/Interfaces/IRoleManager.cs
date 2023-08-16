using FTWRK.Domain.Common;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IRoleManager
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<IRole> GetRoleByName(string name);
    }
}
