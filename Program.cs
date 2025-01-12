using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;

namespace ZamawianiePosiłkowOnline
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //true - use sqlite false-use sql server
            bool useSQLite = true;

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            if (!useSQLite)
            {
                //    "sqlServerConnectionString": "Server=Rydwan;Database=TestProjectdb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServerConnectionString")));
            }
            if (useSQLite)
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("localDb")));
            }

            builder.Services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 2;
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              .AddRoles<IdentityRole>();

            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //Create roles
            using(var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Client" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //add roles
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();

                string email = "admin@admin.pl";
                string password = "admin";
                if(await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new Users()
                    { Email = email,
                      UserName = email,
                      FullName = "admin"};
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();
        }
    }
}
