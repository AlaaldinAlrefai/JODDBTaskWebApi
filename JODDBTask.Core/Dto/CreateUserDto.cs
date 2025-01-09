using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JODDBTask.Core.Dto
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Photo { get; set; }
        public string Password { get; set; }
    }
}
