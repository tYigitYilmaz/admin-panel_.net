using System;
using System.Linq;
using System.Linq.Expressions;

namespace AVBS.Business.Abstract {
    public interface IBaseViewBusiness<TEntity> {
        IQueryable<TEntity> GetAllQueryable( Expression<Func<TEntity, bool>> pregs = null );
    }
}