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
        public Resposta VerificaSprintLucrativa([FromBody] FiltroData data)
        {
            using var repository = new RelatorioHoraRepository();
            var historicos = repository.ObterPorPeriodo(data.Inicio, data.Fim);

            var horasTotais = ObterHorasTotais(historicos);
            var horasTotaisEquipe = ObterHorasTotaisEquipe(historicos);

            var resposta = new Resposta() 
            { 
                Resultado = horasTotaisEquipe < horasTotais,
                Mensagem = $"A equipe resolveu {horasTotais} horas estimadas utilizando {horasTotaisEquipe} horas de trabalho."
            };

            return resposta;
        }       

        [HttpPost, AllowAnonymous, Route("VerificaDemandaLucrativa/{demandaId}")]
        public Resposta VerificaDemandaLucrativa([FromRoute] int demandaId)
        {
            using var repository = new DemandaRepository();
            var demanda = repository.ObterDemanda(demandaId);
            var horaEstimada = demanda.HoraDemanda * demanda.PesoComplexidade;
            var horasTotais = ObterHoraTotalProjeto(demanda);

            var resposta = new Resposta()
            {
                Resultado = horaEstimada > horasTotais
            };

            if (resposta.Resultado)
                resposta.Mensagem = $"Sim, a equipe gastou um total de {horasTotais} horas para um projeto de {horaEstimada} horas.";
            else
                resposta.Mensagem = $"Não, a equipe gastou um total de {horasTotais} horas para um projeto de {horaEstimada} horas.";

            return resposta;
        }

        [HttpPost, AllowAnonymous, Route("VerificaDemandaVaiSerLucrativa")]
        public Resposta VerificaDemandaVaiSerLucrativa([FromBody] Demanda demanda)
        {
            var horaEstimada = demanda.HoraDemanda * demanda.PesoComplexidade;
            var horasEstimadasUsuarios = ObterHorasEstimadasUsuarios(demanda.Usuarios);

            var resposta = new Resposta() 
            { 
                Resultado = horaEstimada > horasEstimadasUsuarios,
            };

            if (resposta.Resultado)
                resposta.Mensagem = $"Sim, a estimativa é que a equipe gaste um total de {horasEstimadasUsuarios} horas para um projeto de {horaEstimada} horas.";
            else
                resposta.Mensagem = $"Não, a estimativa é que a equipe gaste um total de {horasEstimadasUsuarios} horas para um projeto de {horaEstimada} horas.";


            return resposta;
        }

        private double ObterHorasEstimadasUsuarios(IList<DemandaUsuario> usuarios)
        {
            using var repository = new UsuarioRepository();

            double horas = 0;
            foreach(var usuario in usuarios)
                horas += (usuario.HorasEstipuladas * repository.ObterEficiencia(usuario.UsuarioId));

            return horas;
        }

        private double ObterHoraTotalProjeto(Demanda demanda)
        {
            double horaTotal = 0;

            using var repository = new RelatorioHoraRepository();
            using var usuarioRepository = new UsuarioRepository();

            var relatoriosHora = repository.ObterPorDemanda(demanda.Id);
            foreach(var relatorio in relatoriosHora)
                horaTotal += (relatorio.Horas * usuarioRepository.ObterEficiencia(relatorio.UsuarioId));
            
            return horaTotal;
        }
        private double ObterHorasTotaisEquipe(IList<RelatorioHora> historicos)
        {
            Usuario usuario = new Usuario();
            using var repository = new UsuarioRepository();

            double horas = 0;
            foreach (var historico in historicos)
            {
                if (usuario.Id != historico.UsuarioId)
                    usuario = repository.Carregar(historico.UsuarioId);

                horas += (historico.Horas * usuario.PesoEficiencia);
            }

            return horas;
        }

        private double ObterHorasTotais(IList<RelatorioHora> historicos)
        {
            var demandaIds = historicos.Select(x => x.DemandaId).ToList();

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
