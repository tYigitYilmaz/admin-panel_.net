using System;
using System.Linq;
using System.Linq.Expressions;

namespace AVBS.Business.Abstract {
    public interface IViewBusiness<TEntity> {
        IQueryable<TEntity> GetAllQueryable( Expression<Func<TEntity, bool>> pregs = null );
    }
}