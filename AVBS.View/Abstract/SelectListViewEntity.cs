using System;

namespace AVBS.View.Abstract {
    public abstract class SelectListViewEntity : ISelectListViewEntity {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
