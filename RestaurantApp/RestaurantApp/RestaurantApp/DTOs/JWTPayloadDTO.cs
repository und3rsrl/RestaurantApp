using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.DTOs
{
    public class JWTPayloadDTO
    {
        public string Sub { get; set; }
        public string Jti { get; set; }
        public string NameIdentifier { get; set; }
        public string Role { get; set; }
        public string Exp { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
