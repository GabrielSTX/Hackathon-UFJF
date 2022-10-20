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
            ConfigConexao = ObterConfigConexao();
        }

        protected ConexaoBanco ObterConfigConexao()
        {
            using var reader = new StreamReader(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
            var appsettings = reader.ReadToEnd();
            var objeto = JObject.Parse(appsettings);
            return objeto["ConexaoBanco"].ToObject<ConexaoBanco>();
        }
        protected NpgsqlConnection ObterConexao()
        {
            return ObterConexao(ConfigConexao);
        }
        protected NpgsqlConnection ObterConexao(ConexaoBanco config)
        {
            if ((Conexao == null) || (Conexao.State != ConnectionState.Open))
            {
                Conexao = new NpgsqlConnection(config.ToString());
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
