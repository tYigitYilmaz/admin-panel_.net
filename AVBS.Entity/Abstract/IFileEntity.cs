using System;

namespace AVBS.Entity.Abstract {
    public interface IFileEntity : IEntity {
        int FileId { get; set; }
    }
}
