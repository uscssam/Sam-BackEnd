using AutoMapper;
using Moq;
using SAM.Entities;
using SAM.Entities.Enum;
using SAM.Entities.Interfaces;
using SAM.Repositories.Interfaces;
using SAM.Services;
using SAM.Services.AutoMapper;
using SAM.Services.Dto;
using SAM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace SAM.Tests.Services
{
    public class OrderServiceServiceTests
    {
        private readonly Mock<IRepositoryDatabase<OrderService>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<IService<MachineDto>> _machineService;
        private readonly OrderServiceService _orderServiceService;

        public OrderServiceServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<OrderService>>();
            var _currentUser = new Mock<ICurrentUser>();
            _currentUser.Setup(c => c.Id).Returns(1);
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MapperProfile())
            );
            _mapper = config.CreateMapper();
            _machineService = new Mock<IService<MachineDto>>();

            _orderServiceService = new OrderServiceService(_mapper, _repositoryMock.Object, _currentUser.Object, _machineService.Object);
        }

        private static void SetId<T>(T obj, int id)
        {
            var property = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, id);
            }
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenOrderServiceIsDeleted()
        {
            // Arrange
            int orderServiceId = 1;
            _repositoryMock.Setup(r => r.Delete(orderServiceId)).Returns(true);

            // Act
            var result = _orderServiceService.Delete(orderServiceId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(orderServiceId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenOrderServiceIsNotDeleted()
        {
            // Arrange
            int orderServiceId = 1;
            _repositoryMock.Setup(r => r.Delete(orderServiceId)).Returns(false);

            // Act
            var result = _orderServiceService.Delete(orderServiceId);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(r => r.Delete(orderServiceId), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnOrderService_WhenOrderServiceExists()
        {
            // Arrange
            int orderServiceId = 1;
            var orderService = new OrderService
            {
                Description = "Test Order Service",
                Status = OrderServiceStatusEnum.Open,
                Opening = DateTime.Now,
                IdMachine = 1,
                IdTechnician = 1,
                CreatedBy = 1
            };
            SetId(orderService, orderServiceId);
            var orderServiceDto = _mapper.Map<OrderServiceDto>(orderService);
            _repositoryMock.Setup(r => r.Read(orderServiceId)).Returns(orderService);

            // Act
            var result = _orderServiceService.Get(orderServiceId);

            // Assert
            Assert.Equal(orderServiceDto, result);
            _repositoryMock.Verify(r => r.Read(orderServiceId), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllOrderServices()
        {
            // Arrange
            var orderServices = new List<OrderService>
            {
                new OrderService
                {
                    Description = "Order Service 1",
                    Status = OrderServiceStatusEnum.Open,
                    Opening = DateTime.Now,
                    IdMachine = 1,
                    IdTechnician = 1,
                    CreatedBy = 1
                },
                new OrderService
                {
                    Description = "Order Service 2",
                    Status = OrderServiceStatusEnum.Completed,
                    Opening = DateTime.Now,
                    IdMachine = 2,
                    IdTechnician = 2,
                    CreatedBy = 2
                }
            };
            SetId(orderServices[0], 1);
            SetId(orderServices[1], 2);
            var orderServiceDtos = _mapper.Map<IEnumerable<OrderServiceDto>>(orderServices);
            _repositoryMock.Setup(r => r.ReadAll()).Returns(orderServices);

            // Act
            var result = _orderServiceService.GetAll();

            // Assert
            Assert.Equal(orderServiceDtos, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void Update_ShouldUpdateOrderServiceAndMachineStatus()
        {
            // Arrange
            int orderServiceId = 1;
            var orderServiceDto = new OrderServiceDto
            {
                IdTechnician = 2,
                Status = OrderServiceStatusEnum.Completed
            };
            SetId(orderServiceDto, orderServiceId);
            var orderService = new OrderService
            {
                Description = "Test Order Service",
                Status = OrderServiceStatusEnum.InProgress,
                Opening = DateTime.Now,
                IdMachine = 1,
                CreatedBy = 1
            };
            SetId(orderService, orderServiceId);
            var machineDto = new MachineDto
            {
                Name = "Machine 1",
                Status = MachineStatusEnum.Maintenance,
                IdUnit = 1
            };
            SetId(machineDto, 1);

            _repositoryMock.Setup(r => r.Read(orderServiceId)).Returns(orderService);
            _machineService.Setup(m => m.Get(1)).Returns(machineDto);
            _repositoryMock.Setup(r => r.Update(It.IsAny<OrderService>())).Callback<OrderService>(os =>
            {
                os.Status = OrderServiceStatusEnum.Completed;
                os.Closed = DateTime.Now;
            }).Returns((OrderService os) => os);
            _machineService.Setup(m => m.Update(1, It.IsAny<MachineDto>())).Callback<int, MachineDto>((id, updatedMachine) =>
            {
                machineDto.Status = MachineStatusEnum.Active;
            });

            // Act
            var result = _orderServiceService.Update(orderServiceId, orderServiceDto);

            // Assert
            Assert.Equal(OrderServiceStatusEnum.Completed, result.Status);
            Assert.Equal(MachineStatusEnum.Active, machineDto.Status);
            Assert.NotNull(result.Closed);
            _repositoryMock.Verify(r => r.Read(orderServiceId), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<OrderService>()), Times.Once);
            _machineService.Verify(m => m.Update(1, It.IsAny<MachineDto>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenIdMachineIsInvalid()
        {
            // Arrange
            var orderServiceDto = new OrderServiceDto
            {
                IdMachine = null
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _orderServiceService.Create(orderServiceDto));
            Assert.Equal("O valor de IdMachine informado é inválido.", exception.Message);
        }

        [Fact]
        public void Create_ShouldSetStatusToOpenAndMachineStatusToInactive_WhenIdMachineIsValidAndNoIdTechnician()
        {
            // Arrange
            var orderServiceDto = new OrderServiceDto
            {
                IdMachine = 1
            };
            var machineDto = new MachineDto
            {
                Name = "Machine 1",
                Status = MachineStatusEnum.Active,
                IdUnit = 1
            };
            SetId(machineDto, 1);

            _machineService.Setup(m => m.Get(1)).Returns(machineDto);
            _repositoryMock.Setup(r => r.Create(It.IsAny<OrderService>())).Returns((OrderService os) => os);

            // Act
            var result = _orderServiceService.Create(orderServiceDto);

            // Assert
            Assert.Equal(OrderServiceStatusEnum.Open, result.Status);
            Assert.Equal(MachineStatusEnum.Inactive, machineDto.Status);
            Assert.Equal(1, result.CreatedBy);
            Assert.NotNull(result.Opening);
            _machineService.Verify(m => m.Update(1, machineDto), Times.Once);
        }

        [Fact]
        public void Create_ShouldSetStatusToInProgressAndMachineStatusToMaintenance_WhenIdMachineIsValidAndIdTechnicianIsPresent()
        {
            // Arrange
            var orderServiceDto = new OrderServiceDto
            {
                IdMachine = 1,
                IdTechnician = 2
            };
            var machineDto = new MachineDto
            {
                Name = "Machine 1",
                Status = MachineStatusEnum.Active,
                IdUnit = 1
            };
            SetId(machineDto, 1);

            _machineService.Setup(m => m.Get(1)).Returns(machineDto);
            _repositoryMock.Setup(r => r.Create(It.IsAny<OrderService>())).Returns((OrderService os) => os);

            // Act
            var result = _orderServiceService.Create(orderServiceDto);

            // Assert
            Assert.Equal(OrderServiceStatusEnum.InProgress, result.Status);
            Assert.Equal(MachineStatusEnum.Maintenance, machineDto.Status);
            Assert.Equal(1, result.CreatedBy);
            Assert.NotNull(result.Opening);
            _machineService.Verify(m => m.Update(1, machineDto), Times.Once);
        }

        [Fact]
        public void Update_ShouldSetMachineStatusToMaintenance_WhenStatusIsInProgress()
        {
            // Arrange
            int orderServiceId = 1;
            var orderServiceDto = new OrderServiceDto
            {
                Status = OrderServiceStatusEnum.InProgress
            };
            var orderService = new OrderService
            {
                Description = "Test Order Service",
                Status = OrderServiceStatusEnum.Open,
                Opening = DateTime.Now,
                IdMachine = 1,
                CreatedBy = 1
            };
            SetId(orderService, orderServiceId);
            var machineDto = new MachineDto
            {
                Name = "Machine 1",
                Status = MachineStatusEnum.Active,
                IdUnit = 1
            };
            SetId(machineDto, 1);

            _repositoryMock.Setup(r => r.Read(orderServiceId)).Returns(orderService);
            _machineService.Setup(m => m.Get(1)).Returns(machineDto);
            _repositoryMock.Setup(r => r.Update(It.IsAny<OrderService>())).Returns((OrderService os) => os);
            _machineService.Setup(m => m.Update(1, It.IsAny<MachineDto>())).Callback<int, MachineDto>((id, updatedMachine) =>
            {
                machineDto.Status = updatedMachine.Status;
            });

            // Act
            var result = _orderServiceService.Update(orderServiceId, orderServiceDto);

            // Assert
            Assert.Equal(OrderServiceStatusEnum.InProgress, result.Status);
            Assert.Equal(MachineStatusEnum.Maintenance, machineDto.Status);
            _repositoryMock.Verify(r => r.Read(orderServiceId), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<OrderService>()), Times.Once);
            _machineService.Verify(m => m.Update(1, machineDto), Times.Once);
        }

        [Fact]
        public void Update_ShouldSetMachineStatusToInactive_WhenStatusIsImpeded()
        {
            // Arrange
            int orderServiceId = 1;
            var orderServiceDto = new OrderServiceDto
            {
                Status = OrderServiceStatusEnum.Impeded
            };
            var orderService = new OrderService
            {
                Description = "Test Order Service",
                Status = OrderServiceStatusEnum.Open,
                Opening = DateTime.Now,
                IdMachine = 1,
                CreatedBy = 1
            };
            SetId(orderService, orderServiceId);
            var machineDto = new MachineDto
            {
                Name = "Machine 1",
                Status = MachineStatusEnum.Active,
                IdUnit = 1
            };
            SetId(machineDto, 1);

            _repositoryMock.Setup(r => r.Read(orderServiceId)).Returns(orderService);
            _machineService.Setup(m => m.Get(1)).Returns(machineDto);
            _repositoryMock.Setup(r => r.Update(It.IsAny<OrderService>())).Returns((OrderService os) => os);
            _machineService.Setup(m => m.Update(1, It.IsAny<MachineDto>())).Callback<int, MachineDto>((id, updatedMachine) =>
            {
                machineDto.Status = updatedMachine.Status;
            });

            // Act
            var result = _orderServiceService.Update(orderServiceId, orderServiceDto);

            // Assert
            Assert.Equal(OrderServiceStatusEnum.Impeded, result.Status);
            Assert.Equal(MachineStatusEnum.Inactive, machineDto.Status);
            _repositoryMock.Verify(r => r.Read(orderServiceId), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<OrderService>()), Times.Once);
            _machineService.Verify(m => m.Update(1, machineDto), Times.Once);
        }


    }
}
