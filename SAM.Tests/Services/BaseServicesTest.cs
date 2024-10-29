using AutoMapper;
using Moq;
using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.AutoMapper;
using SAM.Services.Dto;
using Xunit;

namespace SAM.Tests.Serviceservices.Tests
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
    }

    public class SampleDto: BaseDto, IEquatable<SampleDto>
    {
        public required string Name { get; set; }

        public bool Equals(SampleDto? other)
        {
            return Name == other?.Name;
        }
    }

    public class SampleEntity : BaseEntity
    {
        public required string Name { get; set; }
    }

    public class SampleEntityService : BaseService<SampleDto, SampleEntity>
    {
        public SampleEntityService(IMapper mapper, IRepositoryDatabase<SampleEntity> repository) : base(mapper, repository)
        {
        }
    }
}