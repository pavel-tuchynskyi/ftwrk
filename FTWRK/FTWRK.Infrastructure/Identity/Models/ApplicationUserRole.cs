using AspNetCore.Identity.MongoDbCore.Models;
using FTWRK.Application.Common.Interfaces;
using MongoDbGenericRepository.Attributes;

namespace FTWRK.Infrastructure.Idenity.Models
{
    [CollectionName("Roles")]
    public class ApplicationUserRole : MongoIdentityRole<Guid>, IRole
    {
    }
}
