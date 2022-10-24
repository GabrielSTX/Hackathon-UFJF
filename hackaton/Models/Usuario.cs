namespace hackaton.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double PesoEficiencia { get; set; }
        public IList<RelatorioHora> ListaRelatorioHora { get; set; }
    }
}