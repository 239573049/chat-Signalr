using Chat.Code.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Chat.EntityFrameworkCore.Mappings.GroupsConfiguration
{
    public class FriendsConfiguration : EntityConfiguration<Friends>
    {
        public override void Configure(EntityTypeBuilder<Friends> builder)
        {
            builder.ToTable("Friends");
            builder.HasOne(a => a.Self).WithMany().HasForeignKey(a => a.SelfId);
            builder.HasOne(a => a.Friend).WithMany().HasForeignKey(a => a.FriendId);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            base.Configure(builder);
        }
    }
}
