using System; 
using AVBS.Entity.Concrete;

namespace AVBS.Web.UI.Models {
    public class CookieObject {
        public int UserId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Token { get; set; }
        public AVBS_User UserModel { get; set; }
    }
}