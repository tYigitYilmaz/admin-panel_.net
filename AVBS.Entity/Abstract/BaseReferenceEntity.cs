using System;

namespace AVBS.Entity.Abstract {
    public abstract class BaseReferenceEntity : BaseEntity, IReferenceEntity {
        public string Name { get; set; }
         
    }
}
