using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class Route
    {
        [Key]
        public int RouteID { get; set; }
        public string StartPoint { get; set; }
        public string FinishPoint { get; set; }
        public string RouteNumber { get; set; }
        public DateTime TimeArrival { get; set; }
        public int MaxNumberOfPlace { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}