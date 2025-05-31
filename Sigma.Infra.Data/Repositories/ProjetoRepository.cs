using Microsoft.EntityFrameworkCore;
using Sigma.Domain.Entities;
using Sigma.Domain.Interfaces.Repositories;

namespace Sigma.Infra.Data.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly SigmaContext _dbContext;

        public ProjetoRepository(SigmaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Projeto>> BuscarTodos()
        {
            return await _dbContext.Projetos.ToListAsync();
        }

        public async Task<Projeto?> BuscarPorId(long id)
        {
            return await _dbContext.Projetos.FindAsync(id);
        }

        public async Task<bool> Inserir(Projeto entidade)
        {
           await _dbContext.Set<Projeto>().AddAsync(entidade);
           await _dbContext.SaveChangesAsync();
           return true;
        }

        public async Task<bool> Alterar(Projeto entidadeAtualizada)
        {
            var projetoExistente = await _dbContext.Projetos.FindAsync(entidadeAtualizada.Id);
            if (projetoExistente == null)
                return false;

            _dbContext.Entry(projetoExistente).CurrentValues.SetValues(entidadeAtualizada);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(long id)
        {
            var projeto = await _dbContext.Projetos.FindAsync(id);
            if (projeto == null)
                return false;

            _dbContext.Projetos.Remove(projeto);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
