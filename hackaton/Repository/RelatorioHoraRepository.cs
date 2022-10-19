using hackaton.Models;
using Npgsql;

namespace hackaton.Repository
{
    public class RelatorioHoraRepository: RepositoryBase
    {
        public IList<RelatorioHora> ObterPorDemandaUsuario(int demandaId, int usuarioId)
        {
            var relatorios = new List<RelatorioHora>();

            var comando = $"SELECT * FROM hackathon.relatorio_hora where id_demanda = {demandaId} and id_usuario = {usuarioId}";

            using var command = new NpgsqlCommand(comando, ObterConexao());

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var relatorio = new RelatorioHora();
                relatorio.Id = Convert.ToInt32(reader["id"]);
                relatorio.IdDemanda = Convert.ToInt32(reader["id_demanda"]);
                relatorio.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
                relatorio.Data = Convert.ToDateTime(reader["data"]);
                relatorio.DataFim = Convert.ToDateTime(reader["data_fim"]);
                relatorio.DataInicio = Convert.ToDateTime(reader["data_inicio"]);

                relatorios.Add(relatorio);
            }

            return relatorios;
        }
    }
}
