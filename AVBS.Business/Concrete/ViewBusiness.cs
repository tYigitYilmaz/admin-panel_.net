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
    public class ViewBusiness<TEntity> : BaseViewBusiness<TEntity>, IViewBusiness<TEntity>
            where TEntity : class, IOfficeViewEntity, new() {


        public ViewBusiness ( AVBS_User loggedUser ) : base(loggedUser) {
         
        }

        public override IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null ) {

            var returnQuery = DataManager.GetAllQueryable( x => x.OfficeId == _loggedUser.OfficeId );

            if ( pregs != null ) returnQuery = returnQuery.Where( pregs );

            return returnQuery;
            
        }
    }
}