using FTWRK.Application.Account.Commands.AddUserToRole;
using FTWRK.Application.Account.Commands.ConfirmUserEmail;
using FTWRK.Application.Account.Commands.EditUserProfile;
using FTWRK.Application.Account.Commands.ExternalSignInUser;
using FTWRK.Application.Account.Commands.RefreshUserToken;
using FTWRK.Application.Account.Commands.RegisterUser;
using FTWRK.Application.Account.Commands.ResetPassword;
using FTWRK.Application.Account.Queries.ForgetPassword;
using FTWRK.Application.Account.Queries.GetPublicProfile;
using FTWRK.Application.Account.Queries.GetUserProfile;
using FTWRK.Application.Account.Queries.SendUserEmailConfirmation;
using FTWRK.Application.Account.Queries.SignInUser;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Models;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FTWRK.Web.Controllers
{
    public class AccountController : BaseController
    {
        private const string ClientProfilePath = "profile";
        private const string ClientResetPasswordPath = "reset-password";
        private readonly ClientURIData _clientURI;
        public AccountController(IOptions<ClientURIData> clientOptions)
        {
            _clientURI = clientOptions.Value;
        }

        [HttpPost]
        public async Task<HttpResponseResult<Token>> Register(CreateUserCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Token>(201, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Token>> AddUserToRole(AddUserToRoleCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Token>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Token>> SignIn(SignInUserCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Token>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Token>> RefreshToken(RefreshUserTokenCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Token>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Token>> ExternalLogin(ExternalSignInUserCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Token>(200, result);
        }

        [HttpGet]
        public async Task<HttpResponseResult<UserDetailsDto>> GetProfile([FromQuery] GetUserProfileQuery query)
        {
            var result = await Mediator.Send(query);

            return new HttpResponseResult<UserDetailsDto>(200, result);
        }

        [HttpGet]
        public async Task<HttpResponseResult<Unit>> SendConfirmationEmail([FromQuery]string email)
        {
            var index = Request.Path.Value.LastIndexOf("/");
            var path = Request.Path.Value.Substring(0, index + 1);
            var newPath = path + nameof(ConfirmEmail);

            var link = URIHelper.CreateLink(Request.Scheme, Request.Host.Host, (int)Request.Host.Port, newPath);

            var result = await Mediator.Send(new SendUserEmailConfirmationQuery
            {
                Email = email,
                Link = link
            });

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpGet]
        public async Task<RedirectResult> ConfirmEmail([FromQuery]ConfirmUserEmailCommand command)
        {
            await Mediator.Send(command);
            var redirect = URIHelper.CreateLink(_clientURI.Scheme, _clientURI.Host, _clientURI.Port, ClientProfilePath);

            return new HttpResponseResult<Unit>(200, Unit.Value).RedirectTo(redirect);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Token>> EditProfile([FromForm]EditUserProfileCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Token>(200, result);
        }

        [HttpGet]
        public async Task<HttpResponseResult<Unit>> ForgetPassword([FromQuery]string email)
        {
            var link = URIHelper.CreateLink(_clientURI.Scheme, _clientURI.Host, _clientURI.Port, ClientResetPasswordPath);

            var result = await Mediator.Send(new ForgetPasswordQuery
            {
                Email = email,
                Link = link
            });

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Unit>> ResetPassword(ResetPasswordCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpGet("{id}")]
        public async Task<HttpResponseResult<UserDto>> GetPublicProfile([FromRoute]Guid id)
        {
            var result = await Mediator.Send(new GetPublicProfileQuery
            {
                Id = id
            });

            return new HttpResponseResult<UserDto>(200, result);
        }
    }
}
