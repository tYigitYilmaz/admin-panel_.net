using System;
using System.Collections.Generic;
using System.Web;

namespace AVBS.Office.UI.Models.Ticket {
    public class TicketAttachmentModel {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public List<TicketAttachmentModel> TicketAttachmentModelList = new List<TicketAttachmentModel>();

        public HttpPostedFileBase File { get; set; }
        public string Url { get; set; }
        

    }
}