using JODDBTask.Core.Data;
using JODDBTask.Core.Dto;
using JODDBTask.Core.IServieces;
using JODDBTask.Infra.Servieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JODDBTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// ///////////////////
        /// </summary>
        private readonly IUserServieces _userService;
        private readonly IExcelImporterService _excelImporterService;


        public UsersController(IUserServieces userService, IExcelImporterService excelImporterService)
        {
            _userService = userService;
            _excelImporterService = excelImporterService;

        }

        //[HttpGet]
        //[Authorize]

        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    var users = await _userService.GetUsersAsync();
        //    return Ok(users);
        //}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {

            Console.WriteLine($"PageNumber: {pageNumber}, PageSize: {pageSize}");

            var result = await _userService.GetUsersAsync(pageNumber, pageSize);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(CreateUserDto userDto)
        {
            var user = await _userService.AddUserAsync(userDto);
            if (user != null)
            {
      
                Console.WriteLine(user.Name);
            }
            else
            {
                Console.WriteLine("The user object is null");
            }
            return CreatedAtAction(nameof(GetUser), new {id=user.Id},user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id,User user)
        {
            var existingUser=await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid client request");
            }

            var token = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("import")]
        [Authorize]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            await _excelImporterService.ImportExcelAsync(file);
            return Ok("File imported successfully.");
        }


    

    }
}
