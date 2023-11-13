using SAM.Entities;
using System.Linq.Expressions;

namespace SAM.Repositories.Interfaces
{
    public interface IRepositoryDatabase<T> 
        where T: BaseEntity
    {
        T Create(T model);
        T Read(int id);
        List<T> Search(Expression<Func<T, bool>> predicate);
        List<T> ReadAll();
        T Update(T model);
        bool Delete(int id);
    }
}
