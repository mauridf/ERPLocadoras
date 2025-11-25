using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Core.Interfaces;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaHasher _senhaHasher;
        private readonly IAuthService _authService;

        public ClienteService(
            ApplicationDbContext context,
            ISenhaHasher senhaHasher,
            IAuthService authService)
        {
            _context = context;
            _senhaHasher = senhaHasher;
            _authService = authService;
        }

        public async Task<ClienteResponse?> ObterPorIdAsync(Guid id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Locacoes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return null;

            return await MapToResponse(cliente);
        }

        public async Task<IEnumerable<ClienteResponse>> ObterTodosAsync()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Locacoes)
                .OrderBy(c => c.NomeCompleto)
                .ToListAsync();

            var responses = new List<ClienteResponse>();
            foreach (var cliente in clientes)
            {
                responses.Add(await MapToResponse(cliente));
            }

            return responses;
        }

        public async Task<ClienteResponse> CriarAsync(CriarClienteRequest request)
        {
            // Criar usuário se email e senha foram fornecidos
            Guid? usuarioId = null;

            if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.Senha))
            {
                var usuario = await _authService.CriarUsuarioAsync(
                    request.Email,
                    request.Senha,
                    UsuarioTipo.Cliente
                );

                if (usuario == null)
                    throw new InvalidOperationException("Não foi possível criar o usuário. Email já existe.");

                usuarioId = usuario.Id;
            }

            var cliente = new Cliente(request.NomeCompleto);

            // Atualizar dados pessoais
            cliente.AtualizarDadosPessoais(
                request.NomeSocial,
                request.Sexo,
                request.Telefone,
                request.FotoUrl
            );

            // Atualizar endereço se fornecido
            if (!string.IsNullOrEmpty(request.CEP))
            {
                cliente.AtualizarEndereco(
                    request.CEP,
                    request.Logradouro,
                    request.Numero,
                    request.Complemento,
                    request.Bairro,
                    request.Cidade,
                    request.UF,
                    request.Pais
                );
            }

            // Vincular usuário se criado
            if (usuarioId.HasValue)
            {
                cliente.VincularUsuario(usuarioId.Value);
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(cliente.Id) ?? throw new Exception("Erro ao criar cliente.");
        }

        public async Task<ClienteResponse?> AtualizarAsync(Guid id, AtualizarClienteRequest request)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return null;

            // Atualizar nome se fornecido - usar o método correto
            if (!string.IsNullOrEmpty(request.NomeCompleto))
                cliente.AtualizarNome(request.NomeCompleto);

            // Atualizar dados pessoais
            cliente.AtualizarDadosPessoais(
                request.NomeSocial,
                request.Sexo,
                request.Telefone,
                request.FotoUrl
            );

            // Atualizar endereço se fornecido
            if (!string.IsNullOrEmpty(request.CEP))
            {
                cliente.AtualizarEndereco(
                    request.CEP,
                    request.Logradouro,
                    request.Numero,
                    request.Complemento,
                    request.Bairro,
                    request.Cidade,
                    request.UF,
                    request.Pais
                );
            }

            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(id);
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Locacoes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return false;

            // Verificar se existem locações ativas
            var temLocacoesAtivas = cliente.Locacoes.Any(l =>
                l.DataCriacao > DateTime.UtcNow.AddMonths(-1)); // Simplificação

            if (temLocacoesAtivas)
                throw new InvalidOperationException("Não é possível excluir cliente com locações ativas ou recentes.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> VincularUsuarioAsync(Guid clienteId, Guid usuarioId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (cliente == null || usuario == null)
                return false;

            // Verificar se o usuário é do tipo Cliente
            if (usuario.Tipo != UsuarioTipo.Cliente)
                throw new InvalidOperationException("Só é possível vincular usuários do tipo Cliente.");

            cliente.VincularUsuario(usuarioId);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Cliente?> ObterEntidadePorIdAsync(Guid id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        private async Task<ClienteResponse> MapToResponse(Cliente cliente)
        {
            var response = new ClienteResponse
            {
                Id = cliente.Id,
                NomeCompleto = cliente.NomeCompleto,
                NomeSocial = cliente.NomeSocial,
                Sexo = cliente.Sexo,
                Telefone = cliente.Telefone,
                DataCadastro = cliente.DataCadastro,
                FotoUrl = cliente.FotoUrl,
                CEP = cliente.CEP,
                Logradouro = cliente.Logradouro,
                Numero = cliente.Numero,
                Complemento = cliente.Complemento,
                Bairro = cliente.Bairro,
                Cidade = cliente.Cidade,
                UF = cliente.UF,
                Pais = cliente.Pais,
                UsuarioId = cliente.UsuarioId,
                Email = cliente.Usuario?.Email,
                TemAcesso = cliente.UsuarioId.HasValue,
                DataCriacao = cliente.DataCriacao,
                DataAtualizacao = cliente.DataAtualizacao
            };

            // Calcular estatísticas
            response.TotalLocacoes = cliente.Locacoes?.Count ?? 0;
            response.LocacoesAtivas = cliente.Locacoes?
                .Count(l => l.DataCriacao > DateTime.UtcNow.AddDays(-30)) ?? 0; // Simplificação

            return response;
        }
    }
}