using AspNetCore.Identity.MongoDbCore.Models;
using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Images;
using MongoDbGenericRepository.Attributes;

namespace FTWRK.Infrastructure.Idenity.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>, IUser, IMap<IUser>
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public ImageBlob ProfilePicture { get; set; }
        public ImageBlob BackgroundPicture { get; set; }
        public UserRefreshToken RefreshToken { get; set; }

        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public ApplicationUser(string userName, string email, string country, int age)
        {
            UserName = userName;
            Country = country;
            Age = age;
            Email = email;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IUser, ApplicationUser>()
                .ForMember(x => x.Roles, x => x.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            profile.CreateMap<ApplicationUser, UserDetailsDto>().ReverseMap();
        }
    }
}
