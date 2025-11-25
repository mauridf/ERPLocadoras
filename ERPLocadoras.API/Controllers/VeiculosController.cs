using Microsoft.AspNetCore.Mvc;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoService _veiculoService;

        public VeiculosController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoResponse>>> ObterTodos()
        {
            var veiculos = await _veiculoService.ObterTodosAsync();
            return Ok(veiculos);
        }

        [HttpGet("locadora/{locadoraId}")]
        public async Task<ActionResult<IEnumerable<VeiculoResponse>>> ObterPorLocadora(Guid locadoraId)
        {
            var veiculos = await _veiculoService.ObterPorLocadoraAsync(locadoraId);
            return Ok(veiculos);
        }

        [HttpGet("locadora/{locadoraId}/disponiveis")]
        public async Task<ActionResult<IEnumerable<VeiculoResponse>>> ObterDisponiveisPorLocadora(Guid locadoraId)
        {
            var veiculos = await _veiculoService.ObterDisponiveisPorLocadoraAsync(locadoraId);
            return Ok(veiculos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoResponse>> ObterPorId(Guid id)
        {
            var veiculo = await _veiculoService.ObterPorIdAsync(id);

            if (veiculo == null)
                return NotFound("Veículo não encontrado.");

            return Ok(veiculo);
        }

        [HttpPost]
        public async Task<ActionResult<VeiculoResponse>> Criar([FromBody] CriarVeiculoRequest request)
        {
            try
            {
                var veiculo = await _veiculoService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = veiculo.Id }, veiculo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VeiculoResponse>> Atualizar(Guid id, [FromBody] AtualizarVeiculoRequest request)
        {
            try
            {
                var veiculo = await _veiculoService.AtualizarAsync(id, request);

                if (veiculo == null)
                    return NotFound("Veículo não encontrado.");

                return Ok(veiculo);
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
                var resultado = await _veiculoService.ExcluirAsync(id);

                if (!resultado)
                    return NotFound("Veículo não encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/quilometragem")]
        public async Task<ActionResult> AtualizarQuilometragem(Guid id, [FromBody] AtualizarQuilometragemRequest request)
        {
            var resultado = await _veiculoService.AtualizarQuilometragemAsync(id, request.NovaQuilometragem);

            if (!resultado)
                return NotFound("Veículo não encontrado ou quilometragem inválida.");

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusVeiculoRequest request)
        {
            var resultado = await _veiculoService.AlterarStatusAsync(id, request.NovoStatus);

            if (!resultado)
                return NotFound("Veículo não encontrado.");

            return NoContent();
        }
    }

    // DTOs auxiliares
    public class AlterarStatusVeiculoRequest
    {
        public StatusVeiculo NovoStatus { get; set; }
    }
}