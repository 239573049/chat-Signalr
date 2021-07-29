using System;

namespace Chat.Core.Base
{
    public interface IHaveModification : IHaveModifiedTime
    {
        Guid? ModifiedBy { get; set; }
    }

    public interface IHaveModifiedTime
    {
        DateTime? ModifiedTime { get; set; }
    }
}