using AutoMapper;
using Sigma.Application.Dtos.Projeto;
using Sigma.Domain.Entities;

namespace Sigma.Application.Mappers
{
    public class ProjetoMapper : Profile
    {
        public ProjetoMapper()
        {
            CreateMap<ProjetoNovoDto, Projeto>();
            CreateMap<Projeto, ProjetosDto>();
        }
    }
}