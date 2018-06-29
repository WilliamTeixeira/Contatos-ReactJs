using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAO
{
    public class TelefoneDAO : BaseDAO
    {
        public void Inserir(Telefone obj)
        {
            string sql = "INSERT INTO telefone (idcontato, numero) VALUES (@idcontato, @numero);";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@idcontato", obj.IdContato));
            cmd.Parameters.Add(new MySqlParameter("@numero", obj.Numero));

            ExecutarComando(cmd);
        }

        public void Alterar(Telefone obj)
        {
            string sql = "UPDATE telefone SET idcontato = @idcontato, numero = @numero WHERE idtel = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);
            cmd.Parameters.Add(new MySqlParameter("@id", obj.Id));
            cmd.Parameters.Add(new MySqlParameter("@idcontato", obj.IdContato));
            cmd.Parameters.Add(new MySqlParameter("@numero", obj.Numero));

            ExecutarComando(cmd);
        }

        public void Excluir(Telefone obj)
        {
            string sql = "DELETE FROM telefone WHERE idtel = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@id", obj.Id));

            ExecutarComando(cmd);
        }

        public void ExcluirPorContato(int idContato)
        {
            string sql = "DELETE FROM telefone WHERE idcontato = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@id", idContato));

            ExecutarComando(cmd);
        }

        public Telefone RetornarPorId(int id)
        {
            string sql = "SELECT * FROM telefone WHERE idtel = @id;";

            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@id", id));

            DataSet ds = RetornarDataSet(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return RetornarObj(ds.Tables[0].Rows[0]);
        }

        public IEnumerable<Telefone> RetornarTodos(int idContato)
        {
            List<Telefone> objs = new List<Telefone>();

            string sql = "SELECT * FROM telefone where idcontato = @idcontato;";
            
            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Parameters.Add(new MySqlParameter("@idcontato", idContato));

            DataSet ds = RetornarDataSet(cmd);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                foreach (DataRow reg in ds.Tables[0].Rows)
                    objs.Add(RetornarObj(reg));

            return objs;
        }

        private Telefone RetornarObj(DataRow reg)
        {
            Telefone obj = new Telefone();

            obj.Id = Convert.ToInt32(reg["idtel"]);
            obj.IdContato = Convert.ToInt32(reg["idcontato"]);
            obj.Numero = reg["numero"].ToString();

            return obj;
        }
    }
}

