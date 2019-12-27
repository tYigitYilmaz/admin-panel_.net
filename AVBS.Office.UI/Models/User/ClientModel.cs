using System.ComponentModel.DataAnnotations;
using AVBS.Enum;
using AVBS.Infrastructure.Validation;
using AVBS.Office.UI.Helper;

namespace AVBS.Office.UI.Models.User {
    public class ClientModel {
        public int Id { get; set; }
        [Required]
        [StringLength( 50, ErrorMessage = Messages.MaxLenError )]
        public string Name { get; set; }
        [Required]
        [StringLength( 50, ErrorMessage = Messages.MaxLenError )]
        public string Surname { get; set; }
        [EmailAddress]
        [StringLength( 50, ErrorMessage = Messages.MaxLenError )]
        public string Email { get; set; }
        [Required]
        [StringLength( 11, ErrorMessage = Messages.MaxLenError )]
        [Identity]
        public string Identity { get; set; }
        [Required]
        [StringLength( 50, ErrorMessage = Messages.MaxLenError )]
        public string Phone { get; set; }

        [StringLength( 100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8 )]
        [Display( Name = "Password" )]
        [RegularExpression( UserHelper.PasswordRegex , ErrorMessage =  Messages.PasswordError )]
        public string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm password" )]
        [Compare( "Password", ErrorMessage = "The passwords do not match." )]
        public string PasswordRepeat { get; set; }
  

    }
}