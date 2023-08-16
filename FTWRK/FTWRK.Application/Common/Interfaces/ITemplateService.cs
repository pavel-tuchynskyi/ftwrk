using FTWRK.Domain.Entities.EmailTemplates;

namespace FTWRK.Application.Common.Interfaces
{
    public interface ITemplateService
    {
        Task<EmailTemplate> GetEmailTemplateAsync(string key);
    }
}
