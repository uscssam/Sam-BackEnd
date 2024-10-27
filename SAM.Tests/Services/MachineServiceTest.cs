using Moq;
using SAM.Entities;
using SAM.Entities.Enum;
using SAM.Repositories.Interfaces;
using SAM.Service;
using Xunit;

namespace SAM.Tests.Services
{
    public class MachineServiceTests
    {
        private readonly Mock<IRepositoryDatabase<Machine>> _repositoryMock;
        private readonly MachineService _machineService;

        public MachineServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<Machine>>();
            _machineService = new MachineService(_repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnCreatedMachine()
        {
            // Arrange
            var machine = new Machine { Id = 1, Name = "Test Machine", Status = MachineStatusEnum.Active, IdUnit = 1 };
            _repositoryMock.Setup(r => r.Create(It.IsAny<Machine>())).Returns(machine);

            // Act
            var result = _machineService.Create(machine);

            // Assert
            Assert.Equal(machine, result);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Machine>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenMachineIsDeleted()
        {
            // Arrange
            int machineId = 1;
            _repositoryMock.Setup(r => r.Delete(machineId)).Returns(true);

            // Act
            var result = _machineService.Delete(machineId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(machineId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenMachineIsNotDeleted()
        {
            // Arrange
            int machineId = 1;
            _repositoryMock.Setup(r => r.Delete(machineId)).Returns(false);

            // Act
            var result = _machineService.Delete(machineId);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(r => r.Delete(machineId), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnMachine_WhenMachineExists()
        {
            // Arrange
            int machineId = 1;
            var machine = new Machine { Id = machineId, Name = "Test Machine", Status = MachineStatusEnum.Active, IdUnit = 1 };
            _repositoryMock.Setup(r => r.Read(machineId)).Returns(machine);

            // Act
            var result = _machineService.Get(machineId);

            // Assert
            Assert.Equal(machine, result);
            _repositoryMock.Verify(r => r.Read(machineId), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllMachines()
        {
            // Arrange
            var machines = new List<Machine>
            {
                new Machine { Id = 1, Name = "Machine 1", Status = MachineStatusEnum.Active, IdUnit = 1 },
                new Machine { Id = 2, Name = "Machine 2", Status = MachineStatusEnum.Inactive, IdUnit = 2 }
            };
            _repositoryMock.Setup(r => r.ReadAll()).Returns(machines);

            // Act
            var result = _machineService.GetAll();

            // Assert
            Assert.Equal(machines, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedMachine()
        {
            // Arrange
            var machine = new Machine { Id = 1, Name = "Updated Machine", Status = MachineStatusEnum.Maintenance, IdUnit = 1 };
            _repositoryMock.Setup(r => r.Update(It.IsAny<Machine>())).Returns(machine);

            // Act
            var result = _machineService.Update(machine);

            // Assert
            Assert.Equal(machine, result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Machine>()), Times.Once);
        }

        [Fact]
        public void ListByUnit_ShouldReturnMachinesForGivenUnit()
        {
            // Arrange
            int unitId = 1;
            var machines = new List<Machine>
            {
                new Machine { Id = 1, Name = "Machine 1", Status = MachineStatusEnum.Active, IdUnit = unitId },
                new Machine { Id = 2, Name = "Machine 2", Status = MachineStatusEnum.Inactive, IdUnit = unitId }
            };
            _repositoryMock.Setup(r => r.Search(m => m.IdUnit == unitId)).Returns(machines);

            // Act
            var result = _machineService.ListByUnit(unitId);

            // Assert
            Assert.Equal(machines, result);
            _repositoryMock.Verify(r => r.Search(m => m.IdUnit == unitId), Times.Once);
        }
    }
}