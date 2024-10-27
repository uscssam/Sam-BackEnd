using Moq;
using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services;
using Xunit;

namespace SAM.Tests.Services
{
    public class UnitServiceTests
    {
        private readonly Mock<IRepositoryDatabase<Unit>> _repositoryMock;
        private readonly UnitService _unitService;

        public UnitServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<Unit>>();
            _unitService = new UnitService(_repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnCreatedUnit()
        {
            // Arrange
            var unit = new Unit { Id = 1, Name = "Test Unit", Street = "Test Street", Neighborhood = "Test Neighborhood", CEP = "12345-678", Number = 123, Phone = "123456789" };
            _repositoryMock.Setup(r => r.Create(It.IsAny<Unit>())).Returns(unit);

            // Act
            var result = _unitService.Create(unit);

            // Assert
            Assert.Equal(unit, result);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Unit>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenUnitIsDeleted()
        {
            // Arrange
            int unitId = 1;
            _repositoryMock.Setup(r => r.Delete(unitId)).Returns(true);

            // Act
            var result = _unitService.Delete(unitId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(unitId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenUnitIsNotDeleted()
        {
            // Arrange
            int unitId = 1;
            _repositoryMock.Setup(r => r.Delete(unitId)).Returns(false);

            // Act
            var result = _unitService.Delete(unitId);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(r => r.Delete(unitId), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnUnit_WhenUnitExists()
        {
            // Arrange
            int unitId = 1;
            var unit = new Unit { Id = unitId, Name = "Test Unit", Street = "Test Street", Neighborhood = "Test Neighborhood", CEP = "12345-678", Number = 123, Phone = "123456789" };
            _repositoryMock.Setup(r => r.Read(unitId)).Returns(unit);

            // Act
            var result = _unitService.Get(unitId);

            // Assert
            Assert.Equal(unit, result);
            _repositoryMock.Verify(r => r.Read(unitId), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllUnits()
        {
            // Arrange
            var units = new List<Unit>
            {
                new Unit { Id = 1, Name = "Unit 1", Street = "Street 1", Neighborhood = "Neighborhood 1", CEP = "12345-678", Number = 123, Phone = "123456789" },
                new Unit { Id = 2, Name = "Unit 2", Street = "Street 2", Neighborhood = "Neighborhood 2", CEP = "23456-789", Number = 456, Phone = "987654321" }
            };
            _repositoryMock.Setup(r => r.ReadAll()).Returns(units);

            // Act
            var result = _unitService.GetAll();

            // Assert
            Assert.Equal(units, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedUnit()
        {
            // Arrange
            var unit = new Unit { Id = 1, Name = "Updated Unit", Street = "Updated Street", Neighborhood = "Updated Neighborhood", CEP = "12345-678", Number = 123, Phone = "123456789" };
            _repositoryMock.Setup(r => r.Update(It.IsAny<Unit>())).Returns(unit);

            // Act
            var result = _unitService.Update(unit);

            // Assert
            Assert.Equal(unit, result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Unit>()), Times.Once);
        }
    }
}