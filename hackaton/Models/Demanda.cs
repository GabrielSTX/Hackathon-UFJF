using Microsoft.AspNetCore.Routing.Constraints;

namespace hackaton.Models
{
    public class Demanda
    {
        public int Id { get; set; }
        public DateTime DataEntrega { get; set; }
        public double HoraDemanda { get; set; }
        public float PesoComplexidade { get; set; }
        public IList<DemandaUsuario> Usuarios { get; set; }
        public IList<HistoricoEstado> ListaHistoricoEstado { get; set; }


        // somatório das horas estipuladas tem que ser = ao horaDemanda * pesoComplexidade
    }
}
