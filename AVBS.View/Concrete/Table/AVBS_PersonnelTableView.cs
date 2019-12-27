using System;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.View.Abstract; 

namespace AVBS.View.Concrete.Table {
    [Serializable]
    [Table( "AVBS.PersonnelTableView", Schema = "dbo" )]
    public class AVBS_PersonnelTableView : OfficeViewEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
