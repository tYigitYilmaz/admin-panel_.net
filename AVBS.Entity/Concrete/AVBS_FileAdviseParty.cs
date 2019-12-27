using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.FileAdviseParty", Schema = "dbo" )]
    public class AVBS_FileAdviseParty : BaseOfficeEntity {

        public string Name { get; set; } 
        public string Identity { get; set; }
        public string AdvocateName { get; set; }
        public string AdvocatePhone { get; set; }
        public string Description { get; set; }
        public int FileId { get; set; } 

        [ForeignKey( "FileId" )]
        public virtual AVBS_File AVBS_File { get; set; }

    }
}
