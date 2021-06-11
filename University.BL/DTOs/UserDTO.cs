using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.BL.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Image { get; set; }
        public string ImageBase64 { get; set; }
    }
}
