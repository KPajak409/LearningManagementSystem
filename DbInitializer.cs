using Bogus;
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
            if(!_dbContext.Users.Any())
            {
                using (var reader = new StreamReader("UsersData.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        User user = new User();
                        user.FirstName = values[0];
                        user.LastName = values[1];
                        user.Email = values[2];
                        user.UserName = values[2];

                        IdentityResult result = userManager.CreateAsync(user, "User123@").Result;

                        if (result.Succeeded)
                            userManager.AddToRoleAsync(user, "Student").Wait();
                    }
                }
            }
            
            
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
        public async Task SeedStudents(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            int countUsers = _dbContext.Users.Count();

            if (countUsers < 100)
            {
                var stock = new Faker<User>()
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.FirstName, f => f.Name.FirstName())
                .RuleFor(m => m.LastName, f => f.Name.LastName());
                for (int i = 0; i < 100; i++)
                {
                    var user = stock.Generate();
                    IdentityResult result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                        userManager.AddToRoleAsync(user, "Student").Wait();
                }

                await _dbContext.SaveChangesAsync();

            }
        }
        private void SeedCourses()
        {

        }

    }
}
