using Sigma.Domain.Enums;

namespace Sigma.Domain.Entities
{
    public class Projeto
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public StatusProjeto Status { get; set; }
    }
}
