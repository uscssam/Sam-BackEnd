using Moq;
using SAM.Entities;
using SAM.Entities.Enum;
using SAM.Repositories.Interfaces;
using SAM.Services;
using Xunit;

namespace SAM.Tests.Services
{
    public class OrderServiceServiceTests
    {
        private readonly Mock<IRepositoryDatabase<OrderService>> _repositoryMock;
        private readonly OrderServiceService _orderServiceService;

        public OrderServiceServiceTests()
        {
            _repositoryMock = new Mock<IRepositoryDatabase<OrderService>>();
            _orderServiceService = new OrderServiceService(_repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnCreatedOrderService()
        {
            // Arrange
            var orderService = new OrderService
            {
                Id = 1,
                Description = "Test Order Service",
                Status = OrderServiceStatusEnum.Open,
                Opening = DateTime.Now,
                IdMachine = 1,
                IdTechnician = 1,
                CreatedBy = 1
            };
            _repositoryMock.Setup(r => r.Create(It.IsAny<OrderService>())).Returns(orderService);

            // Act
            var result = _orderServiceService.Create(orderService);

            // Assert
            Assert.Equal(orderService, result);
            _repositoryMock.Verify(r => r.Create(It.IsAny<OrderService>()), Times.Once);
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
                Id = orderServiceId,
                Description = "Test Order Service",
                Status = OrderServiceStatusEnum.Open,
                Opening = DateTime.Now,
                IdMachine = 1,
                IdTechnician = 1,
                CreatedBy = 1
            };
            _repositoryMock.Setup(r => r.Read(orderServiceId)).Returns(orderService);

            // Act
            var result = _orderServiceService.Get(orderServiceId);

            // Assert
            Assert.Equal(orderService, result);
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
                    Id = 1,
                    Description = "Order Service 1",
                    Status = OrderServiceStatusEnum.Open,
                    Opening = DateTime.Now,
                    IdMachine = 1,
                    IdTechnician = 1,
                    CreatedBy = 1
                },
                new OrderService
                {
                    Id = 2,
                    Description = "Order Service 2",
                    Status = OrderServiceStatusEnum.Completed,
                    Opening = DateTime.Now,
                    IdMachine = 2,
                    IdTechnician = 2,
                    CreatedBy = 2
                }
            };
            _repositoryMock.Setup(r => r.ReadAll()).Returns(orderServices);

            // Act
            var result = _orderServiceService.GetAll();

            // Assert
            Assert.Equal(orderServices, result);
            _repositoryMock.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedOrderService()
        {
            // Arrange
            var orderService = new OrderService
            {
                Id = 1,
                Description = "Updated Order Service",
                Status = OrderServiceStatusEnum.Completed,
                Opening = DateTime.Now,
                IdMachine = 1,
                IdTechnician = 1,
                CreatedBy = 1
            };
            _repositoryMock.Setup(r => r.Update(It.IsAny<OrderService>())).Returns(orderService);

            // Act
            var result = _orderServiceService.Update(orderService);

            // Assert
            Assert.Equal(orderService, result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<OrderService>()), Times.Once);
        }
    }
}