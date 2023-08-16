using FTWRK.Application.Account.Queries.SignInUser;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Infrastructure.Configuration.ExternalAuth;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Identity;
using FTWRK.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FTWRK.Tests.ServicesTests
{
    public class SignInManagerServiceTests
    {
        public Mock<IUserStore<ApplicationUser>> _mockUserStore;
        public Mock<UserManager<ApplicationUser>> _mockUserManager;
        public Mock<ITokenService> _mockTokenService;
        public Mock<IOptions<ExternalAuthConfiguration>> _mockConfig;
        public Mock<IHttpClientFactory> _mockHttpFactory;
        public ApplicationUser _user;

        public SignInManagerServiceTests()
        {
            _mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(_mockUserStore.Object, null, null, null, null, null, null, null, null);
            _mockTokenService = new Mock<ITokenService>();
            _mockConfig = new Mock<IOptions<ExternalAuthConfiguration>>();
            _mockHttpFactory = new Mock<IHttpClientFactory>();

            _user = new ApplicationUser
            {
                Age = 15,
                Email = "test@email.com",
                UserName = "TestUser",
                Country = "Germany",
                Id = Guid.NewGuid()
            };
        }

        [Fact]
        public async Task SignInUserAsync_WhenReceiveValidData_ReturnToken()
        {
            // Arrange
            var userDto = new UserSignInDto()
            {
                Email = "test@email.com",
                Password = "!Password123"
            };

            (string, UserRefreshToken) token = ("TEST123", new UserRefreshToken
            {
                RefreshToken = "TEST123",
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
            });

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "User" } as IList<string>);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(_user, It.IsAny<string>())).ReturnsAsync(true);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _mockTokenService.Setup(x => x.CreateTokens(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>())).Returns(token);

            var service = new SignInManagerService(_mockUserManager.Object, _mockTokenService.Object, _mockConfig.Object,
                _mockHttpFactory.Object);

            // Act
            var result = await service.SignInUserAsync(userDto.Email, userDto.Password);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SignInUserAsync_WhenReceiveInvalidData_ThrowsException()
        {
            // Arrange
            var userDto = new UserSignInDto()
            {
                Email = "InvalidEmail",
                Password = "!Password123"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(_user.Email)).ReturnsAsync(_user);

            var service = new SignInManagerService(_mockUserManager.Object, _mockTokenService.Object, _mockConfig.Object,
                _mockHttpFactory.Object);

            // Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.SignInUserAsync(userDto.Email, userDto.Password));

            //Assert
            Assert.IsType<NotFoundException>(ex);
        }
    }
}
