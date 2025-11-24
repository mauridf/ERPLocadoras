using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Core.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = ERPLocadoras.Core.DTOs.LoginRequest;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
    }
}