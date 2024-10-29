using AutoMapper;
using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.Dto;

namespace SAM.Services
{
    public class UnitService : BaseService<UnitDto, Unit>
    {
        public UnitService(IMapper mapper, IRepositoryDatabase<Unit> repository) : base(mapper, repository)
        {
        }
    }
}
