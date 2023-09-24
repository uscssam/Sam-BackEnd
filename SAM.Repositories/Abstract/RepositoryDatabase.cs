using SAM.Repositories.Database.Context;
using SAM.Repositories.Interfaces;

namespace SAM.Repositories.Abstract
{
    public abstract class RepositoryDatabase<T> : IRepositoryDatabase<T>
        where T : class
    {
        private readonly MySqlContext context;

        public RepositoryDatabase(MySqlContext context)
        {
            this.context = context;
        }

        public T Create(T model)
        {
            var ret = context.Add(model).Entity;
            context.SaveChanges();
            return ret;
        }

        public void Delete(int id)
        {
            var entity = Read(id);
            context.Remove(entity);
            context.SaveChanges();
        }

        public T Read(int id)
        {
            return context.Set<T>().Find(id);
        }    

        public List<T> ReadAll()
        {
            return context.Set<T>().ToList();
        }

        public List<T> Search(T model)
        {
            throw new NotImplementedException();
        }

        public T Update(T model)
        {
            var ret = context.Update(model).Entity;
            context.SaveChanges();
            return ret;
        }
    }
}
