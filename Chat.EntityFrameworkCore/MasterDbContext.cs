using Microsoft.EntityFrameworkCore;
using Chat.Core.Base;
using System.Linq.Expressions;
using System.Reflection;
namespace Chat.EntityFrameworkCore
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)) {
                    modelBuilder.Entity(entityType.ClrType).Property<bool>("IsDeleted");
                    var parameter = Expression.Parameter(entityType.ClrType, "del");
                    var body = Expression.Equal(
                        Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDeleted")),
                    Expression.Constant(false));
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
                }
            }
        }
    }
}
