using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVBS.View.Abstract {
    public interface IOfficeViewEntity : IViewEntity {
        
        int OfficeId { get; set; }
    }
}
