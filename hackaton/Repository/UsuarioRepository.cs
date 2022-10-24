using hackaton.Models;
using hackaton.Repository;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace hackathon.Repository
{
    public class UsuarioRepository : RepositoryBase
    {
        public Usuario Carregar(int id)
        {
            var comando = $"select * from public.usuario where id = {id}";
            using var command = new NpgsqlCommand(comando, ObterConexao());
            using var reader = command.ExecuteReader();

            var usuario = new Usuario();
            while (reader.Read())
            {
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.Nome = Convert.ToString(reader["nome"]);
                usuario.PesoEficiencia = Convert.ToDouble(reader["peso_eficiencia"]);
            }

            return usuario;
        }
        public double ObterEficiencia(int id)
        {
            var comando = $"select peso_eficiencia from public.usuario where id = {id}";
            using var command = new NpgsqlCommand(comando, ObterConexao());
            using var reader = command.ExecuteReader();

            if (reader.Read())
                return Convert.ToDouble(reader["peso_eficiencia"]);

            return 1;
        }
    }
}
