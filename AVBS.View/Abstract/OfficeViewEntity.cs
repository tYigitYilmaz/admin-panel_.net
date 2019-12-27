using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVBS.View.Abstract {
    public abstract class OfficeViewEntity : IOfficeViewEntity {
        public int OfficeId { get; set; }
    }
}
