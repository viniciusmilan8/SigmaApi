using Sigma.Domain.Enums;

namespace Sigma.Domain.Entities
{
    public class Usuario
    {
        public long Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SenhaHash { get; set; } = null!;
        public TipoUsuario Tipo { get; set; } = TipoUsuario.Leitor;
    }
}