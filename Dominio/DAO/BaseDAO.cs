using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DAO
{
    public class BaseDAO
    {
        public void ExecutarComando(MySqlCommand cmd)
        {
            using (MySqlConnection conexao = RetornarConexao())
            {
                conexao.Open();
                try
                {
                    cmd.Connection = conexao; //atribui a conexão ao objeto cmd que foi recebido por parametro
                    cmd.ExecuteNonQuery();   // executa o comando do cmd
                }
                catch
                {
                    throw;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public void ExecutarComandos(List<MySqlCommand> comandos)
        {
            using (MySqlConnection conexao = RetornarConexao())
            {
                conexao.Open();
                MySqlTransaction transacao = conexao.BeginTransaction();

                try
                {
                    foreach (var cmd in comandos)
                    {
                        cmd.Connection = conexao;
                        cmd.Transaction = transacao;
                        cmd.ExecuteNonQuery();
                    }
                    transacao.Commit();
                }
                catch
                {
                    transacao.Rollback();
                    throw;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public DataSet RetornarDataSet(MySqlCommand cmd)
        {
            using (MySqlConnection conexao = RetornarConexao())
            {
                conexao.Open();
                try
                {
                    cmd.Connection = conexao;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    da.Fill(ds);

                    return ds;
                }
                catch
                {

                    throw;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        protected MySqlConnection RetornarConexao()
        {
            return new MySqlConnection(RetornarStringConexao());
        }

        private string RetornarStringConexao()
        {
            return "SERVER=localhost; " +
                     "DATABASE=bdprodutos; " +
                     "UID=root; " +
                     "PASSWORD=root;";
        }
    }
}
