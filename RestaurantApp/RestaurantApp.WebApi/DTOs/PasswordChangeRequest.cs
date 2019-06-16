using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.DTOs
{
    public class PasswordChangeRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
