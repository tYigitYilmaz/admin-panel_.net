
namespace AVBS.Admin.UI.Models.Analytic {
    public class AmountByOutgoingTypeModel {

        public AmountByOutgoingTypeModel () {
        }

        public AmountByOutgoingTypeModel ( int len ) {
            Amount = new decimal[len];
            XAxis = new string[len];
        }
         
        public decimal[] Amount { get; set; }
        public string[] XAxis { get; set; }

    }
}