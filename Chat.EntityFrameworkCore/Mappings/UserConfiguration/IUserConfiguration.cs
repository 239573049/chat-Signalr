using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Chat.Code.Entities.Users;

namespace Chat.EntityFrameworkCore.Mappings.UserConfiguration
{
    public class IUserConfiguration : EntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            base.Configure(builder);
        }
    }
}
