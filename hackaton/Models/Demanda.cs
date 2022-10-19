using Microsoft.AspNetCore.Routing.Constraints;

namespace hackaton.Models
{
    public class Demanda
    {
        public int Id { get; set; }
        public IList<HistoricoEstado> ListaHistoricoEstado { get; set; }
        public DateTime DataEntrega { get; set; }
        public TimeSpan HoraDemanda { get; set; }
        public IList<Usuario> ListaUsuario { get; set; }
        public float PesoComplexidade { get; set; }
    }
}
