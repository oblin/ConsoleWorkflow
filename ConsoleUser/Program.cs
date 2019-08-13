using AbhCare.Identity.Data;
using AbhCare.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleUser
{
    class Program
    {
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            serviceProvider = ConfigureServices();

            while (ReadCommand()) { }
        }

        private static bool ReadCommand()
        {
            Console.WriteLine("Plase input command (Enter to input): ");
            Console.WriteLine("A. Add new User. ");
            Console.WriteLine("B. Query Users. ");
            Console.WriteLine("C. User login. ");
            Console.Write("Your command is (No input to leave): ");
            var command = Console.ReadLine();
            switch (command)
            {
                case "A":
                    Console.Write("User name is: ");
                    var username = Console.ReadLine();
                    if (!string.IsNullOrEmpty(username))
                        AddNewUser(username);
                    break;
                case "B":
                    ShowAllUsers();
                    break;
                case "C":
                    Console.Write("User name is: ");
                    var name = Console.ReadLine();
                    Console.Write("password is: ");
                    var password = Console.ReadLine();
                    LoginUser(name, password);
                    break;
                default:
                    return false;
            }
            return true;
        }

        private async static void LoginUser(string name, string password)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(name);
            if (user == null)
            {
                Console.WriteLine($"{name} can not find user");
                return;
            }

            var result = await userManager.CheckPasswordAsync(user, password);
            if (result)
            {
                Console.WriteLine($"user: {name} login succeed!");
            }
            else
            {
                Console.WriteLine($"login failed");
            }
        }

        private static void ShowAllUsers()
        {
            using (var context = new RbIdentityContext())
            {
                foreach (var user in context.Users)
                {
                    Console.WriteLine($"name: {user.UserName}, email: {user.Email}");
                }
            }
        }

        private async static void AddNewUser(string username)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = new ApplicationUser
            {
                UserName = username,
                Email = $"{username}@example.com"
            };
            var result = await userManager.CreateAsync(user, "password");
            if (result.Succeeded)
            {
                Console.WriteLine($"create user: {username} succeed!");
            }
            else
            {
                string error = string.Empty;
                foreach (var e in result.Errors)
                    error += e.Description + "\n";
                Console.WriteLine($"create user: {username} failed: {error}");
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(logger => logger.AddDebug());

            services.AddDbContext<RbIdentityContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                // Configure identity options
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<RbIdentityContext>()
                .AddDefaultTokenProviders();

            return services.BuildServiceProvider();
        }
    }
}
