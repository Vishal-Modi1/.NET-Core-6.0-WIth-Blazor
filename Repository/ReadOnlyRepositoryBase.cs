using DataModels.VM.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class ReadOnlyRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly MyContext _dbContext;

		public ReadOnlyRepositoryBase(MyContext dbContext)
		{
			this._dbContext = dbContext;
		}

		public List<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
		{
			return _dbContext.Set<TEntity>().Where(predicate).ToList(); ;
		}

		public List<TEntity> ListAll()
		{
			return _dbContext.Set<TEntity>().ToList(); 
		}

		public TEntity GetById(long id)
		{
			TEntity entity = _dbContext.Set<TEntity>().Find(id);
			if (entity != null)
			{
				_dbContext.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public TEntity GetById(int id)
		{
			TEntity entity = _dbContext.Set<TEntity>().Find(id);
			if (entity != null)
			{
				_dbContext.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public TEntity GetById(byte id)
		{
			TEntity entity = _dbContext.Set<TEntity>().Find(id);
			if (entity != null)
			{
				_dbContext.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public TEntity FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
			TEntity entity = _dbContext.Set<TEntity>().Where(predicate).FirstOrDefault();

			if (entity != null)
			{
				_dbContext.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}
    }
}
