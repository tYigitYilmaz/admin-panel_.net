 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AVBS.Business.Abstract {
    public interface IBusiness<TEntity>{
        TEntity Get ( int id );
        TEntity GetAsNoTracking ( int id );
        IEnumerable<TEntity> GetAllEnumerable ( Expression<Func<TEntity, bool>> pregs = null );
        IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null );
        bool Add ( TEntity model );
        bool AddBulk ( List<TEntity> model );
        int AddWithReturn ( TEntity model );
        TEntity AddWithReturnEntity ( TEntity model );
        bool Update ( TEntity model );
        bool UpdateBulk ( List<TEntity> model );
        bool Remove ( int id );
        bool RemoveBulk ( int[] list );
    }
}