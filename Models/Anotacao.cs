using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End.Models
{
    public class Anotacao : Base
    {
        public string Titulo { get; set; }

        public string Conteudo { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}