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
        public IEnumerable<Telefone> Telefones { get => telefones; set => telefones = value; }

        private IEnumerable<Telefone> telefones = new List<Telefone>();

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
