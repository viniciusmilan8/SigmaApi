using Sigma.Domain.Enums;

namespace Sigma.Application.Dtos
{
    public class ProjetosDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public StatusProjeto Status { get; set; }
    }
}
