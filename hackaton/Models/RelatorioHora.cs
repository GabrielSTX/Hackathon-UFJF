namespace hackaton.Models
{
    public class RelatorioHora
    {
        public int Id { get; set; }
        public int IdDemanda { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}