using Microsoft.EntityFrameworkCore;
using TreeService.Entities;

namespace TreeService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TreeNode> TreeNodes => Set<TreeNode>();
        public DbSet<User> Users => Set<User>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreeNode>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
             
        }
    }
}
