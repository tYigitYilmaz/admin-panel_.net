using System.Data.Entity;
using AVBS.View.Concrete.Table;

namespace AVBS.View
{
    public class DbView : DbContext
    {
        private readonly string _schema;
        public DbView( string schema )
            : base("name=DbView") {
            this._schema = schema;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            Database.SetInitializer<DbView>( null );
            modelBuilder.HasDefaultSchema( this._schema );
            base.OnModelCreating( modelBuilder );
        }
        


        #region Analytics
        

        #endregion

        #region SelectList
        
        public virtual DbSet<AVBS_RoleSelectListView> AVBS_RoleSelectListView { get; set; }

        #endregion

        #region Table
        
        public virtual DbSet<AVBS_PersonnelTableView> AVBS_SubOutgoingTypeTableView { get; set; }

        #endregion
    }
}
