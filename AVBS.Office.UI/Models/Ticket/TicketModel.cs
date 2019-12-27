using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using AVBS.Enum;

namespace AVBS.Office.UI.Models.Ticket {
    public class TicketModel {
        public int Id { get; set; }

        [Required]
        [StringLength( 255, ErrorMessage = Messages.MaxLenError )]
        public string QuestionHeader { get; set; }
        [Required]
        [StringLength( 4000, ErrorMessage = Messages.MaxLenError )]
        public string QuestionText { get; set; }
        [Required]
        public DateTime QuestionDate { get; set; }
        [Required]
        public int ClientId { get; set; }
        
        public bool IsAssigned { get; set; }
        public int? AssignedUserId { get; set; }
        public bool IsAnswered { get; set; }
        public int? AnsweredUserId { get; set; }

        public List<TicketAttachmentModel> TicketAttachmentModelList = new List<TicketAttachmentModel>();

        public string AnswerText { get; set; }
        public DateTime AnswerDate { get; set; }


        public string ClientName { get; set; }
        public string AssignedUserName { get; set; }
        public string AnsweredUserName { get; set; }
    }
}