using Chat.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Code.Base
{
    public class EntityCreateDate : Entity, IHaveCreatedTime
    {
        public DateTime? CreatedTime { get ; set; }
    }
}
