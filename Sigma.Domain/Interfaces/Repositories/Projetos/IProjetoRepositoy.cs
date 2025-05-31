using Sigma.Domain.Entities;

namespace Sigma.Domain.Interfaces.Repositories
{
    public interface IProjetoRepository
    {
        Task<List<Projeto>> BuscarTodos();
        Task<Projeto?> BuscarPorId(long id);
        Task<bool> Inserir(Projeto entidade);
        Task<bool> Alterar(Projeto entidadeAtualizada);
        Task<bool> Deletar(long id);
    }
}
