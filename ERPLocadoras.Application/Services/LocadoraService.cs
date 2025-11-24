using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class LocadoraService : ILocadoraService
    {
        private readonly ApplicationDbContext _context;

        public LocadoraService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LocadoraResponse?> ObterPorIdAsync(Guid id)
        {
            var locadora = await _context.Locadoras
                .FirstOrDefaultAsync(l => l.Id == id);

            if (locadora == null)
                return null;

            return MapToResponse(locadora);
        }

        public async Task<IEnumerable<LocadoraResponse>> ObterTodasAsync()
        {
            var locadoras = await _context.Locadoras
                .OrderBy(l => l.NomeFantasia)
                .ToListAsync();

            return locadoras.Select(MapToResponse);
        }

        public async Task<LocadoraResponse> CriarAsync(CriarLocadoraRequest request)
        {
            // Validar CNPJ único
            if (await _context.Locadoras.AnyAsync(l => l.CNPJ == request.CNPJ))
                throw new InvalidOperationException("Já existe uma locadora com este CNPJ.");

            var locadora = new Locadora(
                request.RazaoSocial,
                request.NomeFantasia,
                request.CNPJ,
                StatusLocadora.Ativa
            );

            // Atualizar dados gerais
            locadora.AtualizarDadosGerais(
                request.InscricaoEstadual,
                request.InscricaoMunicipal,
                request.CNAEPrincipal,
                request.RegimeTributario,
                request.DataFundacao,
                request.LogotipoUrl
            );

            // Atualizar endereço
            locadora.AtualizarEndereco(
                request.CEP,
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Bairro,
                request.Cidade,
                request.UF,
                request.Pais,
                request.TipoEndereco
            );

            // Atualizar contato
            locadora.AtualizarContato(
                request.TelefoneFixo,
                request.CelularWhatsApp,
                request.EmailComercial,
                request.Site,
                request.ResponsavelContato,
                request.CargoResponsavel
            );

            // Atualizar configurações operacionais
            locadora.AtualizarConfiguracoesOperacionais(
                request.ModalidadeLocacao,
                request.PadraoCobranca,
                request.PoliticaCaucao,
                request.AceitaTerceiros,
                request.IntegracaoSeguradora,
                request.Seguradora,
                request.TipoFrota
            );

            _context.Locadoras.Add(locadora);
            await _context.SaveChangesAsync();

            return MapToResponse(locadora);
        }

        public async Task<LocadoraResponse?> AtualizarAsync(Guid id, AtualizarLocadoraRequest request)
        {
            var locadora = await _context.Locadoras.FindAsync(id);
            if (locadora == null)
                return null;

            // Agora do jeito certo
            locadora.AtualizarDadosBasicos(
                request.RazaoSocial,
                request.NomeFantasia
            );

            // Atualizar dados gerais
            locadora.AtualizarDadosGerais(
                request.InscricaoEstadual,
                request.InscricaoMunicipal,
                request.CNAEPrincipal,
                request.RegimeTributario,
                request.DataFundacao,
                request.LogotipoUrl
            );

            // Atualizar endereço
            locadora.AtualizarEndereco(
                request.CEP,
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Bairro,
                request.Cidade,
                request.UF,
                request.Pais,
                request.TipoEndereco
            );

            // Atualizar contato
            locadora.AtualizarContato(
                request.TelefoneFixo,
                request.CelularWhatsApp,
                request.EmailComercial,
                request.Site,
                request.ResponsavelContato,
                request.CargoResponsavel
            );

            // Atualizar status se fornecido
            if (request.Status.HasValue)
                locadora.AlterarStatus(request.Status.Value);

            await _context.SaveChangesAsync();

            return MapToResponse(locadora);
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var locadora = await _context.Locadoras.FindAsync(id);
            if (locadora == null)
                return false;

            // Verificar se existem dependências antes de excluir
            var temUsuarios = await _context.Usuarios.AnyAsync(u => u.LocadoraId == id);
            var temVeiculos = await _context.Veiculos.AnyAsync(v => v.LocadoraId == id);

            if (temUsuarios || temVeiculos)
            {
                // Em vez de excluir, marcar como inativa
                locadora.AlterarStatus(StatusLocadora.Inativa);
                await _context.SaveChangesAsync();
                return true;
            }

            _context.Locadoras.Remove(locadora);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarStatusAsync(Guid id, StatusLocadora novoStatus)
        {
            var locadora = await _context.Locadoras.FindAsync(id);
            if (locadora == null)
                return false;

            locadora.AlterarStatus(novoStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Locadora?> ObterEntidadePorIdAsync(Guid id)
        {
            return await _context.Locadoras.FindAsync(id);
        }

        private LocadoraResponse MapToResponse(Locadora locadora)
        {
            return new LocadoraResponse
            {
                Id = locadora.Id,
                RazaoSocial = locadora.RazaoSocial,
                NomeFantasia = locadora.NomeFantasia,
                CNPJ = locadora.CNPJ,
                InscricaoEstadual = locadora.InscricaoEstadual,
                InscricaoMunicipal = locadora.InscricaoMunicipal,
                CNAEPrincipal = locadora.CNAEPrincipal,
                RegimeTributario = locadora.RegimeTributario,
                DataFundacao = locadora.DataFundacao,
                Status = locadora.Status,
                LogotipoUrl = locadora.LogotipoUrl,

                CEP = locadora.CEP,
                Logradouro = locadora.Logradouro,
                Numero = locadora.Numero,
                Complemento = locadora.Complemento,
                Bairro = locadora.Bairro,
                Cidade = locadora.Cidade,
                UF = locadora.UF,
                Pais = locadora.Pais,
                TipoEndereco = locadora.TipoEndereco,

                TelefoneFixo = locadora.TelefoneFixo,
                CelularWhatsApp = locadora.CelularWhatsApp,
                EmailComercial = locadora.EmailComercial,
                Site = locadora.Site,
                ResponsavelContato = locadora.ResponsavelContato,
                CargoResponsavel = locadora.CargoResponsavel,

                ModalidadeLocacao = locadora.ModalidadeLocacao,
                PadraoCobranca = locadora.PadraoCobranca,
                PoliticaCaucao = locadora.PoliticaCaucao,
                AceitaTerceiros = locadora.AceitaTerceiros,
                IntegracaoSeguradora = locadora.IntegracaoSeguradora,
                Seguradora = locadora.Seguradora,
                TipoFrota = locadora.TipoFrota,

                DataCriacao = locadora.DataCriacao,
                DataAtualizacao = locadora.DataAtualizacao,

                // Estatísticas (serão implementadas posteriormente)
                TotalUsuarios = 0,
                TotalVeiculos = 0,
                TotalLocacoesAtivas = 0
            };
        }
    }
}