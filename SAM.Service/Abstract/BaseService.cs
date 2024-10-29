using AutoMapper;
using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Dto;
using SAM.Services.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SAM.Services.Abstract
{
    public abstract class BaseService<TDto, TEntity> : IService<TDto>
            where TDto : BaseDto
            where TEntity : BaseEntity
    {
        protected readonly IMapper mapper;
        protected readonly IRepositoryDatabase<TEntity> repository;

        public BaseService(IMapper mapper, IRepositoryDatabase<TEntity> repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public virtual TDto Create(TDto entity)
        {
            return mapper.Map<TDto>(repository.Create(mapper.Map<TEntity>(entity)));
        }

        public virtual bool Delete(int id)
        {
            return repository.Delete(id);
        }

        public virtual TDto Get(int id)
        {
            return mapper.Map<TDto>(repository.Read(id));
        }

        public virtual IEnumerable<TDto> GetAll()
        {
            return mapper.Map<IEnumerable<TDto>>(repository.ReadAll());
        }

        public IEnumerable<TDto> Search(TDto entity)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var predicates = new List<Expression>();

            foreach (var property in typeof(TDto).GetProperties())
            {
                var value = property.GetValue(entity);
                if (value != null)
                {
                    if (property.PropertyType == typeof(int) && (int)value == 0)
                        continue;
                    var entityProperty = typeof(TEntity).GetProperty(property.Name);
                    var left = Expression.Property(parameter, entityProperty!);
                    Expression predicate = null!;

                    if (property.PropertyType == typeof(string))
                    {
                        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var right = Expression.Constant(value, typeof(string));
                        predicate = Expression.Call(left, method!, right);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        if ((DateTime)value == DateTime.MinValue)
                            continue;
                        var right = Expression.Constant(((DateTime)value).Date, typeof(DateTime));
                        var leftDate = Expression.Property(left, "Date");
                        predicate = Expression.Equal(leftDate, right);
                    }
                    else if (property.PropertyType.IsEnum || (Nullable.GetUnderlyingType(property.PropertyType)?.IsEnum ?? false))
                    {
                        if ((int)value > 0)
                        {
                            var right = Expression.Constant(value);
                            predicate = Expression.Equal(left, right);
                        }
                        else
                        {
                            var enumValue = Enum.ToObject(property.PropertyType.IsEnum ? property.PropertyType : Nullable.GetUnderlyingType(property.PropertyType)!, Math.Abs((int)value));
                            var right = Expression.Constant(enumValue);
                            predicate = Expression.NotEqual(left, right);
                        }
                    }
                    else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    {
                        var right = Expression.Constant(Math.Abs((int)value));
                        if (entityProperty!.PropertyType == typeof(int?))
                            right = Expression.Constant(Math.Abs((int)value), typeof(int?));
                        if ((int)value > 0)
                            predicate = Expression.Equal(left, right);
                        else
                            predicate = Expression.NotEqual(left, right);
                    }
                    else
                    {
                        var right = Expression.Constant(value);
                        predicate = Expression.Equal(left, right);
                    }
                    predicates.Add(predicate);
                }
            }

            if (predicates.Count == 0)
            {
                return Enumerable.Empty<TDto>();
            }

            var body = predicates.Aggregate(Expression.AndAlso);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            var results = repository.Search(lambda);
            return mapper.Map<IEnumerable<TDto>>(results);
        }



        private static bool IsNullableType(Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public virtual TDto Update(int id, TDto entity)
        {
            entity.Id = id;
            return mapper.Map<TDto>(repository.Update(mapper.Map<TEntity>(entity)));
        }
    }
}
