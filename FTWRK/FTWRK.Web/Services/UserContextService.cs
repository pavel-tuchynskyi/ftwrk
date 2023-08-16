using FTWRK.Application.Common.Interfaces;
using System.ComponentModel;
using System.Security.Claims;

namespace FTWRK.Web.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            if (principal == null || principal.Identity == null ||
                !principal.Identity.IsAuthenticated)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var converter = TypeDescriptor.GetConverter(typeof(Guid));

            var id = (Guid)converter.ConvertFromInvariantString(userId);

            return id;
        }

        public List<string> GetUserRoles()
        {
            var principal = _contextAccessor.HttpContext.User;

            if (principal == null || principal.Identity == null ||
                !principal.Identity.IsAuthenticated)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var roles = new List<string>();

            foreach (var claim in principal.Claims)
            {
                if (claim.Type == ClaimTypes.Role)
                    roles.Add(claim.Value);
            }

            return roles;
        }
    }
}
