using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract; 

namespace AVBS.Entity.Concrete {
    [Serializable]
    [Table( "AVBS.Office", Schema = "dbo" )]
    public class AVBS_Office : BaseEntity {

        public string Name { get; set; }
        public string Domain { get; set; }
    }
}
