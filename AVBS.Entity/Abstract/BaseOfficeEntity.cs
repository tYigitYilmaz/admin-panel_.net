using System;

namespace AVBS.Entity.Abstract {
    public abstract class BaseOfficeEntity : BaseEntity, IOfficeEntity {

        public int OfficeId { get; set; }
         
    }
}
