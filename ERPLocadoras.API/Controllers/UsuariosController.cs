using Microsoft.AspNetCore.Mvc;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("locadora/{locadoraId}")]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> ObterPorLocadora(Guid locadoraId)
        {
            var usuarios = await _usuarioService.ObterPorLocadoraAsync(locadoraId);
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> ObterPorId(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponse>> Criar([FromBody] CriarUsuarioRequest request)
        {
            try
            {
                var usuario = await _usuarioService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioResponse>> Atualizar(Guid id, [FromBody] AtualizarUsuarioRequest request)
        {
            try
            {
                var usuario = await _usuarioService.AtualizarAsync(id, request);

                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                return Ok(usuario);
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
                var resultado = await _usuarioService.ExcluirAsync(id);

                if (!resultado)
                    return NotFound("Usuário não encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusUsuarioRequest request)
        {
            var resultado = await _usuarioService.AlterarStatusAsync(id, request.Ativo);

            if (!resultado)
                return NotFound("Usuário não encontrado.");

            return NoContent();
        }
    }

    // DTO para alteração de status
    public class AlterarStatusUsuarioRequest
    {
        public bool Ativo { get; set; }
    }
}