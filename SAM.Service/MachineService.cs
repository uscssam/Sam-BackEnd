using AutoMapper;
using SAM.Entities;
using SAM.Entities.Enum;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.Dto;

namespace SAM.Service
{
    public class MachineService : BaseService<MachineDto, Machine>
    {
        private readonly IRepositoryDatabase<OrderService> orderRepository;

        public MachineService(IMapper mapper, IRepositoryDatabase<Machine> repository, IRepositoryDatabase<OrderService> orderRepository) : base(mapper, repository)
        {
            this.orderRepository = orderRepository;
        }


        public override MachineDto Create(MachineDto entity)
        {
            entity.Status = MachineStatusEnum.Active;
            return base.Create(entity);
        }

        public override bool Delete(int id)
        {
            if(orderRepository.Search(o => o.IdMachine == id).Any())
            {
                throw new ArgumentException("A máquina informada possui ordens de serviço");
            }
            return base.Delete(id);
        }

        public override MachineDto Update(int id, MachineDto entity)
        {
            var machine = mapper.Map<MachineDto>(repository.Read(id));
            machine.Name = entity.Name;
            return base.Update(id, machine);
        }

        public IEnumerable<MachineDto> ListByUnit(int unitId)
        {
            return mapper.Map<IEnumerable<MachineDto>>(repository.Search(m => m.IdUnit == unitId));
        }
    }
}
