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
            string sql = "INSERT INTO contato (nome ,email, telefone) VALUES (@nome ,@email, @telefone);";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new MySqlParameter("@email", obj.Email));
            cmd.Parameters.Add(new MySqlParameter("@telefone", obj.Telefone));

            ExecutarComando(cmd);
        }

        public void Alterar(Contato obj)
        {
            string sql = "UPDATE contato SET nome = @nome, email = @email, telefone = @telefone WHERE id = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);
            cmd.Parameters.Add(new MySqlParameter("@id", obj.Id));
            cmd.Parameters.Add(new MySqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new MySqlParameter("@email", obj.Email));
            cmd.Parameters.Add(new MySqlParameter("@telefone", obj.Telefone));

            ExecutarComando(cmd);
        }

        public void Excluir(Contato obj)
        {
            string sql = "DELETE FROM contato WHERE id = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@id", obj.Id));

            ExecutarComando(cmd);
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

        private Contato RetornarObj(DataRow reg)
        {
            Contato obj = new Contato();

            obj.Id = Convert.ToInt32(reg["id"]);
            obj.Nome = reg["nome"].ToString();
            obj.Email = reg["email"].ToString();
            obj.Telefone = reg["telefone"].ToString();

            return obj;
        }
    }
}
