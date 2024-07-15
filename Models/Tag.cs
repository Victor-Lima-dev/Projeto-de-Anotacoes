using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.Models
{
    public class Tag : Base
    {
        public string Titulo { get; set; }

        public List<Anotacao> Anotacoes { get; set; } = new List<Anotacao>();

    }
}