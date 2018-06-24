using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Contato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public String Telefone { get; set; }
        public List<string> Telefones { get => Telefones; set => Telefones = value; }

        private List<string> telefones;

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
