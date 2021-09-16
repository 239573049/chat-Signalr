using Chat.Code.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Chat.EntityFrameworkCore.Mappings.GroupsConfiguration
{
    public class GroupDataConfiguration : EntityConfiguration<GroupData>
    {
        public override void Configure(EntityTypeBuilder<GroupData> builder)
        {
            builder.ToTable("GuroupData");
            builder.HasOne(a => a.Self).WithMany().HasForeignKey(a => a.SelfId);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            base.Configure(builder);
        }
    }
}
