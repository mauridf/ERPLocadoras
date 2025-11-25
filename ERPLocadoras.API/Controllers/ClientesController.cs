using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Application.Services;
using ERPLocadoras.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IAuthService _authService;

        public ClientesController(IClienteService clienteService, IAuthService authService)
        {
            _clienteService = clienteService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponse>>> ObterTodos()
        {
            var clientes = await _clienteService.ObterTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponse>> ObterPorId(Guid id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponse>> Criar([FromBody] CriarClienteRequest request)
        {
            try
            {
                var cliente = await _clienteService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registrar-cliente")]
        public async Task<ActionResult> RegistrarCliente([FromBody] RegistrarClienteRequest request)
        {
            try
            {
                var clienteService = HttpContext.RequestServices.GetRequiredService<IClienteService>();

                var criarClienteRequest = new CriarClienteRequest
                {
                    NomeCompleto = request.NomeCompleto,
                    NomeSocial = request.NomeSocial,
                    Sexo = request.Sexo,
                    Telefone = request.Telefone,
                    Email = request.Email,
                    Senha = request.Senha,
                    CEP = request.CEP,
                    Logradouro = request.Logradouro,
                    Numero = request.Numero,
                    Complemento = request.Complemento,
                    Bairro = request.Bairro,
                    Cidade = request.Cidade,
                    UF = request.UF,
                    Pais = request.Pais
                };

                var cliente = await clienteService.CriarAsync(criarClienteRequest);

                // Fazer login automático
                var loginRequest = new LoginRequest
                {
                    Email = request.Email,
                    Senha = request.Senha
                };

                var loginResult = await _authService.AutenticarAsync(loginRequest);

                return Ok(new
                {
                    Mensagem = "Cliente registrado com sucesso",
                    Cliente = cliente,
                    Token = loginResult?.Token,
                    DataExpiracao = loginResult?.DataExpiracao
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteResponse>> Atualizar(Guid id, [FromBody] AtualizarClienteRequest request)
        {
            try
            {
                var cliente = await _clienteService.AtualizarAsync(id, request);

                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                return Ok(cliente);
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
                var resultado = await _clienteService.ExcluirAsync(id);

                if (!resultado)
                    return NotFound("Cliente não encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/vincular-usuario")]
        public async Task<ActionResult> VincularUsuario(Guid id, [FromBody] VincularUsuarioRequest request)
        {
            try
            {
                var resultado = await _clienteService.VincularUsuarioAsync(id, request.UsuarioId);

                if (!resultado)
                    return NotFound("Cliente ou usuário não encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DTO para registro de cliente
        public class RegistrarClienteRequest
        {
            public string NomeCompleto { get; set; }
            public string? NomeSocial { get; set; }
            public string? Sexo { get; set; }
            public string? Telefone { get; set; }
            public string Email { get; set; }
            public string Senha { get; set; }
            public string? CEP { get; set; }
            public string? Logradouro { get; set; }
            public string? Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? UF { get; set; }
            public string? Pais { get; set; }
        }
    }
}