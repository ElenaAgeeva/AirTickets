using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public int NumberOfPlace { get; set; }
        public string Condition { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Route Route { get; set; }
        public virtual Person Person { get; set; }
    }
}