using FTWRK.Application.Recommendations.Queries.GetUserRecommendations;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    [Authorize]
    public class RecommendationsController : BaseController
    {
        [HttpGet]
        public async Task<HttpResponseResult<List<Album>>> GetUserRecommendations()
        {
            var result = await Mediator.Send(new GetUserRecommendationsQuery());

            return new HttpResponseResult<List<Album>>(200, result);
        }
    }
}
