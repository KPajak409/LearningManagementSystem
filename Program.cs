using LMS;
using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LearningManagementSystemDb");
IWebHostEnvironment env = builder.Environment;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddUserManager<UserManager<User>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherAdminPermission", policy =>
    policy.RequireRole("Admin", "Teacher"));
    options.AddPolicy("AdminPermission", policy =>
    policy.RequireRole("Admin"));
    options.AddPolicy("TeacherPermission", policy =>
    policy.RequireRole("Teacher"));
});
builder.Services.AddRazorPages(options =>
{
    /*------------- Page folders  -------------*/
    options.Conventions.AuthorizeFolder("/Activities");
    options.Conventions.AuthorizeFolder("/Admin", "AdminPermission");
    options.Conventions.AuthorizeFolder("/Courses");
    options.Conventions.AuthorizeFolder("/Sections");
    options.Conventions.AuthorizeFolder("/Shared");
    options.Conventions.AuthorizeFolder("/Student");
    options.Conventions.AuthorizeFolder("/Teacher", "TeacherPermission");
    options.Conventions.AuthorizeFolder("/Users");
    /*------------- Activities -------------*/
    options.Conventions.AuthorizePage("/Activities/CreateActivity", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Activities/EditActivity", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Activities/DeleteActivity", "TeacherAdminPermission");
    /*------------- Courses -------------*/
    options.Conventions.AuthorizePage("/Courses/CreateCourse", "TeacherAdminPermission"); 
    options.Conventions.AuthorizePage("/Courses/EditCourse", "TeacherAdminPermission"); 
    options.Conventions.AuthorizePage("/Courses/DeleteCourse", "TeacherAdminPermission"); 
    options.Conventions.AuthorizePage("/Courses/CourseEditMode", "TeacherAdminPermission");
    /*------------- Groups -------------*/
    options.Conventions.AuthorizePage("/Groups/AssignUserToGroup", "TeacherAdminPermission"); 
    options.Conventions.AuthorizePage("/Groups/DeassignUserFromGroup", "TeacherAdminPermission"); 
    options.Conventions.AuthorizePage("/Groups/DeassignUserFromGroupConfirm", "TeacherAdminPermission"); 
    options.Conventions.AuthorizePage("/Groups/CreateGroup", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Groups/DeleteGroup", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Groups/EditGroup", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Groups/GroupList", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Groups/UserAssignation", "TeacherAdminPermission");
    /*------------- Sections -------------*/
    options.Conventions.AuthorizePage("/Sections/CreateSection", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Sections/DeleteSection", "TeacherAdminPermission");
    options.Conventions.AuthorizePage("/Sections/EditSection", "TeacherAdminPermission");
    /*------------- Shared -------------*/
    options.Conventions.AuthorizePage("/Shared/_LayoutAdmin");
    options.Conventions.AuthorizePage("/Shared/_LayoutTeacher");
    options.Conventions.AuthorizePage("/Shared/_QuestionCreate");
    options.Conventions.AuthorizePage("/Shared/_UserListForm");
    options.Conventions.AuthorizePage("/Shared/_UsersList");
    //options.Conventions.AuthorizePage("/");
    /*------------- Users -------------*/
    options.Conventions.AuthorizePage("/Users/CreateUser", "AdminPermission");
    options.Conventions.AuthorizePage("/Users/DeleteUser", "AdminPermission"); 
    options.Conventions.AuthorizePage("/Users/EditUser", "AdminPermission");
    options.Conventions.AuthorizePage("/Users/UserDetails", "AdminPermission");

});
builder.Services.AddControllersWithViews().AddRazorPagesOptions(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
    
});



builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementCourseHandler>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped<ApplicationMappingProfile>();
//builder.Services.AddScoped<IHostingEnvironment>(env);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAntiforgery(o => o.HeaderName = "CSRF-TOKEN");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
SeedDatabase();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

void SeedDatabase() //can be placed at the very bottom under app.Run()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbInitializer = services.GetRequiredService<DbInitializer>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        dbInitializer.SeedData(userManager, roleManager);
    }
}