using hackaton.Models;
using hackaton.Repository;
using Npgsql;

namespace hackathon.Repository
{
    public class DemandaRepository: RepositoryBase
    {

        public Demanda ObterDemanda(int id)
        {
            var comando = $"select * FROM public.demanda where id = {id}";
            using var command = new NpgsqlCommand(comando, ObterConexao());
            using var reader = command.ExecuteReader();

            var demanda = new Demanda();
            if (reader.Read())
            {
                demanda.Id = Convert.ToInt32(reader["id"]);
                demanda.DataEntrega = Convert.ToDateTime(reader["data_entrega"]);
                demanda.HoraDemanda = Convert.ToDouble(reader["hora_demanda"]);
                demanda.PesoComplexidade = Convert.ToInt32(reader["peso_complexidade"]);                
            }
            return demanda;
        }

        public IList<Demanda> ObterDemandas(IList<int> ids)
        {
            var comando = $"select * FROM public.demanda where id in ({string.Join(',', ids)})";
            return ObterLista(comando);
        }

        public IList<Demanda> ObterLista(string comando)
        {
            var demandas = new List<Demanda>();
            using var command = new NpgsqlCommand(comando, ObterConexao());
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var demanda = new Demanda();
                demanda.Id = Convert.ToInt32(reader["id"]);
                demanda.DataEntrega = Convert.ToDateTime(reader["data_entrega"]);
                demanda.HoraDemanda = Convert.ToDouble(reader["hora_demanda"]);
                demanda.PesoComplexidade = Convert.ToInt32(reader["peso_complexidade"]);

                demandas.Add(demanda);
            }

            return demandas;
        }

        public IList<DemandaUsuario> ObterUsuarioDemandas(int usuarioId)
        {
            var comando = $"select * FROM public.demanda_usuario where usuario_id = {usuarioId}";
            
            using var command = new NpgsqlCommand(comando, ObterConexao());
            using var reader = command.ExecuteReader();

            var usuarioDemandas = new List<DemandaUsuario>();
            while (reader.Read())
            {
                var usuario = new DemandaUsuario();
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.DemandaId = Convert.ToInt32(reader["demanda_id"]);
                usuario.UsuarioId = Convert.ToInt32(reader["usuario_id"]);
                usuario.HorasEstipuladas = Convert.ToDouble(reader["horas_estipuladas"]);

                usuarioDemandas.Add(usuario);
            }

            return usuarioDemandas;
        }
    }
}
