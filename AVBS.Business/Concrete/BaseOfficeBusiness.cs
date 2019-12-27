using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AVBS.Business.Abstract;
using AVBS.Data.EntityOperations;
using AVBS.Entity.Abstract;
using AVBS.Entity.Concrete;

namespace AVBS.Business.Concrete {

    // TODO Check office id controlls
    public class BaseOfficeBusiness<TEntity> : BaseBusiness<TEntity>, IBusiness<TEntity>
        where TEntity : class, IOfficeEntity, new() {

        public BaseOfficeBusiness ( AVBS_User loggedUser ) : base( loggedUser ) {
            DataManager = new OfficeDataManager<TEntity>(  );
        }

        public override TEntity Get ( int id ) {
            var entity = DataManager.Get( id );
            if ( entity == null || entity.OfficeId != _loggedUser.OfficeId ) return null;
            return entity;
        }

        public override TEntity GetAsNoTracking ( int id ) {
            var entity = DataManager.GetAsNoTracking( id );
            if ( entity == null || entity.OfficeId != _loggedUser.OfficeId ) return null;
            return entity;
        }

        public override IEnumerable<TEntity> GetAllEnumerable ( Expression<Func<TEntity, bool>> pregs = null ) {
            var query = pregs != null ? DataManager.GetAllQuaryable( pregs ) : DataManager.GetAll();

            var list = query.Where(x=>x.OfficeId == _loggedUser.OfficeId ).AsEnumerable();

            return list;
        }

        public override IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null ) {
            var query = pregs != null ? DataManager.GetAllQuaryable( pregs ) : DataManager.GetAll( );

            return query.Where( x => x.OfficeId == _loggedUser.OfficeId );
        }

        public  bool Add ( TEntity entity ) {

            entity.OfficeId = _loggedUser.OfficeId;

            return DataManager.Create( _loggedUser.Id, entity );
        }

        public  bool AddBulk (List<TEntity> model)
        {
            if ( model.Any(x => x.OfficeId != _loggedUser.OfficeId ) ) return false;

            return DataManager.Create( _loggedUser.Id, model.ToArray() );

        }

        public  int AddWithReturn ( TEntity entity ) {
            entity.OfficeId = _loggedUser.OfficeId;

            return DataManager.CreateWithReturn( _loggedUser.Id, entity );
        }

        public  TEntity AddWithReturnEntity ( TEntity entity )
        {

            entity.OfficeId = _loggedUser.OfficeId;
            var id = AddWithReturn( entity );
            return DataManager.GetAsNoTracking( id );
        }

        public  bool Update ( TEntity entity ) {

            if ( entity.OfficeId != _loggedUser.OfficeId ) return false;

            return DataManager.Update( _loggedUser.Id, entity );

        }

        public  bool UpdateBulk ( List<TEntity> model ) {
            if ( model.Any( x => x.OfficeId != _loggedUser.OfficeId ) ) return false;

            return DataManager.Update( _loggedUser.Id, model.ToArray() );

        }

        // TODO Nasıl kontrol edilecek officeIdler
        public  bool RemoveBulk (int[] list)
        {
            return DataManager.Remove( _loggedUser.Id, list );

        }

        public  bool Remove ( int id ) {

            var entity = Get( id );

            if ( entity == null ) return false;

            return DataManager.Remove( _loggedUser.Id, id );
        }

        
    }
}