using Microsoft.AspNetCore.Mvc;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManutencoesController : ControllerBase
    {
        private readonly IManutencaoService _manutencaoService;

        public ManutencoesController(IManutencaoService manutencaoService)
        {
            _manutencaoService = manutencaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManutencaoResponse>>> ObterTodas()
        {
            var manutencoes = await _manutencaoService.ObterTodasAsync();
            return Ok(manutencoes);
        }

        [HttpGet("locadora/{locadoraId}")]
        public async Task<ActionResult<IEnumerable<ManutencaoResponse>>> ObterPorLocadora(Guid locadoraId)
        {
            var manutencoes = await _manutencaoService.ObterPorLocadoraAsync(locadoraId);
            return Ok(manutencoes);
        }

        [HttpGet("veiculo/{veiculoId}")]
        public async Task<ActionResult<IEnumerable<ManutencaoResponse>>> ObterPorVeiculo(Guid veiculoId)
        {
            var manutencoes = await _manutencaoService.ObterPorVeiculoAsync(veiculoId);
            return Ok(manutencoes);
        }

        [HttpGet("locadora/{locadoraId}/emandamento")]
        public async Task<ActionResult<IEnumerable<ManutencaoResponse>>> ObterEmAndamentoPorLocadora(Guid locadoraId)
        {
            var manutencoes = await _manutencaoService.ObterEmAndamentoPorLocadoraAsync(locadoraId);
            return Ok(manutencoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ManutencaoResponse>> ObterPorId(Guid id)
        {
            var manutencao = await _manutencaoService.ObterPorIdAsync(id);

            if (manutencao == null)
                return NotFound("Manutenção não encontrada.");

            return Ok(manutencao);
        }

        [HttpPost]
        public async Task<ActionResult<ManutencaoResponse>> Criar([FromBody] CriarManutencaoRequest request)
        {
            try
            {
                var manutencao = await _manutencaoService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = manutencao.Id }, manutencao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ManutencaoResponse>> Atualizar(Guid id, [FromBody] AtualizarManutencaoRequest request)
        {
            try
            {
                var manutencao = await _manutencaoService.AtualizarAsync(id, request);

                if (manutencao == null)
                    return NotFound("Manutenção não encontrada.");

                return Ok(manutencao);
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
                var resultado = await _manutencaoService.ExcluirAsync(id);

                if (!resultado)
                    return NotFound("Manutenção não encontrada.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/finalizar")]
        public async Task<ActionResult> FinalizarManutencao(Guid id, [FromBody] FinalizarManutencaoRequest request)
        {
            var resultado = await _manutencaoService.FinalizarManutencaoAsync(id, request);

            if (!resultado)
                return NotFound("Manutenção não encontrada ou não pode ser finalizada.");

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusManutencaoRequest request)
        {
            var resultado = await _manutencaoService.AlterarStatusAsync(id, request.NovoStatus);

            if (!resultado)
                return NotFound("Manutenção não encontrada.");

            return NoContent();
        }
    }

    // DTOs auxiliares
    public class AlterarStatusManutencaoRequest
    {
        public StatusManutencao NovoStatus { get; set; }
    }
}