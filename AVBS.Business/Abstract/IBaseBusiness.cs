 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AVBS.Business.Abstract {
    public interface IBaseBusiness<TEntity> {
        TEntity Get ( int id );
        TEntity GetAsNoTracking ( int id );
        IEnumerable<TEntity> GetAllEnumerable ( Expression<Func<TEntity, bool>> pregs = null );
        IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null );
    }
}