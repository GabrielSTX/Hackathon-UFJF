using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Win32;
using System.Net;
using hackathon.Repository;

namespace hackathon.Controllers
{
    public class InicializacaoController
    {
        public static void Iniciar()
        {
            using var repository = new ManutencaoRepository();
            repository.ExecutarManutencao();
        }
    }
}

