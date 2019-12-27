using System.ComponentModel.DataAnnotations;
using AVBS.Enum;
using AVBS.Office.UI.Helper;

namespace AVBS.Office.UI.Models.User {
    public class PasswordModel {

        [Required]
        public int UserId { get; set; }

        [StringLength( 100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8 )]
        [Display( Name = "Password" )]
        [RegularExpression( UserHelper.PasswordRegex, ErrorMessage = Messages.PasswordError )]
        public string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm password" )]
        [Compare( "Password", ErrorMessage = "The passwords do not match." )]
        public string PasswordRepeat { get; set; }

        [Required]
        public string UserFullName { get; set; }

    }
}