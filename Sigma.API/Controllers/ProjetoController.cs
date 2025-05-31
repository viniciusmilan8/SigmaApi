using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sigma.Application.Dtos.Projeto;
using Sigma.Application.Interfaces.Projeto;

namespace Sigma.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _projetoService;

        public ProjetoController(IProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        [Authorize(Roles = "Admin,Leitor")]
        [HttpGet]
        public async Task<IActionResult> Buscar()
        {
            var projetos = await _projetoService.BuscarTodos();
            return Ok(projetos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("inserir")]
        public async Task<IActionResult> Inserir([FromBody] ProjetoNovoDto model)
        {
            return new JsonResult(await _projetoService.Inserir(model));
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("alterar")]
        public async Task<IActionResult> Alterar([FromBody] AtualizarProjetoDto projetoAtualizado)
        {
            return new JsonResult(await _projetoService.Alterar(projetoAtualizado));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("deletar")]
        public async Task<IActionResult> Deletar([FromBody] long id)
        {
            return new JsonResult(await _projetoService.Deletar(id));
        }
    }
}
