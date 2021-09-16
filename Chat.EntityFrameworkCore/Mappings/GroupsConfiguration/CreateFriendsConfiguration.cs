using Chat.Code.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.EntityFrameworkCore.Mappings.GroupsConfiguration
{
    public class CreateFriendsConfiguration : EntityConfiguration<CreateFriends>
    {
        public override void Configure(EntityTypeBuilder<CreateFriends> builder)
        {
            builder.ToTable("CreateFriends");
            base.Configure(builder);
        }
    }
}
