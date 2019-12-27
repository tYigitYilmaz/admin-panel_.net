
namespace AVBS.Admin.UI.Models.Analytic {
    public class AmountBySubOutgoingTypeModel {

        public AmountBySubOutgoingTypeModel () {
        }

        public AmountBySubOutgoingTypeModel ( int len ) {
            Amount = new decimal[len];
            XAxis = new string[len];
        }
         
        public decimal[] Amount { get; set; }
        public string[] XAxis { get; set; }

    }
}