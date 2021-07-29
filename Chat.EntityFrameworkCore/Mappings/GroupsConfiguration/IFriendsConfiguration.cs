using Chat.Code.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Chat.EntityFrameworkCore.Mappings.GroupsConfiguration
{
    public class IFriendsConfiguration : EntityConfiguration<Friends>
    {
        public override void Configure(EntityTypeBuilder<Friends> builder)
        {
            builder.ToTable("Friends");
            base.Configure(builder);
        }
    }
}
