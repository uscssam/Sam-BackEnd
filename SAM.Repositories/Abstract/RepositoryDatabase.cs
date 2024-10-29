using Microsoft.EntityFrameworkCore;
using SAM.Entities;
using SAM.Repositories.Database.Context;
using SAM.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace SAM.Repositories.Abstract
{
    [ExcludeFromCodeCoverage]
    public abstract class RepositoryDatabase<T> : IRepositoryDatabase<T>
        where T : BaseEntity
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

        public bool Delete(int id)
        {
            var entity = Read(id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.Now;
                Update(entity);
                return true;
            }
            return false;
        }

        public T Read(int id)
        {
            return dbSet.Where(r => r.Id == id && r.DeletedAt == null).AsNoTracking().FirstOrDefault()!;
        }

        public List<T> ReadAll()
        {
            return dbSet.Where(r => !r.DeletedAt.HasValue).AsNoTracking().ToList();
        }

        public List<T> Search(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(r => !r.DeletedAt.HasValue).Where(predicate).AsNoTracking().ToList();
        }

        public T Update(T model)
        {
            var ret = dbSet.Update(model).Entity;
            context.SaveChanges();
            return ret;
        }
    }
}
