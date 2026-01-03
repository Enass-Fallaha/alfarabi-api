using AlFarabiApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlFarabiApi.Models
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext()
        {
        }

        public  ApplicationDbContext ( DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
     public DbSet<User> Users { get; set; }
     public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectUser> SubjectUsers { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GroupUser>()
                .HasOne(u => u.User)
                .WithMany(o => o.GroupUsers)
                .HasForeignKey(o => o.UserId);

            modelBuilder
                .Entity<GroupUser>()
                .HasOne(u => u.Group)
                .WithMany(o => o.GroupUsers)
                .HasForeignKey(o => o.GroupId);

            modelBuilder
                .Entity<SubjectUser> ()
                .HasOne (u => u.User)
                .WithMany (o => o.SubjectUsers)
                .HasForeignKey (o => o.UserId);

            modelBuilder
                .Entity<SubjectUser> ()
                .HasOne (u => u.Subject)
                .WithMany (o => o.SubjectUsers)
                .HasForeignKey (o => o.SubjectId);

            modelBuilder.Entity<User>().HasData(new User
            {    Id =1,
                 Name ="Enass",
                Email = "Admin@enass.com",
                Password = "enass023",
                Role = Enums.RoleEnum.Admin,
                 Phone ="0940053146",
                 IsLogIn = false
            });
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Level).WithMany();
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Level).WithMany();
                
               

        }

    }
}
