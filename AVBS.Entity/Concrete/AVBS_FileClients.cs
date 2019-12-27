using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileClients", Schema = "dbo" )]
    public class AVBS_FileClients : BaseOfficeEntity {

        public string MeritsNo { get; set; } 

        public int ClientId { get; set; }
        public int FileId { get; set; }

        [ForeignKey( "ClientId" )]
        public virtual AVBS_Client AVBS_Client { get; set; }

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
