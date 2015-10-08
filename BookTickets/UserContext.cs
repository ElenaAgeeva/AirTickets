using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BookTickets.Models;

namespace BookTickets
{
    class UserContext : DbContext
    {
        public UserContext()
            : base("DbConnection")
        { }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Route> Routes { get; set; }
    }
}