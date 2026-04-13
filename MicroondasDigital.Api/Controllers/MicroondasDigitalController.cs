using MicroondasDigital.Api.DTO;
using MicroondasDigital.Aplicacao.Interfaces;
using System;
using System.Linq;
using System.Web.Http;

namespace MicroondasDigital.Api.Controllers
{
    [RoutePrefix("api/microondas")]
    public class MicroondasDigitalController : ApiController
    {
        private readonly IAquecimentoService _aquecimentoService;
        private readonly IProgramaAquecimentoService _programaAquecimentoService;
        private readonly IProgramaCustomizadoService _customizadoService;

        public MicroondasDigitalController(
            IAquecimentoService aquecimentoService, 
            IProgramaAquecimentoService programaAquecimentoService,
            IProgramaCustomizadoService customizadoService)
        {
            _aquecimentoService = aquecimentoService;
            _programaAquecimentoService = programaAquecimentoService;
            _customizadoService = customizadoService;
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

        [HttpGet]
        [Route("programas")]
        public IHttpActionResult ObterTodosProgramas()
        {
            var preDefinidos = _programaAquecimentoService.ObterTodos();
            var customizados = _customizadoService.ObterTodos();

            return Ok(new
            {
                preDefinidos = preDefinidos.Select(p => new {
                    nome = p.Nome,
                    tempo = p.Tempo,
                    potencia = p.Potencia,
                    instrucoes = p.Instrucoes
                }),
                customizados = customizados.Select(p => new {
                    nome = p.Nome,
                    tempo = p.Tempo,
                    potencia = p.Potencia,
                    instrucoes = p.Instrucoes
                })
            });
        }
    }
}