using System;

namespace AVBS.Entity.Abstract {
    public abstract class BaseEntity : IEntity {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreateUserId { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeleteUserId { get; set; }
         
    }
}
