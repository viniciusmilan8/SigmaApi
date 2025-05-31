using Sigma.Application.Dtos.Usuario;

namespace Sigma.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<string?> Login(UsuarioLoginDto login);
        Task<bool> Cadastrar(UsuarioCadastroDto dto);
    }
}