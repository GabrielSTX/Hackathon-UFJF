namespace hackaton.Models
{
    internal class ConexaoBanco
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"Server={Server};Port={Port};Database={Database};User Id={UserId};Password={Password};";
        }
    }
}
