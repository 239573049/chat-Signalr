using Chat.Code.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Chat.EntityFrameworkCore.Mappings.GroupsConfiguration
{
    public class IGroupDataConfiguration : EntityConfiguration<GroupData>
    {
        public override void Configure(EntityTypeBuilder<GroupData> builder)
        {
            builder.ToTable("GuroupData");
            base.Configure(builder);
        }
    }
}
