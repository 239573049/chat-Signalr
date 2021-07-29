using Chat.Code.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Mappings.GroupsConfiguration
{
    public class IGroupMembersConfiguration : EntityConfiguration<GroupMembers>
    {
        public override void Configure(EntityTypeBuilder<GroupMembers> builder)
        {
            builder.ToTable("GroupMembers");
            base.Configure(builder);
        }
    }
}
