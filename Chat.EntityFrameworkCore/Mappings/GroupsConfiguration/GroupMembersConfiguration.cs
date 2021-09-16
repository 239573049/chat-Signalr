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
    public class GroupMembersConfiguration : EntityConfiguration<GroupMembers>
    {
        public override void Configure(EntityTypeBuilder<GroupMembers> builder)
        {
            builder.ToTable("GroupMembers");
            builder.HasOne(a => a.Self).WithMany().HasForeignKey(a => a.SelfId);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            base.Configure(builder);
        }
    }
}
