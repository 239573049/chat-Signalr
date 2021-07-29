using System;

namespace Chat.Core.Base
{
    public class EntityWithModification : IHaveModification
    {
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}