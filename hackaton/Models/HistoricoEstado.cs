namespace hackaton.Models
{
    public class HistoricoEstado
    {
        public int Id { get; set; }
        public int IdDemanda { get; set; }
        public string Estado { get; set; }
        public DateTime DataMudancaEstado { get; set; }
    }
}