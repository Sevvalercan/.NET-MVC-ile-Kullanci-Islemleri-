using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models
{

    //"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UserInformation;Trusted_Connection=True;MultipleActiveResultSets=true",
    public class User
    {
       public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
