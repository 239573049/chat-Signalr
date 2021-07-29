using System;

namespace Chat.Core.Base
{
    public class EntityWithAllBaseProperty : Entity, IHaveCreation, IHaveModification, IHaveDeletion
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}