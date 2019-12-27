using AVBS.Business.Concrete;
using AVBS.Entity.Concrete;
using AVBS.Office.UI.Models.User;

namespace AVBS.Office.UI.Helper {
    public static class UserHelper {

        public const string PasswordRegex =
            "^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$";

        public static UserModel ConvertToModel ( AVBS_User entity ) {
            var model = new UserModel() {
                ImageUrl = entity.ImageUrl,
                Name = entity.Name,
                Email = entity.Email,
                Surname = entity.Surname,  
                Identity = entity.Identity,
                Phone = entity.Phone,
                Id = entity.Id,
                RoleId = entity.RoleId
            };

            return model;
        }


        public static AVBS_User ConvertToEntity ( UserModel model, AVBS_User entity ) {

            if ( entity == null ) {
                entity = new AVBS_User( );
            }

            entity.Email = model.Email;
            entity.ImageUrl = model.ImageUrl;
            entity.Identity = model.Identity;
            entity.Name = model.Name;
            entity.Surname = model.Surname;
            entity.Phone = model.Phone;
            entity.RoleId = model.RoleId;
            entity.Id = model.Id;

            return entity;
        }


        public static bool PasswordCheck(string password, string passwordRepeat ) {
            return password != passwordRepeat;
        }

        public static UserSessionModel ConvertToSessionModel ( AVBS_User entity ) {
            return new UserSessionModel() { Email = entity.Email, Name = entity.Name, Surname = entity.Surname, Id = entity.Id, ImageUrl = entity.ImageUrl, RoleId  =  entity.RoleId, OfficeId = entity.OfficeId};
        }
    }
}

