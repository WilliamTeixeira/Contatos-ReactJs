using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/Contato")]
    public class ContatoController : Controller
    {
        private List<Contato> contatos;


        [HttpGet("[action]")]
        public IEnumerable<Contato> RetornarTodos()
        {
            contatos = new ContatoDAO().RetornarTodos().ToList();
            return (from x in contatos orderby x.Nome select x);
        }

        // GET: api/Contato
        [HttpGet]
        public IEnumerable<Contato> GetContact()
        {
            return RetornarTodos();
        }

        // GET: api/Contato/5
        [HttpGet("{id}", Name = "GetContact")]
        public Contato GetContact(int id)
        {
            return new ContatoDAO().RetornarPorId(id);
            //return contatos.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/Contato
        [HttpPost]
        public void Post([FromBody]Contato obj)
        {
            new ContatoDAO().Inserir(obj);
            //contatos.Add(obj);
        }

        // PUT: api/Contato/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Contato obj)
        {
            new ContatoDAO().Alterar(obj);
            //contatos.Remove(GetContact(obj.Id));
            //contatos.Add(obj);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            new ContatoDAO().Excluir(new Contato() { Id = id });
            //contatos.Remove(GetContact(id));
        }

    }
}