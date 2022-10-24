namespace hackaton.Models
{
    public class DemandaUsuario
    {
        public int Id { get; set; }
        public int DemandaId { get; set; }
        public int UsuarioId { get; set; }
        public double HorasEstipuladas { get; set; }
    }
}