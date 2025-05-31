using Sigma.Domain.Enums;

namespace Sigma.Application.Dtos.Usuario
{
    public class UsuarioCadastroDto
    {
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public TipoUsuario Tipo { get; set; } = TipoUsuario.Leitor;
    }
}