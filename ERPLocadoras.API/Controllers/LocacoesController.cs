using Microsoft.AspNetCore.Mvc;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocacoesController : ControllerBase
    {
        private readonly ILocacaoService _locacaoService;

        public LocacoesController(ILocacaoService locacaoService)
        {
            _locacaoService = locacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocacaoResponse>>> ObterTodas()
        {
            var locacoes = await _locacaoService.ObterTodasAsync();
            return Ok(locacoes);
        }

        [HttpGet("locadora/{locadoraId}")]
        public async Task<ActionResult<IEnumerable<LocacaoResponse>>> ObterPorLocadora(Guid locadoraId)
        {
            var locacoes = await _locacaoService.ObterPorLocadoraAsync(locadoraId);
            return Ok(locacoes);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<LocacaoResponse>>> ObterPorCliente(Guid clienteId)
        {
            var locacoes = await _locacaoService.ObterPorClienteAsync(clienteId);
            return Ok(locacoes);
        }

        [HttpGet("locadora/{locadoraId}/ativas")]
        public async Task<ActionResult<IEnumerable<LocacaoResponse>>> ObterAtivasPorLocadora(Guid locadoraId)
        {
            var locacoes = await _locacaoService.ObterAtivasPorLocadoraAsync(locadoraId);
            return Ok(locacoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocacaoResponse>> ObterPorId(Guid id)
        {
            var locacao = await _locacaoService.ObterPorIdAsync(id);

            if (locacao == null)
                return NotFound("Locação não encontrada.");

            return Ok(locacao);
        }

        [HttpPost]
        public async Task<ActionResult<LocacaoResponse>> Criar([FromBody] CriarLocacaoRequest request)
        {
            try
            {
                var locacao = await _locacaoService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = locacao.Id }, locacao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LocacaoResponse>> Atualizar(Guid id, [FromBody] AtualizarLocacaoRequest request)
        {
            try
            {
                var locacao = await _locacaoService.AtualizarAsync(id, request);

                if (locacao == null)
                    return NotFound("Locação não encontrada.");

                return Ok(locacao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            try
            {
                var resultado = await _locacaoService.ExcluirAsync(id);

                if (!resultado)
                    return NotFound("Locação não encontrada.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/iniciar")]
        public async Task<ActionResult> IniciarLocacao(Guid id, [FromBody] ChecklistEntregaRequest request)
        {
            var resultado = await _locacaoService.IniciarLocacaoAsync(id, request);

            if (!resultado)
                return NotFound("Locação não encontrada ou não pode ser iniciada.");

            return NoContent();
        }

        [HttpPost("{id}/finalizar")]
        public async Task<ActionResult> FinalizarLocacao(Guid id, [FromBody] FinalizarLocacaoRequest request)
        {
            var resultado = await _locacaoService.FinalizarLocacaoAsync(id, request);

            if (!resultado)
                return NotFound("Locação não encontrada ou não pode ser finalizada.");

            return NoContent();
        }

        [HttpPost("{id}/cancelar")]
        public async Task<ActionResult> CancelarLocacao(Guid id)
        {
            var resultado = await _locacaoService.CancelarLocacaoAsync(id);

            if (!resultado)
                return NotFound("Locação não encontrada ou não pode ser cancelada.");

            return NoContent();
        }

        [HttpPost("{id}/atraso")]
        public async Task<ActionResult> MarcarComoAtraso(Guid id)
        {
            var resultado = await _locacaoService.MarcarComoAtrasoAsync(id);

            if (!resultado)
                return NotFound("Locação não encontrada ou não pode ser marcada como atraso.");

            return NoContent();
        }
    }
}