using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Mappings;
using FTWRK.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Account.Commands.EditUserProfile
{
    public class EditUserProfileCommand : IRequest<Token>, IMap<IUser>
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public IFormFile? BackgroundPicture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EditUserProfileCommand, IUser>()
                .ConstructUsing(x => new EditUserDto())
                .ForMember(x => x.ProfilePicture, x => x.Ignore())
                .ForMember(x => x.BackgroundPicture, x => x.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
