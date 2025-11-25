using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Core.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using static ERPLocadoras.API.Controllers.ClientesController;
using LoginRequest = ERPLocadoras.Core.DTOs.LoginRequest;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClienteService _clienteService;

        public AuthController(IAuthService authService, IClienteService clienteService)
        {
            _authService = authService;
            _clienteService = clienteService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var resultado = await _authService.AutenticarAsync(loginRequest);

            if (resultado == null)
                return Unauthorized("Email ou senha inválidos");

            return Ok(resultado);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarRequest request)
        {
            // Este endpoint será implementado posteriormente para criar usuários iniciais
            return Ok("Registro criado com sucesso");
        }

        [HttpPost("registrar-cliente")]
        public async Task<ActionResult> RegistrarCliente([FromBody] RegistrarClienteRequest request)
        {
            try
            {
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

                var cliente = await _clienteService.CriarAsync(criarClienteRequest);

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