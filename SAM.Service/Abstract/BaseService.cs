using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Interfaces;

namespace SAM.Services.Abstract
{
    public abstract class BaseService<T> : IService<T>
        where T : BaseEntity
    {
        protected readonly IRepositoryDatabase<T> repository;

        public BaseService(IRepositoryDatabase<T> repository)
        {
            this.repository = repository;
        }

        public virtual T Create(T entity)
        {
            return repository.Create(entity);
        }

        public virtual bool Delete(int id)
        {
            return repository.Delete(id);
        }

        public virtual T Get(int id)
        {
            return repository.Read(id);
        }

        public virtual List<T> GetAll()
        {
            return repository.ReadAll();
        }

        public virtual T Update(T entity)
        {
            return repository.Update(entity);
        }
    }
}
