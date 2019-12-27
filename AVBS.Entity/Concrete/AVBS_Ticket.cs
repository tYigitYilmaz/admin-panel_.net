using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.Ticket", Schema = "dbo" )]
    public class AVBS_Ticket : BaseOfficeEntity {

        public string QuestionHeader { get; set; }
        public string QuestionText { get; set; }
        public DateTime QuestionDate { get; set; }
        public int ClientId { get; set; }
        public bool IsAssigned { get; set; }
        public int? AssignedUserId { get; set; }
        public bool IsAnswered { get; set; }
        public int? AnsweredUserId { get; set; }
        public string AnswerText { get; set; }
        public DateTime AnswerDate { get; set; }

        [ForeignKey( "OfficeId" )]
        public virtual AVBS_Office AVBS_Office { get; set; }

        [ForeignKey( "ClientId" )]
        public virtual AVBS_Client AVBS_Client { get; set; }

        [ForeignKey( "AssignedUserId" )]
        public virtual AVBS_User AVBS_AssignedUser { get; set; }

        [ForeignKey( "AnsweredUserId" )]
        public virtual AVBS_User AVBS_AnsweredUser { get; set; }


    }
}
