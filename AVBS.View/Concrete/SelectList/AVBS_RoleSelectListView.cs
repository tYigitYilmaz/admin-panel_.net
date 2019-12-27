using System;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.View.Abstract; 

namespace AVBS.View.Concrete.Table {
    [Serializable]
    [Table( "AVBS.RoleSelectListView", Schema = "dbo" )]
    public class AVBS_RoleSelectListView : IViewEntity {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
