using System;
using System.Linq;
using System.Linq.Expressions;
using AVBS.Business.Abstract;
using AVBS.Data.EntityOperations;
using AVBS.Data.ViewOperations;
using AVBS.Entity.Abstract;
using AVBS.Entity.Concrete;
using AVBS.View.Abstract;

namespace AVBS.Business.Concrete {
    public class BaseViewBusiness<TEntity> : IViewBusiness<TEntity>
        where TEntity : class, IViewEntity, new() {

        protected readonly AVBS_User _loggedUser;
        protected readonly ViewDataManager<TEntity> DataManager;

        public BaseViewBusiness ( AVBS_User loggedUser ) {
            DataManager = new ViewDataManager<TEntity>();
            _loggedUser = loggedUser;
        }

        public virtual IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null ) {

            return DataManager.GetAllQueryable( pregs ); ;
            
        }
    }
}