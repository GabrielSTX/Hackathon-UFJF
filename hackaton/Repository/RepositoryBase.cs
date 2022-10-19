using hackaton.Models;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;

namespace hackaton.Repository
{
    public class RepositoryBase : IDisposable
    {
        private ConexaoBanco ConfigConexao { get; set; }
        private NpgsqlConnection Conexao { get; set; }

        public RepositoryBase()
        {
            ObterConfigConexao();
        }

        protected void ObterConfigConexao()
        {
            var objeto = JObject.Parse(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
            ConfigConexao = objeto["ConexaoBanco"].ToObject<ConexaoBanco>();
        }
        protected NpgsqlConnection ObterConexao()
        {
            if ((Conexao == null) || (Conexao.State != ConnectionState.Open))
            {
                Conexao = new NpgsqlConnection(ConfigConexao.ToString());
                Conexao.Open();
            }

            return Conexao;
        }

        public void Dispose()
        {
            if ((Conexao != null) && (Conexao.State != ConnectionState.Closed))
                Conexao.Close();
        }
    }
}
