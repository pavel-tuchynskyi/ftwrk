using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.DTO.Account
{
    public class EditUserDto : IUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public ImageBlob? ProfilePicture { get; set; }
        public ImageBlob? BackgroundPicture { get; set; }
        public List<Guid> Roles { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
