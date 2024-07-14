using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.Models
{
    public class Base
    {
        public guid Id { get; set; } = Guid.NewGuid();
    }
}