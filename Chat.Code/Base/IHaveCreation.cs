using System;

namespace Chat.Core.Base
{
    public interface IHaveCreation : IHaveCreatedTime
    {
        Guid? CreatedBy { get; set; }
    }

    public interface IHaveCreatedTime
    {
        DateTime? CreatedTime { get; set; }
    }
}