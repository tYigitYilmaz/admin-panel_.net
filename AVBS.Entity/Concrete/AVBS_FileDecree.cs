using System;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileDecree", Schema = "dbo" )]
    public class AVBS_FileDecree : BaseOfficeEntity {
         
        public int No { get; set; }
        public string Summary { get; set; } 

        public int FileId { get; set; } 

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
