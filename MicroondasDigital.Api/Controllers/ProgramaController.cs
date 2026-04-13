using MicroondasDigital.Api.DTO;
using MicroondasDigital.Aplicacao.Interfaces;
using System;
using System.Web.Http;

namespace MicroondasDigital.Api.Controllers
{
    [RoutePrefix("api/programa")]
    public class ProgramaController : ApiController
    {
        private readonly IProgramaCustomizadoService _customizadoService;
        private readonly IProgramaAquecimentoService _preDefinidosService;

        public ProgramaController(
            IProgramaCustomizadoService customizadoService,
            IProgramaAquecimentoService preDefinidosService)
        {
            _customizadoService = customizadoService;
            _preDefinidosService = preDefinidosService;
        }

        [HttpPost]
        [Route("criar")]
        public IHttpActionResult Criar(CriarProgramaRequest request)
        {
            try
            {
                _customizadoService.Criar(
                    request.Nome,
                    request.Alimento,
                    request.Tempo,
                    request.Potencia,
                    request.Caractere,
                    request.Instrucoes
                );
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("todos")]
        public IHttpActionResult ObterCustomizados()
        {
            var customizados = _customizadoService.ObterTodos();

            return Ok(new
            {
                customizados
            });
        }
    }
}
