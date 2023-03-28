using Repository.Interface;

namespace Repository
{
    public class BaseRepository<TEntity> : ReadOnlyRepositoryBase<TEntity>, IBaseRepository<TEntity> where TEntity : class
	{
		private readonly MyContext _dbContext;

		public BaseRepository(MyContext dbContext)
			: base(dbContext)
		{
			_dbContext = dbContext;
		}

        public virtual TEntity Create(TEntity entity)
        {
			_dbContext.Set<TEntity>().Add(entity);
			_dbContext.SaveChanges();

			return entity;
		}
    }
}
