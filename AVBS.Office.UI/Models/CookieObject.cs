using System; 
using AVBS.Entity.Concrete;
using AVBS.Office.UI.Models.User;

namespace AVBS.Office.UI.Models {
    public class CookieObject {
        public int UserId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Token { get; set; }
        public UserSessionModel UserModel { get; set; }
    }
}