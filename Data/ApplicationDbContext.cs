using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Dtos;
using Bogus;

namespace LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; } 
        public DbSet<Section> Sections { get; set; } 
        public DbSet<Activity> Activities { get; set; } 
        public DbSet<Question> Questions { get; set; } 
        public DbSet<Answer> Answers { get; set; } 
        public DbSet<Group> Groups { get; set; } 
        public DbSet<ActivityUserResponse> ActivityUserResponses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
           

        }
    
    }
}