using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class Person : Base<int>
    {
        int _personId;
        [Key]
        public int PersonID {
            get { return _personId; }
            set
            {
                _personId = value;
                Id = value;
            } 
        }

        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(450)]
        [Required(ErrorMessage = "Please enter your logname")]
        public string LogName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your number of passport")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; } //что это за поле????!!!
    }
}