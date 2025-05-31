using Microsoft.AspNetCore.Mvc;
using Sigma.Application.Dtos.Usuario;
using Sigma.Application.Interfaces;

namespace Sigma.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AutenticacaoController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
        {
            var token = await _usuarioService.Login(dto);
            if (token == null)
                return Unauthorized("Usuário ou senha inválidos.");

            return Ok(new { token });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioCadastroDto dto)
        {
            var sucesso = await _usuarioService.Cadastrar(dto);
            if (!sucesso)
                return BadRequest("Usuário já existente.");

            return Ok("Usuário cadastrado com sucesso.");
        }
    }
}