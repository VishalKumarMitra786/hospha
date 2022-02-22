using Hospha.DbModel;
using Hospha.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospha.Service
{
    public interface IRoleService
    {
    }
    public class RoleService : IRoleService
    {
        public HosphaContext Context { get; set; }
        public RoleService(HosphaContext context)
        {
            Context = context;
        }

        public static async Task CreateRolesAndPowerUser(IServiceProvider serviceProvider,
            string adminName,
            string email,
            int adminContractNumber,
            string password)
        {
            var context = serviceProvider.GetService<HosphaContext>();

            if (!context.Database.CanConnect())
            {
                context.Database.EnsureCreated();
            }
            else
            {
                context.Database.Migrate();
            }

            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = Global.roleNames;
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new User
            {
                UniqueId = Guid.NewGuid(),
                Name = adminName,
                UserName = adminContractNumber.ToString(),
                PhoneNumber = adminContractNumber.ToString(),
                Email = email,
                UpdatedAt = DateTime.UtcNow,
                IsEnabled = true
            };

            //Ensure you have these values in your appsettings.json file
            string userPWD = password;
            var _user = await UserManager.FindByEmailAsync(email);
            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, Hospha.Model.Constants.Roles.admin);
                }
            }
        }
    }
}
