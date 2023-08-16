using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Infrastructure.Extensions;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog;

namespace FTWRK.Infrastructure.Identity
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<ApplicationUserRole> _roleManager;

        public UserManagerService(UserManager<ApplicationUser> userManager, IMapper mapper, ITokenService tokenService,
            RoleManager<ApplicationUserRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateUserAsync(UserCreateDto userDto)
        {
            var user = new ApplicationUser(userDto.UserName, userDto.Email, userDto.Country, userDto.Age);

            await _userManager.CreateAppUserAsync(user, userDto.Password);

            return true;
        }

        public async Task<bool> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindAppUserByEmailAsync(email);

            if (!await _roleManager.RoleExistsAsync(roleName) || user.Roles.Count() > 0)
            {
                Log.Error("Can't find this role: {name}", roleName);
                throw new NotFoundException("Can't find this role");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                Log.Error("Can't add user: {userId} to this role: {roleName}", user.Id, roleName);
                throw new ApplicationException("Can't add user to this role");
            }

            return true;
        }

        public async Task<IUser> GetUser(Guid id)
        {
            var user = await _userManager.FindAppUserByIdAsync(id) as IUser;

            return user;
        }

        public async Task<string> CreateEmailConfirmationToken(Guid id)
        {
            var user = await _userManager.FindAppUserByIdAsync(id);

            if(user.EmailConfirmed == true)
            {
                Log.Error("Email is already confirmed for user", user.Id);
                throw new BadRequestException("Email already confirmed");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return code;
        }

        public async Task<bool> ConfirmUserEmailAsync(ConfirmUserEmailDto confirmEmailDto)
        {
            var user = await _userManager.FindAppUserByEmailAsync(confirmEmailDto.Email);

            if (user.EmailConfirmed == true)
            {
                Log.Error("Email is already confirmed for user", user.Id);
                throw new BadRequestException("Email already confirmed");
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Code);

            if (!result.Succeeded)
            {
                Log.Error("Invalid confirmation code: {code}", confirmEmailDto.Code);
                throw new BadRequestException("Invalid confirmation code");
            }

            return true;
        }

        public async Task<Token> EditUser(IUser user)
        {
            var userToEdit = await _userManager.FindAppUserByIdAsync(user.Id);
            var roles = (await _userManager.GetRolesAsync(userToEdit)).ToList();

            if (userToEdit.Email != user.Email)
            {
                userToEdit.EmailConfirmed = false;
            }

            _mapper.Map(user, userToEdit);

            var tokens = _tokenService.CreateTokens(userToEdit, roles);
            userToEdit.RefreshToken = tokens.RefreshToken;

            await _userManager.UpdateAppUserAsync(userToEdit);

            return new Token
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken.RefreshToken
            };
        }

        public async Task<string> CreateForgetPasswordToken(string email)
        {
            var user = await _userManager.FindAppUserByEmailAsync(email);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            return code;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto passwordDto)
        {
            var user = await _userManager.FindAppUserByEmailAsync(passwordDto.Email);

            var result = await _userManager.ResetUserPasswordAsync(user, passwordDto.Code, passwordDto.Password);

            return result;
        }

        public async Task<bool> IsUserInRole(Guid userId, string role)
        {
            var user = await _userManager.FindAppUserByIdAsync(userId);

            var result = await _userManager.IsInRoleAsync(user, role);

            return result;
        }

        public async Task<PagedList<IUser>> GetUsers(QueryParameters parameters)
        {
            var users = await ((IMongoQueryable<ApplicationUser>)_userManager.Users
                .Filter(parameters.Filter))
                .ToPagedListAsync<ApplicationUser, IUser>(parameters.PageNumber, parameters.PageSize);

            return users;
        }
    }
}
