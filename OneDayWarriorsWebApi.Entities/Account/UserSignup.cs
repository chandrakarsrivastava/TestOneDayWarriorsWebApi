using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Entities.Account
{
    public class UserSignup
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
