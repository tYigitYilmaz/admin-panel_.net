using System; 
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AVBS.Entity;
using AVBS.Entity.Abstract; 

namespace AVBS.Data.EntityOperations
{
    public class DataManager<TEntity> where TEntity : class, IEntity, new()
    {

        protected readonly AVBSEntities _db = new AVBSEntities( "dbo");

        public virtual TEntity Get(int id)
        {
            TEntity entity = _db.Set<TEntity>().FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            return entity;
        }

        public virtual TEntity GetAsNoTracking (int id)
        {
            TEntity entity = _db.Set<TEntity>().AsNoTracking().FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            return entity;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> list = _db.Set<TEntity>().Where(x => !x.IsDeleted);

            return list;
        }

        public virtual IQueryable<TEntity> GetAllQuaryable(Expression<Func<TEntity, bool>> pregs = null)
        {
            IQueryable<TEntity> list = _db.Set<TEntity>().AsNoTracking().Where(pregs).Where(x => !x.IsDeleted);

            return list;
        }

        public virtual bool Create (int createduserid, params TEntity[] items )
        {
            var returnVal = true;

            foreach (TEntity entity in items)
            {
                entity.CreatedAt = DateTime.Now;
                entity.CreateUserId = createduserid;

                _db.Entry(entity).State = EntityState.Added;
            }
            try {
                _db.SaveChanges();
            } catch ( Exception e ) {
                returnVal = false;
            }

            return returnVal;
        }

        public virtual int CreateWithReturn (int createduserid, TEntity item)
        {
            item.CreatedAt = DateTime.Now;
            item.CreateUserId = createduserid;

            _db.Entry(item).State = EntityState.Added;
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }

            return item.Id;
        }


        public virtual bool Update ( int updateduserid, params TEntity[] items)
        {
            var returnVal = true;

            foreach (TEntity entity in items)
            {
                entity.IsUpdated = true;
                entity.UpdatedAt = DateTime.Now;

				entity.UpdateUserId = updateduserid;
				_db.Entry(entity).State = EntityState.Modified;
	            _db.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
	            _db.Entry(entity).Property(x => x.CreateUserId).IsModified = false;

            }

            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                returnVal = false;
            }

            return returnVal;
        }

        //public bool Remove(Expression<Func<TEntity, bool>> pregs = null)
        //{
        //    IQueryable<TEntity> list = _db.Set<TEntity>().Where(pregs);

        //    return list;
        //}

        public virtual bool Remove ( int deleteduserid, params int[] items)
        {
            var returnVal = true;

            foreach (var id in items)
            {
                var entity = Get(id);
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeleteUserId = deleteduserid;
                _db.Entry(entity).State = EntityState.Modified;
            }

            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                returnVal = false;
            }

            return returnVal;
        }
    }
}