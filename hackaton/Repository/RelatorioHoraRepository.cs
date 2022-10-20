using hackaton.Models;
using Npgsql;

namespace hackaton.Repository
{
    public class RelatorioHoraRepository: RepositoryBase
    {
        public IList<RelatorioHora> ObterPorPeriodo(DateTime inicio, DateTime fim)
        {
            var comando = $"SELECT * FROM public.relatorio_hora where data between '{inicio:yyyy-MM-dd HH:mm:ss}'::timestamp and '{fim:yyyy-MM-dd HH:mm:ss}'::timestamp";
            return ObterLista(comando);
        }

        public IList<RelatorioHora> ObterPorDemandaUsuario(int demandaId, int usuarioId)
        {
            var comando = $"SELECT * FROM public.relatorio_hora where demanda_id = {demandaId} and usuario_id = {usuarioId}";
            return ObterLista(comando);
        }

        public IList<RelatorioHora> ObterPorUsuario(int usuarioId)
        {
            var comando = $"SELECT * FROM public.relatorio_hora where usuario_id = {usuarioId}";
            return ObterLista(comando);
        }

        public IList<RelatorioHora> ObterLista(string comando)
        {
            var relatorios = new List<RelatorioHora>();            
            using var command = new NpgsqlCommand(comando, ObterConexao());
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var relatorio = new RelatorioHora();
                relatorio.Id = Convert.ToInt32(reader["id"]);
                relatorio.IdDemanda = Convert.ToInt32(reader["demanda_id"]);
                relatorio.IdUsuario = Convert.ToInt32(reader["usuario_id"]);
                relatorio.Data = Convert.ToDateTime(reader["data"]);
                relatorio.Horas = Convert.ToDouble(reader["horas"]);

                relatorios.Add(relatorio);
            }

            return relatorios;
        }

        
    }
}
