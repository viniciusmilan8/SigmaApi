using Sigma.Domain.Entities;

namespace Sigma.Domain.Interfaces.Repositories
{
    public interface IProjetoRepository
    {
        Task<bool> Inserir(Projeto entidade);

        Task<List<Projeto>> BuscarTodos();
    }
}
