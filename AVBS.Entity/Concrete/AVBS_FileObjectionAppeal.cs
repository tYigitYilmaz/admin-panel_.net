using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileObjectionAppeal", Schema = "dbo" )]
    public class AVBS_FileObjectionAppeal : BaseOfficeEntity {
         
        public DateTime Date { get; set; } 
        public string Description { get; set; } 

        public int FileId { get; set; } 

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
