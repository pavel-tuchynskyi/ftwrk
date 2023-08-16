using FTWRK.Application.Roles.Commands.CreateRole;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : BaseController
    {
        [HttpPost]
        public async Task<HttpResponseResult<Unit>> CreateRole(CreateRoleCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(201, result);
        }
    }
}
