using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Common;
using FTWRK.Domain.Entities.Images;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Account.Commands.EditUserProfile
{
    public class EditUserProfileCommandHandler : IRequestHandler<EditUserProfileCommand, Token>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public EditUserProfileCommandHandler(IUserManager userManager, IMapper mapper, IUserContextService userContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<Token> Handle(EditUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<EditUserDto>(request);
            user.Id = _userId;
            user.ProfilePicture = await ConvertPicture(request.ProfilePicture);
            user.BackgroundPicture = await ConvertPicture(request.BackgroundPicture);

            var result = await _userManager.EditUser(user);

            return result;
        }

        private async Task<ImageBlob> ConvertPicture(IFormFile picture)
        {
            if(picture == null)
            {
                return null;
            }

            var imageBytes = await FileHelper.SerializeImageAsync(picture);
            return new ImageBlob(picture.ContentType, imageBytes);
        }
    }
}
