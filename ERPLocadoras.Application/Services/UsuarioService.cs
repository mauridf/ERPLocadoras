using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Core.Interfaces;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaHasher _senhaHasher;

        public UsuarioService(ApplicationDbContext context, ISenhaHasher senhaHasher)
        {
            _context = context;
            _senhaHasher = senhaHasher;
        }

        public async Task<UsuarioResponse?> ObterPorIdAsync(Guid id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Locadora)
                .Include(u => u.Pessoa)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return null;

            return MapToResponse(usuario);
        }

        public async Task<IEnumerable<UsuarioResponse>> ObterTodosAsync()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Locadora)
                .Include(u => u.Pessoa)
                .OrderBy(u => u.Email)
                .ToListAsync();

            return usuarios.Select(MapToResponse);
        }

        public async Task<IEnumerable<UsuarioResponse>> ObterPorLocadoraAsync(Guid locadoraId)
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Locadora)
                .Include(u => u.Pessoa)
                .Where(u => u.LocadoraId == locadoraId)
                .OrderBy(u => u.Email)
                .ToListAsync();

            return usuarios.Select(MapToResponse);
        }

        public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request)
        {
            // Validar email único
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Já existe um usuário com este email.");

            // Validar locadora para usuários não-globais
            if (request.Tipo != UsuarioTipo.Global && !request.LocadoraId.HasValue)
                throw new InvalidOperationException("Usuários não-globais devem ter uma locadora vinculada.");

            // Validar locadora existe
            if (request.LocadoraId.HasValue)
            {
                var locadoraExiste = await _context.Locadoras.AnyAsync(l => l.Id == request.LocadoraId.Value);
                if (!locadoraExiste)
                    throw new InvalidOperationException("Locadora não encontrada.");
            }

            var senhaHash = _senhaHasher.HashSenha(request.Senha);
            var usuario = new Usuario(
                request.Email,
                senhaHash,
                request.Tipo,
                true,
                request.LocadoraId
            );

            if (!string.IsNullOrEmpty(request.Permissoes))
                usuario.AtualizarPermissoes(request.Permissoes);

            if (request.DataExpiracao.HasValue)
                usuario.DefinirDataExpiracao(request.DataExpiracao.Value);

            _context.Usuarios.Add(usuario);

            // Criar dados pessoais se fornecidos (exceto para clientes)
            if (request.DadosPessoais != null && request.Tipo != UsuarioTipo.Cliente)
            {
                var pessoa = new Pessoa(request.DadosPessoais.NomeCompleto, usuario.Id);

                pessoa.AtualizarDadosPessoais(
                    request.DadosPessoais.NomeSocial,
                    request.DadosPessoais.Sexo,
                    request.DadosPessoais.Telefone,
                    request.DadosPessoais.DataNascimento,
                    request.DadosPessoais.FotoUrl
                );

                if (!string.IsNullOrEmpty(request.DadosPessoais.CEP))
                {
                    pessoa.AtualizarEndereco(
                        request.DadosPessoais.CEP,
                        request.DadosPessoais.Logradouro,
                        request.DadosPessoais.Numero,
                        request.DadosPessoais.Complemento,
                        request.DadosPessoais.Bairro,
                        request.DadosPessoais.Cidade,
                        request.DadosPessoais.UF,
                        request.DadosPessoais.Pais
                    );
                }

                _context.Pessoas.Add(pessoa);
            }

            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(usuario.Id) ?? throw new Exception("Erro ao criar usuário.");
        }

        public async Task<UsuarioResponse?> AtualizarAsync(Guid id, AtualizarUsuarioRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Pessoa)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return null;

            // Atualizar email se fornecido
            if (!string.IsNullOrEmpty(request.Email) && request.Email != usuario.Email)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email && u.Id != id))
                    throw new InvalidOperationException("Já existe um usuário com este email.");

                usuario.AtualizarEmail(request.Email);
            }

            // Atualizar senha se fornecida
            if (!string.IsNullOrEmpty(request.Senha))
            {
                var novaSenhaHash = _senhaHasher.HashSenha(request.Senha);
                usuario.AtualizarSenha(novaSenhaHash);
            }

            // Atualizar permissões se fornecidas
            if (request.Permissoes != null)
                usuario.AtualizarPermissoes(request.Permissoes);

            // Atualizar data de expiração se fornecida
            if (request.DataExpiracao.HasValue)
                usuario.DefinirDataExpiracao(request.DataExpiracao.Value);

            // Atualizar status se fornecido
            if (request.Ativo.HasValue)
                usuario.AlterarStatus(request.Ativo.Value);

            // Atualizar dados pessoais se fornecidos
            if (request.DadosPessoais != null && usuario.Pessoa != null)
            {
                if (!string.IsNullOrEmpty(request.DadosPessoais.NomeCompleto))
                    usuario.Pessoa.AtualizarNomeCompleto(request.DadosPessoais.NomeCompleto);

                usuario.Pessoa.AtualizarDadosPessoais(
                    request.DadosPessoais.NomeSocial,
                    request.DadosPessoais.Sexo,
                    request.DadosPessoais.Telefone,
                    request.DadosPessoais.DataNascimento,
                    request.DadosPessoais.FotoUrl
                );

                if (!string.IsNullOrEmpty(request.DadosPessoais.CEP))
                {
                    usuario.Pessoa.AtualizarEndereco(
                        request.DadosPessoais.CEP,
                        request.DadosPessoais.Logradouro,
                        request.DadosPessoais.Numero,
                        request.DadosPessoais.Complemento,
                        request.DadosPessoais.Bairro,
                        request.DadosPessoais.Cidade,
                        request.DadosPessoais.UF,
                        request.DadosPessoais.Pais
                    );
                }
            }

            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(id);
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Pessoa)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return false;

            // Não permitir exclusão de usuário global
            if (usuario.Tipo == UsuarioTipo.Global)
                throw new InvalidOperationException("Não é possível excluir usuários globais.");

            // Em vez de excluir, desativar o usuário
            usuario.AlterarStatus(false);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AlterarStatusAsync(Guid id, bool ativo)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            usuario.AlterarStatus(ativo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> ObterEntidadePorIdAsync(Guid id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        private UsuarioResponse MapToResponse(Usuario usuario)
        {
            var response = new UsuarioResponse
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Tipo = usuario.Tipo,
                Permissoes = usuario.Permissoes,
                DataExpiracao = usuario.DataExpiracao,
                Ativo = usuario.Ativo,
                LocadoraId = usuario.LocadoraId,
                LocadoraNome = usuario.Locadora?.NomeFantasia,
                DataCriacao = usuario.DataCriacao,
                DataAtualizacao = usuario.DataAtualizacao
            };

            if (usuario.Pessoa != null)
            {
                response.DadosPessoais = new PessoaResponse
                {
                    Id = usuario.Pessoa.Id,
                    NomeCompleto = usuario.Pessoa.NomeCompleto,
                    NomeSocial = usuario.Pessoa.NomeSocial,
                    Sexo = usuario.Pessoa.Sexo,
                    Telefone = usuario.Pessoa.Telefone,
                    DataNascimento = usuario.Pessoa.DataNascimento,
                    DataCadastro = usuario.Pessoa.DataCadastro,
                    FotoUrl = usuario.Pessoa.FotoUrl,
                    CEP = usuario.Pessoa.CEP,
                    Logradouro = usuario.Pessoa.Logradouro,
                    Numero = usuario.Pessoa.Numero,
                    Complemento = usuario.Pessoa.Complemento,
                    Bairro = usuario.Pessoa.Bairro,
                    Cidade = usuario.Pessoa.Cidade,
                    UF = usuario.Pessoa.UF,
                    Pais = usuario.Pessoa.Pais,
                    DataCriacao = usuario.Pessoa.DataCriacao,
                    DataAtualizacao = usuario.Pessoa.DataAtualizacao
                };
            }

            return response;
        }
    }
}