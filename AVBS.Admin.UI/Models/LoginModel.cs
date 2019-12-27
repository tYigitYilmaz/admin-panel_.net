using System;
using System.Web;
using AVBS.Entity.Concrete;

namespace AVBS.Web.UI.Models {
    public class LoginModel {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int CardId { get; set; }
        public int CompanyId { get; set; }
        public string Phone { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }

        public HttpPostedFileBase File { get; set; }

        public string ImageUrl { get; set; }
    }
}