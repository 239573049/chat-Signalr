using System;

namespace Chat.Core.Base
{
    public class EntityWithSoftDelete : IHaveDeletion
    {
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}