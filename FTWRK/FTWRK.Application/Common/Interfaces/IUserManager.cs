using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Common;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<bool> CreateUserAsync(UserCreateDto userDto);
        Task<bool> AddUserToRole(string email, string roleName);
        Task<string> CreateEmailConfirmationToken(Guid id);
        Task<bool> ConfirmUserEmailAsync(ConfirmUserEmailDto confirmEmailDto);
        Task<IUser> GetUser(Guid id);
        Task<Token> EditUser(IUser user);
        Task<string> CreateForgetPasswordToken(string email);
        Task<bool> ResetPassword(ResetPasswordDto passwordDto);
        Task<bool> IsUserInRole(Guid userId, string role);
        Task<PagedList<IUser>> GetUsers(QueryParameters parameters);
    }
}
