using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTickets.Models
{
    public interface IBase<T>
    {
        T Id { get; set; }
    }
}
