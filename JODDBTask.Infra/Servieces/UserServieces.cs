using JODDBTask.Core.Data;
using JODDBTask.Core.Dto;
using JODDBTask.Core.Helpers;
using JODDBTask.Core.IReposetory;
using JODDBTask.Core.IServieces;
using JODDBTask.Infra.Helpers;
using JODDBTask.Infra.Reposetory;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace JODDBTask.Infra.Servieces
{
    public class UserServieces : IUserServieces
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;

        public UserServieces(IUserRepository userRepository, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> AddUserAsync(CreateUserDto userDto)
        {
            return await _userRepository.AddUserAsync(userDto);          
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

       

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return null;

            if (user.Password != password)
                return null;

            var token = _jwtHelper.GenerateToken(user);

            return token;
        }





        public async Task<PagedResult<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _userRepository.CountUsersAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var users = await _userRepository.GetUsersAsync(pageNumber, pageSize);

            return new PagedResult<User>
            {
                Items = users,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }



    }
}
