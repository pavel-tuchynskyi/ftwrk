using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.RegisterUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Token>
    {
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly ISignInManager _signInManager;

        public CreateUserCommandHandler(IMapper mapper, IUserManager userManager, ISignInManager signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Token> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = _mapper.Map<UserCreateDto>(request);
            
            await _userManager.CreateUserAsync(userDto);

            var result = await _signInManager.SignInUserAsync(userDto.Email, userDto.Password);

            return result;
        }
    }
}
