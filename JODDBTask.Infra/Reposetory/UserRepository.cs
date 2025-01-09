using AutoMapper;
using JODDBTask.Core.Data;
using JODDBTask.Core.Dto;
using JODDBTask.Core.Helpers;
using JODDBTask.Core.IReposetory;
using JODDBTask.Infra.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JODDBTask.Infra.Reposetory
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;


        public UserRepository(ApplicationDbContext context, IJwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddUserAsync(CreateUserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User DTO cannot be null");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                MobileNumber = userDto.MobileNumber,
                Photo=userDto.Photo,
                Password = userDto.Password,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.MobileNumber = user.MobileNumber;
            existingUser.Photo = user.Photo;
            existingUser.Password = user.Password;

            await _context.SaveChangesAsync();
        }


        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }



        public async Task<int> CountUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<List<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
