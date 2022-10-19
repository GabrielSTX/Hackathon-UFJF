using hackaton.Models;
using hackaton.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hackaton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandaController : ControllerBase
    {
        [HttpGet, AllowAnonymous, Route("VerificaDemandaLucrativa")]
        public bool VerificaDemandaLucrativa([FromBody] Demanda demanda)
        {
            var horaEstimada = demanda.HoraDemanda.Multiply(demanda.PesoComplexidade).TotalHours;
            return horaEstimada > ObterHoraTotalProjeto(demanda);
        }

        private double ObterHoraTotalProjeto(Demanda demanda)
        {
            double horaTotal = 0;

            using var repository = new RelatorioHoraRepository();
            foreach(var usuario in demanda.ListaUsuario)
            {
                var relatoriosHora = repository.ObterPorDemandaUsuario(demanda.Id, usuario.Id);

                double horaTotalUsuario = 0;
                foreach(var relatorio in relatoriosHora)
                {
                    horaTotalUsuario += (relatorio.DataFim - relatorio.DataInicio).TotalHours;
                }

                horaTotal += (horaTotalUsuario * usuario.PesoEficiencia);
            }

            return horaTotal;
        }
    }
}
