using System;

namespace Chat.Core.Base
{
    public class EntityWithCreation : IHaveCreation
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}