using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class Base<T> : IBase<T>
    {
        [NotMapped]
        public T Id { get; set; }
    }
}