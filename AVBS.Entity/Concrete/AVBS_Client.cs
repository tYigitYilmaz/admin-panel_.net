using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.Client", Schema = "dbo" )]
    public class AVBS_Client : BaseOfficeEntity {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Identity { get; set; }

        [ForeignKey( "OfficeId" )]
        public virtual AVBS_Office AVBS_Office { get; set; }


        [NotMapped]
        public string FullName => Name + " " + Surname;
    }
}
