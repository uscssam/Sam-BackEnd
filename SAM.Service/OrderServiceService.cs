using AutoMapper;
using SAM.Entities;
using SAM.Entities.Enum;
using SAM.Entities.Interfaces;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.Dto;

namespace SAM.Services
{
    public class OrderServiceService : BaseService<OrderServiceDto, OrderService>
    {
        private readonly ICurrentUser currentUser;
        private readonly IRepositoryDatabase<Machine> machineRepository;

        public OrderServiceService(
            IMapper mapper, 
            IRepositoryDatabase<OrderService> repository, 
            ICurrentUser currentUser,
            IRepositoryDatabase<Machine> machineRepository)
            : base(mapper, repository)
        {
            this.currentUser = currentUser;
            this.machineRepository = machineRepository;
        }

        public override OrderServiceDto Create(OrderServiceDto entity)
        {
            if (!entity.IdMachine.HasValue) {
                throw new ArgumentException("O valor de IdMachine informado é inválido.");
            }
            var machine = machineRepository.Read(entity.IdMachine.Value) ?? throw new ArgumentException("O valor de IdMachine informado é inválido.");
            entity.Status = OrderServiceStatusEnum.Open;
            entity.CreatedBy = currentUser.Id;
            entity.Opening = DateTime.Now;
            machine.Status = MachineStatusEnum.Inactive;

            if (entity.IdTechnician.HasValue)
            {
                entity.Status = OrderServiceStatusEnum.InProgress;
                machine.Status = MachineStatusEnum.Maintenance;
            }

            entity = base.Create(entity);
            machineRepository.Update(machine);
            return entity!;
        }

        public override OrderServiceDto Update(int id, OrderServiceDto entity)
        {
            var orderService = mapper.Map<OrderServiceDto>(repository.Read(id)) ?? throw new ArgumentException("Ordem de serviço não encontrada");
            var machine = machineRepository.Read(orderService.IdMachine!.Value);

            if (entity.IdTechnician.HasValue)
            {
                orderService.Status = OrderServiceStatusEnum.InProgress;
                orderService.IdTechnician = entity.IdTechnician;
                machine.Status = MachineStatusEnum.Maintenance;
            }

            if(entity.Status.HasValue)
            {
                switch (entity.Status)
                {
                    case OrderServiceStatusEnum.InProgress:
                        machine.Status = MachineStatusEnum.Maintenance;
                        break;
                    case OrderServiceStatusEnum.Completed:
                        machine.Status = MachineStatusEnum.Active;
                        machine.LastMaintenance = orderService.Closed = DateTime.Now;
                        break;
                    case OrderServiceStatusEnum.Impeded:
                        machine.Status = MachineStatusEnum.Inactive;
                        break;
                }
                orderService.Status = entity.Status;
            }
            entity = base.Update(id, orderService);
            machineRepository.Update(machine);
            return entity!;
        }
    }
}
