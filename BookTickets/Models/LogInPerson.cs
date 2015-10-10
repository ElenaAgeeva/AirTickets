using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class LogInPerson
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string LogName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}