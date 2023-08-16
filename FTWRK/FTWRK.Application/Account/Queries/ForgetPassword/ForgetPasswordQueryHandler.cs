using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Queries.ForgetPassword
{
    public class ForgetPasswordQueryHandler : IRequestHandler<ForgetPasswordQuery, Unit>
    {
        private readonly IUserManager _userManager;
        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;
        private const string ResetPasswordKey = "ResetPassword";

        public ForgetPasswordQueryHandler(IUserManager userManager, ITemplateService templateService, IEmailService emailService)
        {
            _userManager = userManager;
            _templateService = templateService;
            _emailService = emailService;
        }
        public async Task<Unit> Handle(ForgetPasswordQuery request, CancellationToken cancellationToken)
        {
            var code = await _userManager.CreateForgetPasswordToken(request.Email);

            var parameters = new Dictionary<string, string>()
            {
                { "email", request.Email },
                { "code", code }
            };

            var link = URIHelper.AddQueryParameters(request.Link, parameters);

            var template = await _templateService.GetEmailTemplateAsync(ResetPasswordKey);
            var message = new EmailMessage(request.Email, template.Subject, EmailMessageHelper.ReplacePlaceholders(template.Body, link));

            await _emailService.SendAsync(message);

            return Unit.Value;
        }
    }
}
