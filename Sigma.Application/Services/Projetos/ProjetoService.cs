using AutoMapper;
using Sigma.Application.Dtos.Projeto;
using Sigma.Application.Interfaces.Projeto;
using Sigma.Domain.Entities;
using Sigma.Domain.Enums;
using Sigma.Domain.Interfaces.Repositories;

namespace Sigma.Application.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly IMapper _mapper;
        private readonly IProjetoRepository _projetoRepository;

        public ProjetoService(IMapper mapper, IProjetoRepository projetoRepository)
        {
            _mapper = mapper;
            _projetoRepository = projetoRepository;
        }

        #region Métodos públicos
        public async Task<List<ProjetosDto>> BuscarTodos()
        {
            var projetos = await _projetoRepository.BuscarTodos();
            return _mapper.Map<List<ProjetosDto>>(projetos);
        }

        public async Task<bool> Inserir(ProjetoNovoDto model)
        {
            return await _projetoRepository.Inserir(_mapper.Map<Projeto>(model));
        }

        public async Task<bool> Alterar(AtualizarProjetoDto projetoAtualizado)
        {
            var projetoExistente = await _projetoRepository.BuscarPorId(projetoAtualizado.Id);
            if (projetoExistente == null)
                return false;

            // Regra de negócio: não permitir alterar projeto já concluído
            if (projetoExistente.Status == StatusProjeto.Encerrado && projetoAtualizado.Status != StatusProjeto.Encerrado)
                throw new InvalidOperationException("Não é possível reabrir um projeto concluído.");

            if (!string.IsNullOrWhiteSpace(projetoAtualizado.Nome))
                projetoExistente.Nome = projetoAtualizado.Nome;

            if (projetoAtualizado.Status.HasValue)
                projetoExistente.Status = projetoAtualizado.Status.Value;

            return await _projetoRepository.Alterar(projetoExistente);
        }

        public async Task<bool> Deletar(long id)
        {
            return await _projetoRepository.Deletar(id);
        }
        #endregion
    }
}