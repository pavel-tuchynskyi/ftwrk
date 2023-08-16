using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Images;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Identity;
using FTWRK.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FTWRK.Tests.ServicesTests
{
    public class UserManagerServiceTests
    {
        public Mock<IUserStore<ApplicationUser>> _mockUserStore;
        public Mock<UserManager<ApplicationUser>> _mockUserManager;
        public Mock<RoleManager<ApplicationUserRole>> _mockRoleManager;
        public Mock<IMapper> _mockMapper;
        public Mock<IEmailService> _mockEmailService;
        public Mock<ITokenService> _mockTokenService;
        public Mock<IImageService> _mockImageService;
        public Mock<ITemplateService> _mockTemplateService;
        public ApplicationUser _user;

        public UserManagerServiceTests()
        {
            _mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(_mockUserStore.Object, null, null, null, null, null, null, null, null);
            _mockRoleManager = new Mock<RoleManager<ApplicationUserRole>>(Mock.Of<IRoleStore<ApplicationUserRole>>(), null, null, null, null);
            _mockMapper = new Mock<IMapper>();
            _mockEmailService = new Mock<IEmailService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockImageService = new Mock<IImageService>();
            _mockTemplateService = new Mock<ITemplateService>();
            _user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@email.com",
                NormalizedEmail = "test@email.com",
                UserName = "User",
                Age = 20,
                ProfilePicture = new ImageBlob("image/png", new byte[] { 1, 1, 1 })
            };
        }

        [Fact]
        public async Task CreateUserAsync_WhenReciveValidData_CreatesNewUser()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                Age = 20,
                Country = "Ukraine",
                Email = "test@mail.com",
                Password = "!Password123",
                UserName = "TestUser"
            };

            _mockUserStore.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUserRole { Name = "User" });
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var service = new UserManagerService(_mockUserManager.Object, _mockMapper.Object, _mockTokenService.Object, _mockRoleManager.Object);

            // Act
            var result = await service.CreateUserAsync(userDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditUser_WhenReceiveValidData_EditUserEntity()
        {
            // Arrange
            var userDto = new EditUserDto()
            {
                Age = 20,
                Email = "test@email.com",
                UserName = "TestUser2",
                Country = "Ukraine",
                Id = Guid.NewGuid()
            };

            (string, UserRefreshToken) token = ("TEST123", new UserRefreshToken
            {
                RefreshToken = "TEST123",
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
            });

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "User"} as IList<string>);
            _mockTokenService.Setup(x => x.CreateTokens(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>())).Returns(token);

            var service = new UserManagerService(_mockUserManager.Object, _mockMapper.Object, _mockTokenService.Object, _mockRoleManager.Object);

            // Act
            var result = await service.EditUser(userDto);

            // Assert
            Assert.NotNull(result);
        }
    }
}
