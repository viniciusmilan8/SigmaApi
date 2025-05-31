using Sigma.Application.Dtos.Projeto;
using Sigma.Domain.Entities;

namespace Sigma.Application.Interfaces.Projeto
{
    public interface IProjetoService
    {
        Task<List<ProjetosDto>> BuscarTodos();
        Task<bool> Inserir(ProjetoNovoDto model);
        Task<bool> Alterar(AtualizarProjetoDto projetoAtualizado);
        Task<bool> Deletar(long id);
    }
}