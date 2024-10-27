using Moq;
using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using Xunit;

namespace SAM.Tests.Serviceservices.Tests
{
    public class BaseServiceTests
    {
        private readonly Mock<IRepositoryDatabase<SampleEntity>> _repositoryMock;
        private readonly BaseService<SampleEntity> _baseService;

        public BaseServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<SampleEntity>>();
            _baseService = new SampleEntityService(_repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnCreatedEntity()
        {
            // Arrange
            var entity = new SampleEntity { Id = 1, Name = "Test Entity" };
            _repositoryMock.Setup(r => r.Create(It.IsAny<SampleEntity>())).Returns(entity);

            // Act
            var result = _baseService.Create(entity);

            // Assert
            Assert.Equal(entity, result);
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
            _repositoryMock.Setup(r => r.Read(entityId)).Returns(entity);

            // Act
            var result = _baseService.Get(entityId);

            // Assert
            Assert.Equal(entity, result);
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
            _repositoryMock.Setup(r => r.ReadAll()).Returns(entities);

            // Act
            var result = _baseService.GetAll();

            // Assert
            Assert.Equal(entities, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedEntity()
        {
            // Arrange
            var entity = new SampleEntity { Id = 1, Name = "Updated Entity" };
            _repositoryMock.Setup(r => r.Update(It.IsAny<SampleEntity>())).Returns(entity);

            // Act
            var result = _baseService.Update(entity);

            // Assert
            Assert.Equal(entity, result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<SampleEntity>()), Times.Once);
        }
    }

    public class SampleEntity : BaseEntity
    {
        public string Name { get; set; }
    }

    public class SampleEntityService : BaseService<SampleEntity>
    {
        public SampleEntityService(IRepositoryDatabase<SampleEntity> repository) : base(repository)
        {
        }
    }
}