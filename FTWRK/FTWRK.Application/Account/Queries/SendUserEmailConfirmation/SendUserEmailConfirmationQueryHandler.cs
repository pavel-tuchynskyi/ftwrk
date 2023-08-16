using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Queries.SendUserEmailConfirmation
{
    public class SendUserEmailConfirmationQueryHandler : IRequestHandler<SendUserEmailConfirmationQuery, Unit>
    {
        private readonly IUserManager _userManager;
        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;
        private readonly Guid _userId;
        private const string EmailConfirmationKey = "EmailConfirm";

        public SendUserEmailConfirmationQueryHandler(IUserManager userManager, IUserContextService userContext,
            ITemplateService templateService, IEmailService emailService)
        {
            _userManager = userManager;
            _templateService = templateService;
            _emailService = emailService;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(SendUserEmailConfirmationQuery request, CancellationToken cancellationToken)
        {
            var code = await _userManager.CreateEmailConfirmationToken(_userId);

            var parameters = new Dictionary<string, string>()
            {
                { "email", request.Email },
                { "code", code }
            };

            var link = URIHelper.AddQueryParameters(request.Link, parameters);

            var template = await _templateService.GetEmailTemplateAsync(EmailConfirmationKey);
            var message = new EmailMessage(request.Email, template.Subject, EmailMessageHelper.ReplacePlaceholders(template.Body, link));

            await _emailService.SendAsync(message);

            return Unit.Value;
        }
    }
}
