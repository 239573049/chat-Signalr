using System;

namespace Chat.Core.Base
{
    public interface IHaveDeletion : IHaveDeleteTime, ISoftDelete
    {
        Guid? DeletedBy { get; set; }
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }

    public interface IHaveDeleteTime
    {
        DateTime? DeletedTime { get; set; }
    }
}