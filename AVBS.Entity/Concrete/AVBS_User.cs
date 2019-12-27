using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract;
using AVBS.Entity.References;

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.User", Schema = "dbo" )]
    public class AVBS_User : BaseOfficeEntity {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Identity { get; set; }

        [ForeignKey( "OfficeId" )]
        public virtual AVBS_Office AVBS_Office { get; set; }

        [ForeignKey( "RoleId" )]
        public virtual AVBS_Roles Role { get; set; }


        [NotMapped]
        public string FullName => Name + " " + Surname;
    }
}
