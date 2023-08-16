using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Account.Commands.ConfirmUserEmail
{
    public class ConfirmUserEmailCommandHandler : IRequestHandler<ConfirmUserEmailCommand, Unit>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public ConfirmUserEmailCommandHandler(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var confirmEmailDto = _mapper.Map<ConfirmUserEmailDto>(request);

            await _userManager.ConfirmUserEmailAsync(confirmEmailDto);

            return Unit.Value;
        }
    }
}
