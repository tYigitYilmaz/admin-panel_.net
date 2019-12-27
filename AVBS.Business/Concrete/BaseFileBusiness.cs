using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AVBS.Entity.Abstract;
using AVBS.Entity.Concrete;

namespace AVBS.Business.Concrete {

    // TODO Check file id controlls
    public class BaseFileBusiness<TEntity> : BaseBusiness<TEntity>
        where TEntity : class, IFileEntity, new() {

        public BaseFileBusiness ( AVBS_User loggedUser, AVBS_File file ) : base( loggedUser, file ) {
        }

        public override TEntity Get ( int id ) {
            var entity = DataManager.Get( id );

            return entity.FileId != _file.Id ? null : entity;
        }

        public override TEntity GetAsNoTracking ( int id ) {
            var entity = DataManager.GetAsNoTracking( id );

            return entity.FileId != _file.Id ? null : entity;
        }


        public override IEnumerable<TEntity> GetAllEnumerable ( Expression<Func<TEntity, bool>> pregs = null ) {
            var query = pregs != null ? DataManager.GetAllQuaryable( pregs ) : DataManager.GetAll();

            var list = query.Where( x=>x.FileId == _file.Id).AsEnumerable();

            return list;
        }

        public override IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null ) {
            var query = pregs != null ? DataManager.GetAllQuaryable( pregs ) : DataManager.GetAll( );

            return query.Where( x=> x.FileId == _file.Id );
        }

    }
}