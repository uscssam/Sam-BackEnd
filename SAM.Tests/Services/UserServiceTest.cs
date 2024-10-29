using AutoMapper;
using Moq;
using SAM.Entities.Enum;
using SAM.Repositories.Interfaces;
using SAM.Services;
using SAM.Services.AutoMapper;
using SAM.Services.Dto;
using System.Linq.Expressions;
using Xunit;

namespace SAM.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepositoryDatabase<User>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<User>>();
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MapperProfile())
            );
            _mapper = config.CreateMapper();

            _userService = new UserService(_mapper, _repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenUserNameAlreadyExists()
        {
            // Arrange
            var user = new User
            {
                UserName = "existingUser",
                Fullname = "Test User",
                Email = "test@example.com",
                Phone = "1234567890",
                Level = LevelEnum.Employee
            };
            var userDto = _mapper.Map<UserDto>(user);
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User> { user });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _userService.Create(userDto));
            Assert.Equal("Nome do usuário já cadastrado", exception.Message);
            _repositoryMock.Verify(r => r.Search(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldReturnCreatedUser_WhenUserNameDoesNotExist()
        {
            // Arrange
            var user = new User
            {
                UserName = "newUser",
                Fullname = "Test User",
                Email = "test@example.com",
                Phone = "1234567890",
                Level = LevelEnum.Employee
            };
            var userDto = _mapper.Map<UserDto>(user);
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>());
            _repositoryMock.Setup(r => r.Create(It.IsAny<User>())).Returns(user);

            // Act
            var result = _userService.Create(userDto);

            // Assert
            Assert.Equal(userDto, result);
            _repositoryMock.Verify(r => r.Search(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _repositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldKeepExistingPassword_WhenPasswordIsNullOrEmpty()
        {
            // Arrange
            var existingUser = new User
            {
                Id = 1,
                UserName = "existingUser",
                Password = "existingPassword",
                Phone = "1234567890",
                Level = LevelEnum.Employee,
                Fullname = "Existing User",
                Email = "existing@example.com"
            };
            var updatedUser = new User
            {
                Id = 1,
                UserName = "existingUser",
                Password = null,
                Phone = "1234567890",
                Level = LevelEnum.Employee,
                Fullname = "Existing User",
                Email = "existing@example.com"
            };
            var updatedUserDto = _mapper.Map<UserDto>(updatedUser);
            _repositoryMock.Setup(r => r.Read(existingUser.Id)).Returns(existingUser);
            _repositoryMock.Setup(r => r.Update(It.IsAny<User>())).Returns((User u) => u);

            // Act
            var result = _userService.Update(existingUser.Id, updatedUserDto);

            // Assert
            Assert.Equal(existingUser.Password, result.Password);
            _repositoryMock.Verify(r => r.Read(existingUser.Id), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedUser_WhenPasswordIsNotNullOrEmpty()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                UserName = "existingUser",
                Password = "newPassword",
                Phone = "1234567890",
                Level = LevelEnum.Employee,
                Fullname = "Existing User",
                Email = "existing@example.com"
            };
            var userDto = _mapper.Map<UserDto>(user);
            _repositoryMock.Setup(r => r.Update(It.IsAny<User>())).Returns(user);

            // Act
            var result = _userService.Update(user.Id, userDto);

            // Assert
            Assert.Equal(userDto, result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        }
    }
}