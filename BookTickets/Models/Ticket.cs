using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class Ticket : Base<int>
    {
        int _ticketID;
        [Key]
        public int TicketID 
        {
            get { return _ticketID; }
            set
            {
                _ticketID = value;
                Id = value;
            }
        }
        public int NumberOfPlace { get; set; }
        public TypeOfTicketEnum Condition { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Route Route { get; set; }
        public virtual Person Person { get; set; }
    }
}