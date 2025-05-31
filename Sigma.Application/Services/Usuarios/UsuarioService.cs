using Sigma.Application.Dtos.Usuario;
using Sigma.Application.Interfaces;
using Sigma.Application.Interfaces.Autenticacao;
using Sigma.Domain.Entities;
using Sigma.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Sigma.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAutenticacaoService _autenticacaoService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IAutenticacaoService autenticacaoService)
        {
            _usuarioRepository = usuarioRepository;
            _autenticacaoService = autenticacaoService;
        }

        public async Task<string?> Login(UsuarioLoginDto login)
        {
            var usuario = await _usuarioRepository.BuscarPorEmail(login.Email);
            if (usuario == null) return null;

            string hash = HashSenha(login.Senha);
            if (usuario.SenhaHash != hash) return null;

            return _autenticacaoService.GerarToken(usuario.Email, usuario.Tipo);
        }

        public async Task<bool> Cadastrar(UsuarioCadastroDto dto)
        {
            var existente = await _usuarioRepository.BuscarPorEmail(dto.Email);
            if (existente != null) return false;

            var novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = HashSenha(dto.Senha),
                Tipo = dto.Tipo
            };

            return await _usuarioRepository.Inserir(novoUsuario);
        }

        private static string HashSenha(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}