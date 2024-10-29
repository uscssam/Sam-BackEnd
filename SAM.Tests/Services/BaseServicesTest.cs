using AutoMapper;
using Moq;
using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.Dto;
using System.Linq.Expressions;
using Xunit;

namespace SAM.Tests.Services
{
    public class BaseServiceTests
    {
        private readonly Mock<IRepositoryDatabase<SampleEntity>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly BaseService<SampleDto, SampleEntity> _baseService;

        public BaseServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<SampleEntity>>();

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<SampleEntity, SampleDto>()
                    .ReverseMap()
            );
            _mapper = config.CreateMapper();

            _baseService = new SampleEntityService(_mapper, _repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnCreatedEntity()
        {
            // Arrange
            var entity = new SampleEntity { Id = 1, Name = "Test Entity" };
            var dto = _mapper.Map<SampleDto>(entity);
            _repositoryMock.Setup(r => r.Create(It.IsAny<SampleEntity>())).Returns(entity);

            // Act
            var result = _baseService.Create(dto);

            // Assert
            Assert.Equal(dto, result);
            _repositoryMock.Verify(r => r.Create(It.IsAny<SampleEntity>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenEntityIsDeleted()
        {
            // Arrange
            int entityId = 1;
            _repositoryMock.Setup(r => r.Delete(entityId)).Returns(true);

            // Act
            var result = _baseService.Delete(entityId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(entityId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenEntityIsNotDeleted()
        {
            // Arrange
            int entityId = 1;
            _repositoryMock.Setup(r => r.Delete(entityId)).Returns(false);

            // Act
            var result = _baseService.Delete(entityId);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(r => r.Delete(entityId), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnEntity_WhenEntityExists()
        {
            // Arrange
            int entityId = 1;
            var entity = new SampleEntity { Id = entityId, Name = "Test Entity" };
            var dto = _mapper.Map<SampleDto>(entity);
            _repositoryMock.Setup(r => r.Read(entityId)).Returns(entity);

            // Act
            var result = _baseService.Get(entityId);

            // Assert
            Assert.Equal(dto, result);
            _repositoryMock.Verify(r => r.Read(entityId), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            var entities = new List<SampleEntity>
            {
                new SampleEntity { Id = 1, Name = "Entity 1" },
                new SampleEntity { Id = 2, Name = "Entity 2" }
            };
            var dtos = _mapper.Map<IEnumerable<SampleDto>>(entities);
            _repositoryMock.Setup(r => r.ReadAll()).Returns(entities);

            // Act
            var result = _baseService.GetAll();

            // Assert
            Assert.Equal(dtos, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedEntity()
        {
            // Arrange
            var entity = new SampleEntity { Id = 1, Name = "Updated Entity" };
            var dto = _mapper.Map<SampleDto>(entity);
            _repositoryMock.Setup(r => r.Update(It.IsAny<SampleEntity>())).Returns(entity);

            // Act
            var result = _baseService.Update(entity.Id, dto);

            // Assert
            Assert.Equal(dto, result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<SampleEntity>()), Times.Once);
        }

        [Fact]
        public void Search_ShouldReturnEmpty_WhenNoPredicatesMatch()
        {
            // Arrange
            var dto = new SampleDto { Name = "Default Name" };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(new List<SampleEntity>());

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Search_ShouldReturnResults_WhenPredicatesMatch()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test Entity" };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test Entity" } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Entity", result.First().Name);
        }

        [Fact]
        public void Search_ShouldIgnoreDefaultIntValues()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test Entity" };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test Entity", Id = 1 } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Entity", result.First().Name);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public void Search_ShouldHandleStringProperties()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test" };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test Entity" } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Contains("Test", result.First().Name);
        }

        [Fact]
        public void Search_ShouldHandleDateTimeProperties()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test Entity", CreatedDate = DateTime.Now.Date };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test Entity", CreatedDate = DateTime.Now.Date } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Equal(DateTime.Now.Date, result.First().CreatedDate);
        }

        [Fact]
        public void Search_ShouldHandleEnumProperties()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test Entity", Status = SampleStatus.Active };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test Entity", Status = SampleStatus.Active } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Equal(SampleStatus.Active, result.First().Status);
        }

        [Fact]
        public void Search_ShouldHandleNullableIntProperties()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test Entity", NullableInt = 5 };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test Entity", NullableInt = 5 } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Equal(5, result.First().NullableInt);
        }

        [Fact]
        public void Search_ShouldIgnoreDefaultDateTimeValues()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test Entity", CreatedDate = DateTime.MinValue };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(new List<SampleEntity>());

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Search_ShouldHandleMultiplePredicates()
        {
            // Arrange
            var dto = new SampleDto { Name = "Test", Status = SampleStatus.Active };
            var entities = new List<SampleEntity> { new SampleEntity { Name = "Test", Status = SampleStatus.Active } };
            _repositoryMock.Setup(r => r.Search(It.IsAny<Expression<Func<SampleEntity, bool>>>())).Returns(entities);

            // Act
            var result = _baseService.Search(dto);

            // Assert
            Assert.Single(result);
            Assert.Equal("Test", result.First().Name);
            Assert.Equal(SampleStatus.Active, result.First().Status);
        }
    }
}

namespace SAM.Services.Dto
{
    public sealed class SampleDto : BaseDto, IEquatable<SampleDto>
    {
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public SampleStatus Status { get; set; }
        public int? NullableInt { get; set; }
        public SampleStatus? NullableStatus { get; set; }

        public bool Equals(SampleDto? other)
        {
            return Name == other?.Name;
        }
    }
}

namespace SAM.Entities
{
    public class SampleEntity : BaseEntity
    {
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public SampleStatus Status { get; set; }
        public int? NullableInt { get; set; }
        public SampleStatus? NullableStatus { get; set; }
    }
}

namespace SAM.Services.Abstract
{
    public class SampleEntityService : BaseService<SampleDto, SampleEntity>
    {
        public SampleEntityService(IMapper mapper, IRepositoryDatabase<SampleEntity> repository) : base(mapper, repository)
        {
        }
    }
}

namespace SAM
{
    public enum SampleStatus
    {
        Inactive = 0,
        Active = 1
    }
}
