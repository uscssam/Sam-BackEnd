using Microsoft.EntityFrameworkCore;
using SAM.Repositories.Database.Context;
using SAM.Repositories.Interfaces;
using System;
using System.Linq.Expressions;

namespace SAM.Repositories.Abstract
{
    public abstract class RepositoryDatabase<T> : IRepositoryDatabase<T>
        where T : class
    {
        protected readonly MySqlContext context;
        protected readonly DbSet<T> dbSet;

        public RepositoryDatabase(MySqlContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
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

        public List<T> Search(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).AsNoTracking().ToList();
        }

        public T Update(T model)
        {
            var ret = context.Update(model).Entity;
            context.SaveChanges();
            return ret;
        }
    }
}
