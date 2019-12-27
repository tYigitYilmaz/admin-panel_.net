using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract;
using AVBS.Entity.References;

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.Court", Schema = "dbo" )]
    public class AVBS_Court : BaseOfficeEntity {

        public string Name { get; set; } 
        public int CourtTypeId { get; set; } 

        [ForeignKey( "CourtTypeId" )]
        public virtual AVBS_CourtTypes AVBS_CourtTypes { get; set; }

        [ForeignKey( "OfficeId" )]
        public virtual AVBS_Office AVBS_Office { get; set; }

    }
}
