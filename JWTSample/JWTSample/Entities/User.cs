using Newtonsoft.Json;
using System;

namespace JWTSample.Entities
{
    public class User
    {
        [JsonIgnore]
        public string ID { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Role { get; set; }
        public UserDetail UserDetail { get; set; }
    }

    public class UserDetail
    {
        [JsonIgnore]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
    }
}
