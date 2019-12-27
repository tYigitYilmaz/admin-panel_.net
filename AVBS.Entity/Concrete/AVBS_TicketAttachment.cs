using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.TicketAttachment", Schema = "dbo" )]
    public class AVBS_TicketAttachment : BaseOfficeEntity {

        public string Url { get; set; }
        public int TicketId { get; set; }

        [ForeignKey( "OfficeId" )]
        public virtual AVBS_Office AVBS_Office { get; set; }

        [ForeignKey( "TicketId" )]
        public virtual AVBS_Ticket AVBS_Ticket { get; set; }

    }
}
