using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AVBS.Business.Abstract;
using AVBS.Data.EntityOperations;
using AVBS.Entity.Abstract;
using AVBS.Entity.Concrete;

namespace AVBS.Business.Concrete {
    public class BaseBusiness<TEntity> : IBaseBusiness <TEntity>
        where TEntity : class, IEntity, new() {
         
        protected DataManager<TEntity> DataManager;
        protected AVBS_User _loggedUser;
        protected AVBS_File _file;

        public BaseBusiness( ) {
            DataManager = new DataManager<TEntity>();
        }

        public BaseBusiness ( AVBS_User loggedUser ) {
            DataManager = new DataManager<TEntity>();
            _loggedUser = loggedUser;
        }

        public BaseBusiness ( AVBS_User loggedUser, AVBS_File file ) {
            DataManager = new DataManager<TEntity>();
            _loggedUser = loggedUser;
            _file = file;
        }

        public virtual TEntity Get ( int id ) {
            var entity = DataManager.Get( id );

            return entity;
        }

        public virtual TEntity GetAsNoTracking ( int id ) {
            var entity = DataManager.GetAsNoTracking( id ); 

            return entity;
        }

        public virtual IEnumerable<TEntity> GetAllEnumerable ( Expression<Func<TEntity, bool>> pregs = null ) {
            var query = pregs != null ? DataManager.GetAllQuaryable( pregs ) : DataManager.GetAll();

            var list = query.AsEnumerable();

            return list;
        }

        public virtual IQueryable<TEntity> GetAllQueryable ( Expression<Func<TEntity, bool>> pregs = null ) {
            var query = pregs != null ? DataManager.GetAllQuaryable( pregs ) : DataManager.GetAll( );

            return query;
        }

        //public virtual bool Add ( TEntity entity ) { 
        //    return DataManager.Create( _loggedUser.Id, entity );
        //}

        //public virtual bool AddBulk (List<TEntity> model)
        //{
        //    return DataManager.Create( _loggedUser.Id, model.ToArray());

        //}

        //public virtual int AddWithReturn ( TEntity entity )
        //{ 
        //    return DataManager.CreateWithReturn( _loggedUser.Id, entity );
        //}

        //public virtual TEntity AddWithReturnEntity ( TEntity entity )
        //{ 
        //    var id = DataManager.CreateWithReturn( _loggedUser.Id, entity );
        //    return DataManager.GetAsNoTracking( id );
        //}

        //public virtual bool Update ( TEntity entity ) { 

        //    return DataManager.Update( _loggedUser.Id, entity );

        //}

        //public virtual bool UpdateBulk ( List<TEntity> model ) {
        //    return DataManager.Update( _loggedUser.Id, model.ToArray() );

        //}

        //public virtual bool RemoveBulk (int[] list)
        //{
        //    return DataManager.Remove( _loggedUser.Id, list );

        //}

        //public virtual bool Remove ( int id ) {

        //    return DataManager.Remove( _loggedUser.Id, id );
        //}

    }
}