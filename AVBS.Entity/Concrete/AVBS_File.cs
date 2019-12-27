using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract;
using AVBS.Entity.References;

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.File", Schema = "dbo" )]
    public class AVBS_File : BaseOfficeEntity {

        public int No { get; set; }
        public int RelatedUserId { get; set; }
        public int FileTypeId { get; set; }
        public int FileStatusId { get; set; }
        public DateTime Date { get; set; }
        public int FolderNo { get; set; }
        public string FileMeritsNo { get; set; }
        public int CourtId { get; set; }
        public int InvestigationNo { get; set; }
        public decimal CounselFee { get; set; }
        public string Description { get; set; }
        public int CaseSideTypeId { get; set; }

        [ForeignKey( "OfficeId" )]
        public virtual AVBS_Office AVBS_Office { get; set; }

        [ForeignKey( "CourtId" )]
        public virtual AVBS_Court AVBS_Court { get; set; }

        [ForeignKey( "FileStatusId" )]
        public virtual AVBS_FileStatuses AVBS_FileStatus { get; set; }
    }
}
