using Microsoft.AspNetCore.Mvc;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocadorasController : ControllerBase
    {
        private readonly ILocadoraService _locadoraService;

        public LocadorasController(ILocadoraService locadoraService)
        {
            _locadoraService = locadoraService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocadoraResponse>>> ObterTodas()
        {
            var locadoras = await _locadoraService.ObterTodasAsync();
            return Ok(locadoras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocadoraResponse>> ObterPorId(Guid id)
        {
            var locadora = await _locadoraService.ObterPorIdAsync(id);

            if (locadora == null)
                return NotFound("Locadora não encontrada.");

            return Ok(locadora);
        }

        [HttpPost]
        public async Task<ActionResult<LocadoraResponse>> Criar([FromBody] CriarLocadoraRequest request)
        {
            try
            {
                var locadora = await _locadoraService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = locadora.Id }, locadora);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LocadoraResponse>> Atualizar(Guid id, [FromBody] AtualizarLocadoraRequest request)
        {
            var locadora = await _locadoraService.AtualizarAsync(id, request);

            if (locadora == null)
                return NotFound("Locadora não encontrada.");

            return Ok(locadora);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            var resultado = await _locadoraService.ExcluirAsync(id);

            if (!resultado)
                return NotFound("Locadora não encontrada.");

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusRequest request)
        {
            var resultado = await _locadoraService.AlterarStatusAsync(id, request.NovoStatus);

            if (!resultado)
                return NotFound("Locadora não encontrada.");

            return NoContent();
        }
    }

    // DTO para alteração de status
    public class AlterarStatusRequest
    {
        public StatusLocadora NovoStatus { get; set; }
    }
}