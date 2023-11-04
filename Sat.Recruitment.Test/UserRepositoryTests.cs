using Microsoft.Extensions.Logging;
using Moq;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserRepositoryTests
    {
        private readonly Mock<ILogger<UserRepository>> loggerMock;
        private readonly Mock<StreamReader> streamReaderMock = new Mock<StreamReader>();

        public UserRepositoryTests()
        {
            loggerMock = new Mock<ILogger<UserRepository>>();
        }

        [Fact]
        public void GetUsersFromDatabase_WithData_ReturnsUsers()
        {
            // Arrange
            var repository = new UserRepository(loggerMock.Object);

            // Act
            var users = repository.GetUsersFromDatabase();

            // Assert
            Assert.NotNull(users);
        }

        [Fact]
        public void UserExists_UserExistsInDataBase_ReturnsTrue()
        {
            //Arrange
            var repository = new UserRepository(loggerMock.Object);
            var newUser = new User() { Address= "19 Street", Email= "existingemail@gmail.com", Money= 150, Name = "Cintia", Phone="123456789", UserType = UserType.Normal };

            var userList = new List<User>()
            { new User { Address = "19 Street", Email = "existingemail@gmail.com", Money = 150, Name = "Cintia", Phone = "123456789", UserType = UserType.Normal },
              new User { Address = "55 Street", Email= "test@gmail.com", Money= 60, Name = "Gabriela", Phone="789456123", UserType = UserType.SuperUser} };

            //Act
            bool result = repository.UserExists(newUser, userList);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void UserExists_UserExistsInDataBase_ReturnsFalse()
        {
            //Arrange
            var repository = new UserRepository(loggerMock.Object);
            var newUser = new User() { Address = "9 Street", Email = "newemail@gmail.com", Money = 100, Name = "Marcos", Phone = "555456789", UserType = UserType.Premium };

            var userList = new List<User>()
            { new User { Address = "19 Street", Email = "existingemail@gmail.com", Money = 150, Name = "Cintia", Phone = "123456789", UserType = UserType.Normal },
              new User { Address = "55 Street", Email= "test@gmail.com", Money= 60, Name = "Gabriela", Phone="789456123", UserType = UserType.SuperUser} };

            //Act
            bool result = repository.UserExists(newUser, userList);

            //Assert
            Assert.False(result);
        }

    }
}
