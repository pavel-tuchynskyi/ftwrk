using FTWRK.Application.Common.Exceptions;
using FTWRK.Infrastructure.Idenity.Models;
using Microsoft.AspNetCore.Identity;

namespace FTWRK.Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> CreateAppUserAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string? password = null)
        {
            IdentityResult result;

            if (password == null)
            {
                result = await userManager.CreateAsync(user);
            }
            else
            {
                result = await userManager.CreateAsync(user, password);
            }

            if (!result.Succeeded)
            {
                throw new ApplicationException("Can't create this user");
            }

            return true;
        }

        public static async Task<ApplicationUser> GetOrCreateUser(this UserManager<ApplicationUser> userManager, string email, string userName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser(userName.ToLower(), email);

                await userManager.CreateAppUserAsync(user);
            }

            return user;
        }

        public static async Task<ApplicationUser> FindAppUserByEmailAsync(this UserManager<ApplicationUser> userManager, string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException("Can't find this user");
            }

            return user;
        }

        public static async Task<bool> ResetUserPasswordAsync(this UserManager<ApplicationUser> userManager, 
            ApplicationUser user, string code, string password)
        {
            var result = await userManager.ResetPasswordAsync(user, code, password);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to change user password");
            }

            return true;
        }

        public static async Task<ApplicationUser> FindAppUserByIdAsync(this UserManager<ApplicationUser> userManager, Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("Can't find this user");
            }

            return user;
        }

        public static async Task<bool> UpdateAppUserAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new ApplicationException("Error while trying to update user");
            }

            return true;
        }
    }
}
