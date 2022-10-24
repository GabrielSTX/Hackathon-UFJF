using hackaton.Repository;
using Npgsql;

namespace hackathon.Repository
{
    public class ManutencaoRepository: RepositoryBase
    {
        public void ExecutarManutencao()
        {
            CriarDatabase();
            CriarTabelaDemanda();
            CriarTabelaHistoricoEstado();
            CriarTabelaRelatorioHora();
            CriarTabelaUsuario();
        }
        private void CriarDatabase()
        {
            try
            {
                var comando = "CREATE DATABASE hackathon WITH OWNER = postgres ENCODING = 'UTF8' CONNECTION LIMIT = -1;";
                var config = ObterConfigConexao();
                config.Database = "";

                using var command = new NpgsqlCommand(comando, ObterConexao(config));
                using var reader = command.ExecuteReader();
                Dispose();
            }
            catch
            {
                // se deu errado é porque já existe
            }
        }

        private void CriarTabela(string comando)
        {
            try
            {
                using var command = new NpgsqlCommand(comando, ObterConexao());
                using var reader = command.ExecuteReader();
            }
            catch
            {
                // se deu errado é porque já existe              
            }
        }
        private void CriarTabelaUsuario()
        {
            var comando = "CREATE TABLE public.usuario (id serial, nome character varying(100), peso_eficiencia double precision, PRIMARY KEY (id));" +
                "ALTER TABLE IF EXISTS public.usuario OWNER to postgres;";

            CriarTabela(comando);
        }

        private void CriarTabelaRelatorioHora()
        {
            var comando = "CREATE TABLE public.relatorio_hora (id serial, demanda_id integer, usuario_id integer, data timestamp with time zone, horas double precision, PRIMARY KEY (id));" +
                "ALTER TABLE IF EXISTS public.relatorio_hora OWNER to postgres;";

            CriarTabela(comando);
        }

        private void CriarTabelaHistoricoEstado()
        {
            var comando = "CREATE TABLE public.historico_estado(id serial, demanda_id integer, estado character varying(50), data_mudanca_estado timestamp with time zone, PRIMARY KEY (id));" +
                "ALTER TABLE IF EXISTS public.historico_estado OWNER to postgres;";

            CriarTabela(comando);
        }

        private void CriarTabelaDemanda()
        {
            var comando = "CREATE TABLE public.demanda(id bigserial, data_entrega timestamp with time zone, hora_demanda double precision, peso_complexidade double precision, PRIMARY KEY (id));" +
                "ALTER TABLE IF EXISTS public.demanda OWNER to postgres;" +
                "CREATE TABLE public.demanda_usuario(id serial, demanda_id integer, usuario_id integer, horas_estipuladas double precision, PRIMARY KEY (id));" +
                "ALTER TABLE IF EXISTS public.demanda_usuario OWNER to postgres;";

            CriarTabela(comando);
        }
    }
}
