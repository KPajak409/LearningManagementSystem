using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _dbContext;


        public DbInitializer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            /*if (!_dbContext.Courses.Any())
                SeedCourses();*/
            _dbContext.SaveChanges();
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Student").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Student";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Teacher").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Teacher";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("admin@localhost").Result == null)
            {
                User user = new User();
                user.Email = "admin@localhost";
                user.UserName = user.Email;
                user.FirstName = "Admin";
                user.LastName = "Admin";
                IdentityResult result = userManager.CreateAsync(user, "User123@").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "Admin").Wait();
            }
            if (userManager.FindByEmailAsync("Teacher@localhost").Result == null)
            {
                User user = new User();
                user.Email = "Teacher@localhost";
                user.UserName = user.Email;
                user.FirstName = "Teacher";
                user.LastName = "Teacher";
                IdentityResult result = userManager.CreateAsync(user, "User123@").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "Teacher").Wait();
            }
            if (userManager.FindByEmailAsync("student@localhost").Result == null)
            {
                User user = new User();
                user.Email = "student@localhost";
                user.UserName = user.Email;
                user.FirstName = "Student";
                user.LastName = "Student";
                IdentityResult result = userManager.CreateAsync(user, "User123@").Result;
                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "Student").Wait();
            }
        }

        private void SeedCourses()
        {

        }
    }
}
