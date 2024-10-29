using AutoMapper;
using Moq;
using SAM.Entities;
using SAM.Entities.Enum;
using SAM.Repositories.Interfaces;
using SAM.Service;
using SAM.Services.AutoMapper;
using SAM.Services.Dto;
using Xunit;

namespace SAM.Tests.Services
{
    public class MachineServiceTests
    {
        private readonly Mock<IRepositoryDatabase<Machine>> _repositoryMock;
        private readonly Mock<IRepositoryDatabase<OrderService>> _orderRepositoryMock;
        private readonly IMapper _mapper;
        private readonly MachineService _machineService;

        public MachineServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<Machine>>();
            _orderRepositoryMock = new Mock<IRepositoryDatabase<OrderService>>();
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MapperProfile())
            );
            _mapper = config.CreateMapper();

            _machineService = new MachineService(_mapper, _repositoryMock.Object, _orderRepositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnCreatedMachine()
        {
            // Arrange
            var machine = new Machine { Id = 1, Name = "Test Machine", Status = MachineStatusEnum.Active, IdUnit = 1 };
            var machineDto = _mapper.Map<MachineDto>(machine);
            _repositoryMock.Setup(r => r.Create(It.IsAny<Machine>())).Returns(machine);

            // Act
            var result = _machineService.Create(machineDto);

            // Assert
            Assert.Equal(machineDto, result);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Machine>()), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnMachine_WhenMachineExists()
        {
            // Arrange
            int machineId = 1;
            var machine = new Machine { Id = machineId, Name = "Test Machine", Status = MachineStatusEnum.Active, IdUnit = 1 };
            var machineDto = _mapper.Map<MachineDto>(machine);
            _repositoryMock.Setup(r => r.Read(machineId)).Returns(machine);

            // Act
            var result = _machineService.Get(machineId);

            // Assert
            Assert.Equal(machineDto, result);
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
            var machineDtos = _mapper.Map<IEnumerable<MachineDto>>(machines);
            _repositoryMock.Setup(r => r.ReadAll()).Returns(machines);

            // Act
            var result = _machineService.GetAll();

            // Assert
            Assert.Equal(machineDtos, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
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
            var machineDtos = _mapper.Map<IEnumerable<MachineDto>>(machines);
            _repositoryMock.Setup(r => r.Search(m => m.IdUnit == unitId)).Returns(machines);

            // Act
            var result = _machineService.ListByUnit(unitId);

            // Assert
            Assert.Equal(machineDtos, result);
            _repositoryMock.Verify(r => r.Search(m => m.IdUnit == unitId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenMachineHasOrders()
        {
            // Arrange
            int machineId = 1;
            _orderRepositoryMock.Setup(r => r.Search(o => o.IdMachine == machineId)).Returns(new List<OrderService>
                {
                    new OrderService
                    {
                        Description = "Test Order",
                        Status = OrderServiceStatusEnum.Open,
                        Opening = DateTime.Now,
                        IdMachine = machineId,
                        CreatedBy = 1
                    }
                });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _machineService.Delete(machineId));
            Assert.Equal("A máquina informada possui ordens de serviço", exception.Message);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenMachineHasNoOrders()
        {
            // Arrange
            int machineId = 1;
            var machine = new Machine { Id = machineId, Name = "Test Machine", Status = MachineStatusEnum.Active, IdUnit = 1 };
            _orderRepositoryMock.Setup(r => r.Search(o => o.IdMachine == machineId)).Returns(new List<OrderService>());
            _repositoryMock.Setup(r => r.Read(machineId)).Returns(machine);
            _repositoryMock.Setup(r => r.Delete(machineId)).Returns(true);

            // Act
            var result = _machineService.Delete(machineId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(machineId), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedMachine()
        {
            // Arrange
            int machineId = 1;
            var existingMachine = new Machine { Id = machineId, Name = "Old Machine", Status = MachineStatusEnum.Active, IdUnit = 1 };
            var updatedMachineDto = new MachineDto { Name = "Updated Machine", IdUnit = 1 };
            _repositoryMock.Setup(r => r.Read(machineId)).Returns(existingMachine);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Machine>())).Returns((Machine m) => m);

            // Act
            var result = _machineService.Update(machineId, updatedMachineDto);

            // Assert
            Assert.Equal(updatedMachineDto.Name, result.Name);
        }
    }
}