using System; 
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileAppeal", Schema = "dbo" )]
    public class AVBS_FileAppeal : BaseOfficeEntity {

        public string MeritsNo { get; set; } 
        public string DecreeNo { get; set; }
        public DateTime Date { get; set; }
        public string Petition { get; set; } // Dİlekçe Url

        public int FileId { get; set; } 

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
