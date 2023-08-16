namespace FTWRK.Application.Common.Interfaces
{
    public interface IUserContextService
    {
        Guid GetUserId();
        List<string> GetUserRoles();
    }
}
