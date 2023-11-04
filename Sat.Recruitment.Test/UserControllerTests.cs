using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerTests
    {
        
        User user = new User();
        private readonly Mock<IUserService> userServiceMock = new Mock<IUserService>();

        [Fact]
        public void CreateUser_OkResult()
        {
            //Arrange
            userServiceMock.Setup(service => service.CreateUser(user))
            .Returns(new Result { IsSuccess = true });            
            var controller = new UsersController(userServiceMock.Object, null);

            //Act
            var result = controller.CreateUser(user);

            //Assert
            var okResult = (OkObjectResult)result;
            var resultData = (Result)okResult.Value;

            Assert.True(resultData.IsSuccess);
        }

        [Fact]
        public void CreateUser_BadRequestResult()
        {
            //Arrange
            userServiceMock.Setup(service => service.CreateUser(user))
            .Returns(new Result { IsSuccess = false });
            var controller = new UsersController(userServiceMock.Object, null);

            //Act
            var result = controller.CreateUser(user);

            //Assert
            var badRequestResult = (BadRequestObjectResult)result;
            var resultData = (Result)badRequestResult.Value;

            Assert.False(resultData.IsSuccess);
        }

    }
}
