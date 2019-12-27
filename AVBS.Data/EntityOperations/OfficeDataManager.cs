using System; 
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AVBS.Entity;
using AVBS.Entity.Abstract; 

namespace AVBS.Data.EntityOperations
{
    public class OfficeDataManager<TEntity> : DataManager<TEntity> where TEntity : class, IOfficeEntity, new( ) {
        

        public override bool Update( int updateduserid, params TEntity[] items ) {
            var returnVal = true;

            foreach ( TEntity entity in items ) {
                entity.IsUpdated = true;
                entity.UpdatedAt = DateTime.Now;

                entity.UpdateUserId = updateduserid;
                _db.Entry( entity ).State = EntityState.Modified;
                _db.Entry( entity ).Property( x => x.CreatedAt ).IsModified = false;
                _db.Entry( entity ).Property( x => x.CreateUserId ).IsModified = false;
                _db.Entry( entity ).Property( x => x.OfficeId ).IsModified = false;
            }

            try {
                _db.SaveChanges( );
            } catch ( Exception e ) {
                returnVal = false;
            }

            return returnVal;
        }

    }
}