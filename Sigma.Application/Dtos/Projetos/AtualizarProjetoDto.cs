using Sigma.Domain.Enums;

namespace Sigma.Application.Dtos.Projeto
{
    public class AtualizarProjetoDto
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public StatusProjeto? Status { get; set; }
    }
}