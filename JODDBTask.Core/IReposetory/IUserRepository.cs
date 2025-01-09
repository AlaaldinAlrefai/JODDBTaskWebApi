using JODDBTask.Core.Data;
using JODDBTask.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JODDBTask.Core.IReposetory
{
    public interface IUserRepository
    {
        
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(CreateUserDto userDto);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        public Task<int> CountUsersAsync();

        public Task<List<User>> GetUsersAsync(int pageNumber, int pageSize);


    }
}
