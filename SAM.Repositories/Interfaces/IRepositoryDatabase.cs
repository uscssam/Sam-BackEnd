using System.Linq.Expressions;

namespace SAM.Repositories.Interfaces
{
    public interface IRepositoryDatabase<T> 
        where T: class
    {
        T Create(T model);
        T Read(int id);
        List<T> Search(Expression<Func<T, bool>> predicate);
        List<T> ReadAll();
        T Update(T model);
        void Delete(int id);
    }
}
