namespace hackaton.Models
{
    public class Time
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public IList<Usuario> ListaUsuario { get; set; }
    }
}