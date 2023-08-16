using FTWRK.Infrastructure.Idenity.Models;

namespace FTWRK.Infrastructure.Interfaces
{
    public interface IExternalAuth
    {
        Task<ExternalAuthUserResult> ValidateTokenAsync(string token);
    }
}
