using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Account.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public ResetPasswordCommandHandler(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetPasswordDto = _mapper.Map<ResetPasswordDto>(request);

            await _userManager.ResetPassword(resetPasswordDto);

            return Unit.Value;
        }
    }
}
