using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IBaseRepository<TEntity>
    {
        abstract List<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        abstract List<TEntity> ListAll();

        abstract TEntity FindByCondition(Expression<Func<TEntity, bool>> predicate);

        abstract TEntity GetById(long id);

        abstract TEntity GetById(int id);

        abstract TEntity GetById(byte id);

        abstract TEntity Create(TEntity entity);
    }
}
 