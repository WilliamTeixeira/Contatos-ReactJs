using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAO
{
    public class ContatoDAO : BaseDAO
    {
        public void Inserir(Contato obj)
        {
            string sql = "INSERT INTO contato (nome ,email) VALUES (@nome ,@email);";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new MySqlParameter("@email", obj.Email));

            ExecutarComando(cmd);

            obj.Id = LastIdContato(); //id -1 significa que houve um erro ao incluir então não inclui o tel
            if (obj.Telefone != null && obj.Id != -1)
            {
                var novoTel = new Telefone { IdContato = obj.Id, Numero = obj.Telefone };
                new TelefoneDAO().Inserir(novoTel);
                obj.Telefones = obj.Telefones.Append(novoTel);
            }
        }

        public void Alterar(Contato obj)
        {
            string sql = "UPDATE contato SET nome = @nome, email = @email WHERE id = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);
            cmd.Parameters.Add(new MySqlParameter("@id", obj.Id));
            cmd.Parameters.Add(new MySqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new MySqlParameter("@email", obj.Email));

            ExecutarComando(cmd);

            if (obj.Telefone != null)
                new TelefoneDAO().Inserir(new Telefone { IdContato = obj.Id, Numero = obj.Telefone });
        }

        public void Excluir(Contato obj)
        {
            string sql = "DELETE FROM contato WHERE id = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@id", obj.Id));

            ExecutarComando(cmd);

            new TelefoneDAO().ExcluirPorContato(obj.Id);
        }

        public Contato RetornarPorId(int id)
        {
            string sql = "SELECT * FROM contato WHERE id = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@id", id));

            DataSet ds = RetornarDataSet(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return RetornarObj(ds.Tables[0].Rows[0]);
        }

        public IEnumerable<Contato> RetornarTodos()
        {
            List<Contato> objs = new List<Contato>();

            string sql = "SELECT * FROM contato ORDER BY nome;";

            MySqlCommand cmd = new MySqlCommand(sql);

            DataSet ds = RetornarDataSet(cmd);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                foreach (DataRow reg in ds.Tables[0].Rows)
                    objs.Add(RetornarObj(reg));

            return objs;
        }

        private int LastIdContato()
        {
            string sql = "select last_insert_id() as id;";
            MySqlCommand cmd = new MySqlCommand(sql);
            DataSet ds = RetornarDataSet(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return -1;
            else
                return Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);

        }
        private Contato RetornarObj(DataRow reg)
        {
            Contato obj = new Contato();

            obj.Id = Convert.ToInt32(reg["id"]);
            obj.Nome = reg["nome"].ToString();
            obj.Email = reg["email"].ToString();
            obj.Telefone = reg["telefone"].ToString();

            obj.Telefones = new TelefoneDAO().RetornarTodos(obj.Id);

            return obj;
        }
    }
}
