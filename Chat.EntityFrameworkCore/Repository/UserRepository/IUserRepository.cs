using Chat.Code.Entities.Users;
namespace Chat.EntityFrameworkCore.Repository.UserRepository
{

    public interface IUserRepository : IMasterDbRepositoryBase<User>
    {
    }

    public class UserRepository : MasterDbRepositoryBase<User>, IUserRepository
    {
        public UserRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
