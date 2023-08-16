using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.EmailTemplates;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(EmailMessage message);
    }
}
