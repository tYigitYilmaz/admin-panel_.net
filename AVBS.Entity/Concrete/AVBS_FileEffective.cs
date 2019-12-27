using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileEffective", Schema = "dbo" )]
    public class AVBS_FileEffective : BaseOfficeEntity { // DOSYA KESİNLEŞME
         
        public DateTime Date { get; set; }
        public string DecreeSummary { get; set; } // Karar özeti

        public int FileId { get; set; } 

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
