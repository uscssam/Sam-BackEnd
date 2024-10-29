using SAM.Services.Dto;

namespace SAM.Services.Interfaces
{
    public interface IService<TDto>
        where TDto : BaseDto
    {
        TDto Get(int id);
        IEnumerable<TDto> GetAll();
        IEnumerable<TDto> Search(TDto entity);
        bool Delete(int id);
        TDto Update(int id, TDto entity);
        TDto Create(TDto entity);
    }
}
