using JODDBTask.Core.Data;
using JODDBTask.Core.Dto;
using JODDBTask.Core.IReposetory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JODDBTask.Core.IServieces
{
    public interface IUserServieces
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(CreateUserDto userDto);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<string> AuthenticateAsync(string email, string password);
        public Task<PagedResult<User>> GetUsersAsync(int pageNumber, int pageSize);
    }
}
