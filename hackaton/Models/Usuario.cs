namespace hackaton.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public float PesoEficiencia { get; set; }
        public IList<RelatorioHora> ListaRelatorioHora { get; set; }
    }
}