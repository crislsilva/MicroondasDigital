using System;
using System.Web.Http;
using MicroondasDigital.Api.DTO;
using MicroondasDigital.Aplicacao.Interfaces;

namespace MicroondasDigital.Api.Controllers
{
    [RoutePrefix("api/microondas")]
    public class MicroondasDigitalController : ApiController
    {
        private readonly IAquecimentoService _aquecimentoService;
        private readonly IProgramaAquecimentoService _programaAquecimentoService;
        public MicroondasDigitalController(IAquecimentoService aquecimentoService, IProgramaAquecimentoService programaAquecimentoService)
        {
            _aquecimentoService = aquecimentoService;
            _programaAquecimentoService = programaAquecimentoService;
        }

        [HttpPost]
        [Route("iniciar")]
        public IHttpActionResult Iniciar([FromBody] AquecimentoRequest request)
        {
            try
            {
                _aquecimentoService.Iniciar(request.Tempo, request.Potencia);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("pausar-cancelar")]
        public IHttpActionResult PausarCancelar()
        {
            var resultado = _aquecimentoService.PausarOuCancelar();
            return Ok(new
            {
                resultado
            });
        }

        [HttpPost]
        [Route("continuar")]
        public IHttpActionResult Continuar()
        {
            _aquecimentoService.Continuar();
            return Ok();
        }

        [HttpGet]
        [Route("status")]
        public IHttpActionResult ObterStatus()
        {
            var status = _aquecimentoService.TimerTick();
            var tempo = _aquecimentoService.ObterTempoFormatado();

            return Ok(new
            {
                tempo,
                status

            });
        }

        [HttpGet]
        [Route("programas")]
        public IHttpActionResult ObterProgramas()
        {
            var programas = _programaAquecimentoService.ObterTodos();
            return Ok(programas);
        }

        [HttpPost]
        [Route("selecionar-programa")]
        public IHttpActionResult SelecionarPrograma([FromUri] string nome)
        {
            try
            {
                var resultado =_aquecimentoService.SelecionarPrograma(nome);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}