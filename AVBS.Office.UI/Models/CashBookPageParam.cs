using System;

namespace AVBS.Office.UI.Models {
    public class CashBookPageParam {
        public int PeriodType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}