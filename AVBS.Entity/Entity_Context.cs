using System.Data.Entity;
using AVBS.Entity.Concrete;
using AVBS.Entity.References;

namespace AVBS.Entity
{
    public class AVBSEntities : DbContext {
        private readonly string _schema;
        public AVBSEntities ( string schema )
          : base( "AVBSEntities" ) {
            this._schema = schema;
        }
        
        protected override void OnModelCreating ( DbModelBuilder builder ) {
            //builder.HasDefaultSchema( this._schema );
            //base.OnModelCreating( builder );
            Database.SetInitializer<AVBSEntities>( null );
            base.OnModelCreating( builder );
        }


        public DbSet<AVBS_Client> AVBS_Client { get; set; }
        public DbSet<AVBS_Court> AVBS_Court { get; set; }
        public DbSet<AVBS_File> AVBS_File { get; set; }
        public DbSet<AVBS_FileAdviseParty> AVBS_FileAdviseParty { get; set; }
        public DbSet<AVBS_FileAppeal> AVBS_FileAppeal { get; set; }
        public DbSet<AVBS_FileClients> AVBS_FileClients { get; set; }
        public DbSet<AVBS_FileDecree> AVBS_FileDecree { get; set; }
        public DbSet<AVBS_FileIstinaf> AVBS_FileIstinaf { get; set; }
        public DbSet<AVBS_FileObjectionAppeal> AVBS_FileObjectionAppeal { get; set; }
        public DbSet<AVBS_Office> AVBS_Office { get; set; }
        public DbSet<AVBS_User> AVBS_User { get; set; }
        public DbSet<AVBS_Ticket> AVBS_Ticket { get; set; }
        public DbSet<AVBS_TicketAttachment> AVBS_TicketAttachment { get; set; }

        #region References

        public DbSet<AVBS_CaseSideTypes> AVBS_CaseSideTypes { get; set; }
        public DbSet<AVBS_CourtTypes> AVBS_CourtTypes { get; set; }
        public DbSet<AVBS_FileStatuses> AVBS_FileStatuses { get; set; }
        public DbSet<AVBS_FileTypes> AVBS_FileTypes { get; set; }
        #endregion
    }
}
