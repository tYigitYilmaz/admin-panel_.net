using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AVBS.View;
using AVBS.View.Abstract;

namespace AVBS.Data.ViewOperations {
    public class ViewDataManager<TEntity> where TEntity : class, IViewEntity, new( ) {

        private readonly IDbSet<TEntity> _dbset;
      

        public ViewDataManager () {
            var context = new DbView("dbo");
            _dbset = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null ) {
            return pregs != null ? _dbset.AsNoTracking().Where( pregs ) : _dbset.AsNoTracking();
        }

    }
}