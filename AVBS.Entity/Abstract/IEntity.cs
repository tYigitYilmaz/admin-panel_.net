using System;

namespace AVBS.Entity.Abstract {
    public interface IEntity {
        int Id { get; set; }
        DateTime? CreatedAt { get; set; }
        int CreateUserId { get; set; }
        bool IsUpdated { get; set; }
        DateTime? UpdatedAt { get; set; }
        int? UpdateUserId { get; set; }
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        int? DeleteUserId { get; set; }
        
    }
}
