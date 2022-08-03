using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IBaseRepository<TEntity>
    {
        List<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        List<TEntity> ListAll();

        TEntity FindByCondition(Expression<Func<TEntity, bool>> predicate);

        TEntity GetById(long id);

        TEntity GetById(int id);

        TEntity GetById(byte id);

        TEntity Create(TEntity entity);
    }
}
