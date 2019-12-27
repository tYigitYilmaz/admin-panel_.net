using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileIstinaf", Schema = "dbo" )]
    public class AVBS_FileIstinaf : BaseOfficeEntity {
         
        public string MeritsNo { get; set; } // Esas No
        public int DecreeNo { get; set; } 

        public int FileId { get; set; } 

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
