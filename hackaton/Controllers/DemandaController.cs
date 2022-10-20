using hackathon.Models;
using hackathon.Repository;
using hackaton.Models;
using hackaton.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication;

namespace hackaton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandaController : ControllerBase
    {
        [HttpPost, AllowAnonymous, Route("VerificaSprintLucrativa")]
        public bool VerificaSprintLucrativa([FromBody] FiltroData data)
        {
            using var repository = new RelatorioHoraRepository();
            var historicos = repository.ObterPorPeriodo(data.Inicio, data.Fim);

            var horasTotais = ObterHorasTotais(historicos);
            var horasTotaisEquipe = ObterHorasTotaisEquipe(historicos);
            return horasTotaisEquipe < horasTotais;
        }       

        [HttpPost, AllowAnonymous, Route("VerificaDemandaLucrativa")]
        public bool VerificaDemandaLucrativa([FromBody] Demanda demanda)
        {
            var horaEstimada = demanda.HoraDemanda * demanda.PesoComplexidade;
            return horaEstimada > ObterHoraTotalProjeto(demanda);
        }        



        private double ObterHoraTotalProjeto(Demanda demanda)
        {
            double horaTotal = 0;

            using var repository = new RelatorioHoraRepository();
            using var usuarioRepository = new UsuarioRepository();

            foreach (var usuario in demanda.Usuarios)
            {
                var relatoriosHora = repository.ObterPorDemandaUsuario(demanda.Id, usuario.Id);

                double horaTotalUsuario = 0;
                foreach(var relatorio in relatoriosHora)
                    horaTotalUsuario += relatorio.Horas;

                horaTotal += (horaTotalUsuario * usuarioRepository.ObterEficiencia(usuario.Id));
            }

            return horaTotal;
        }
        private double ObterHorasTotaisEquipe(IList<RelatorioHora> historicos)
        {
            Usuario usuario = new Usuario();
            using var repository = new UsuarioRepository();

            double horas = 0;
            foreach (var historico in historicos)
            {
                if (usuario.Id != historico.IdUsuario)
                    usuario = repository.Carregar(historico.IdUsuario);

                horas += (historico.Horas * usuario.PesoEficiencia);
            }

            return horas;
        }

        private double ObterHorasTotais(IList<RelatorioHora> historicos)
        {
            var demandaIds = historicos.Select(x => x.IdDemanda).ToList();

            if (demandaIds.Count == 0)
                return 0;

            using var repository = new DemandaRepository();
            var demandas = repository.ObterDemandas(demandaIds);
            double horasTotais = 0;

            foreach (var demanda in demandas)
                horasTotais += demanda.HoraDemanda * demanda.PesoComplexidade;

            return horasTotais;
        }
    }
}
