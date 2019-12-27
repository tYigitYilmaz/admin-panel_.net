using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVBS.Entity.Abstract;

namespace AVBS.Entity.References {
    [Serializable]
    [Table( "AVBS.FileTypes", Schema = "dbo" )]
    public class AVBS_FileTypes : BaseOfficeReferenceEntity {

    }
}
