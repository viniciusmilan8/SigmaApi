using Sigma.Domain.Entities;

namespace Sigma.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorEmail(string email);
        Task<bool> Inserir(Usuario usuario);
    }
}