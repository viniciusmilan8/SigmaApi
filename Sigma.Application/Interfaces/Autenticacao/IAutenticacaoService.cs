using Sigma.Domain.Enums;

namespace Sigma.Application.Interfaces.Autenticacao
{
    public interface IAutenticacaoService
    {
        string GerarToken(string userId, TipoUsuario tipo);
    }
}