using System;

namespace AVBS.Entity.Abstract {
    public abstract class BaseOfficeReferenceEntity : BaseOfficeEntity, IOfficeReferenceEntity {
        public string Name { get; set; } 
         
    }
}
