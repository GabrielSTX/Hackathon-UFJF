using hackathon.Models;
using hackathon.Repository;
using hackaton.Models;
using hackaton.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hackathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost, AllowAnonymous, Route("VerificaProdutividadeAcimaEsperado/{usuarioId}")]
        public Resposta VerificaProdutividadeAcimaEsperado([FromRoute] int usuarioId)
        {
            var horasEsperadas = ObterHorasEsperadas(usuarioId);
            var horasFeitas = ObterHorasFeitas(usuarioId);

            using var repository = new UsuarioRepository();
            var usuario = repository.Carregar(usuarioId);

            var produtividade = (horasEsperadas * usuario.PesoEficiencia) / horasFeitas;

            var resposta = new Resposta()
            {
                Resultado = produtividade > usuario.PesoEficiencia,
                Mensagem = $"O usuário está apresentando uma produtividade de {produtividade}; o esperado para ele é {usuario.PesoEficiencia}"
            };

            return resposta;
                
        }

        private double ObterHorasFeitas(int usuarioId)
        {
            using var repository = new RelatorioHoraRepository();
            var relatorios = repository.ObterPorUsuario(usuarioId);

            double horasTotais = 0;
            foreach(var relatorio in relatorios)
                horasTotais += relatorio.Horas;

            return horasTotais;
        }

        private double ObterHorasEsperadas(int usuarioId)
        {
            using var repository = new DemandaRepository();
            var demandas = repository.ObterUsuarioDemandas(usuarioId);
            double horas = 0;
            foreach(var demanda in demandas)
                horas += demanda.HorasEstipuladas;

            return horas;
        }
    }
}

