using Hospha.DbModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hospha.Service
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task<List<User>> GetUsers();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
    }
    public class UserService : IUserService, IProfileService
    {
        public HosphaContext Context { get; set; }
        public UserManager<User> UserManager { get; set; }
        public UserService(HosphaContext context, UserManager<User> userManager)
        {
            Context = context;
            UserManager = userManager;
        }
        public async Task<User> AddUser(User user)
        {
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await Context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            return user;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = await UserManager.GetUserAsync(context.Subject);
            var userRole = await UserManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("isEnabled", user.IsEnabled.ToString()),
                new Claim("phone", user.PhoneNumber.ToString()),
                new Claim("role", string.Join(",", userRole))
            };
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = await UserManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.IsEnabled;
        }
    }
}
