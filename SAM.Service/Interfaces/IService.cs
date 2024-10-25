using SAM.Entities;

namespace SAM.Services.Interfaces
{
    public interface IService<T>
        where T : BaseEntity
    {
        T Get(int id);
        List<T> GetAll();
        bool Delete(int id);
        T Update(T entity);
        T Create(T entity);
    }
}
