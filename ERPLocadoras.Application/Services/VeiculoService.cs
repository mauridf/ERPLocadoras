using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly ApplicationDbContext _context;

        public VeiculoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VeiculoResponse?> ObterPorIdAsync(Guid id)
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.Locadora)
                .Include(v => v.Locacoes)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
                return null;

            return await MapToResponse(veiculo);
        }

        public async Task<IEnumerable<VeiculoResponse>> ObterTodosAsync()
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Locadora)
                .Include(v => v.Locacoes)
                .OrderBy(v => v.Marca)
                .ThenBy(v => v.Modelo)
                .ToListAsync();

            var responses = new List<VeiculoResponse>();
            foreach (var veiculo in veiculos)
            {
                responses.Add(await MapToResponse(veiculo));
            }

            return responses;
        }

        public async Task<IEnumerable<VeiculoResponse>> ObterPorLocadoraAsync(Guid locadoraId)
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Locadora)
                .Include(v => v.Locacoes)
                .Where(v => v.LocadoraId == locadoraId)
                .OrderBy(v => v.Marca)
                .ThenBy(v => v.Modelo)
                .ToListAsync();

            var responses = new List<VeiculoResponse>();
            foreach (var veiculo in veiculos)
            {
                responses.Add(await MapToResponse(veiculo));
            }

            return responses;
        }

        public async Task<IEnumerable<VeiculoResponse>> ObterDisponiveisPorLocadoraAsync(Guid locadoraId)
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Locadora)
                .Include(v => v.Locacoes)
                .Where(v => v.LocadoraId == locadoraId && v.EstaDisponivelParaLocacao())
                .OrderBy(v => v.Marca)
                .ThenBy(v => v.Modelo)
                .ToListAsync();

            var responses = new List<VeiculoResponse>();
            foreach (var veiculo in veiculos)
            {
                responses.Add(await MapToResponse(veiculo));
            }

            return responses;
        }

        public async Task<VeiculoResponse> CriarAsync(CriarVeiculoRequest request)
        {
            // Validar placa única
            if (await _context.Veiculos.AnyAsync(v => v.Placa == request.Placa))
                throw new InvalidOperationException("Já existe um veículo com esta placa.");

            // Validar renavam único
            if (await _context.Veiculos.AnyAsync(v => v.Renavam == request.Renavam))
                throw new InvalidOperationException("Já existe um veículo com este RENAVAM.");

            // Validar chassi único
            if (await _context.Veiculos.AnyAsync(v => v.Chassi == request.Chassi))
                throw new InvalidOperationException("Já existe um veículo com este chassi.");

            // Validar locadora existe
            var locadoraExiste = await _context.Locadoras.AnyAsync(l => l.Id == request.LocadoraId);
            if (!locadoraExiste)
                throw new InvalidOperationException("Locadora não encontrada.");

            var veiculo = new Veiculo(
                request.Tipo,
                request.Marca,
                request.Modelo,
                request.AnoFabricacao,
                request.AnoModelo,
                request.Placa,
                request.Renavam,
                request.Chassi,
                request.Cor,
                request.Categoria,
                request.Combustivel,
                request.QuilometragemAtual,
                request.DataAquisicao,
                request.ValorCompra,
                request.LocadoraId
            );

            // Atualizar dados adicionais
            veiculo.AtualizarDadosBasicos(
                request.VersaoMotorizacao,
                request.Capacidade,
                request.Observacoes
            );

            if (request.ValorMercadoAtual.HasValue)
                veiculo.AtualizarValorMercado(request.ValorMercadoAtual.Value);

            if (!string.IsNullOrEmpty(request.ApoliceSeguro))
            {
                veiculo.AtualizarDadosSeguro(
                    request.ApoliceSeguro,
                    request.Seguradora,
                    request.VencimentoSeguro
                );
            }

            if (!string.IsNullOrEmpty(request.Documentacao))
                veiculo.AtualizarDocumentacao(request.Documentacao);

            if (request.DataUltimaRevisao.HasValue || request.DataProximaRevisao.HasValue)
            {
                veiculo.AtualizarManutencao(
                    request.DataUltimaRevisao,
                    request.DataProximaRevisao
                );
            }

            if (!string.IsNullOrEmpty(request.FotosAnexos))
                veiculo.AtualizarFotosAnexos(request.FotosAnexos);

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(veiculo.Id) ?? throw new Exception("Erro ao criar veículo.");
        }

        public async Task<VeiculoResponse?> AtualizarAsync(Guid id, AtualizarVeiculoRequest request)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return null;

            // Atualizar dados básicos
            veiculo.AtualizarDadosBasicos(
                request.VersaoMotorizacao,
                request.Capacidade,
                request.Observacoes
            );

            // Atualizar valor de mercado
            if (request.ValorMercadoAtual.HasValue)
                veiculo.AtualizarValorMercado(request.ValorMercadoAtual.Value);

            // Atualizar seguro
            if (request.ApoliceSeguro != null || request.Seguradora != null || request.VencimentoSeguro.HasValue)
            {
                veiculo.AtualizarDadosSeguro(
                    request.ApoliceSeguro,
                    request.Seguradora,
                    request.VencimentoSeguro
                );
            }

            // Atualizar documentação
            if (request.Documentacao != null)
                veiculo.AtualizarDocumentacao(request.Documentacao);

            // Atualizar manutenção
            if (request.DataUltimaRevisao.HasValue || request.DataProximaRevisao.HasValue)
            {
                veiculo.AtualizarManutencao(
                    request.DataUltimaRevisao,
                    request.DataProximaRevisao
                );
            }

            // Atualizar fotos/anexos
            if (request.FotosAnexos != null)
                veiculo.AtualizarFotosAnexos(request.FotosAnexos);

            // Atualizar status
            if (request.Status.HasValue)
                veiculo.AlterarStatus(request.Status.Value);

            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(id);
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.Locacoes)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
                return false;

            // Verificar se existem locações ativas
            var temLocacoesAtivas = veiculo.Locacoes.Any(l =>
                l.DataCriacao > DateTime.UtcNow.AddMonths(-1)); // Simplificação

            if (temLocacoesAtivas)
                throw new InvalidOperationException("Não é possível excluir veículo com locações ativas ou recentes.");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AtualizarQuilometragemAsync(Guid id, decimal novaQuilometragem)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return false;

            try
            {
                veiculo.AtualizarQuilometragem(novaQuilometragem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> AlterarStatusAsync(Guid id, StatusVeiculo novoStatus)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return false;

            veiculo.AlterarStatus(novoStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Veiculo?> ObterEntidadePorIdAsync(Guid id)
        {
            return await _context.Veiculos.FindAsync(id);
        }

        private async Task<VeiculoResponse> MapToResponse(Veiculo veiculo)
        {
            var response = new VeiculoResponse
            {
                Id = veiculo.Id,
                Tipo = veiculo.Tipo,
                Marca = veiculo.Marca,
                Modelo = veiculo.Modelo,
                VersaoMotorizacao = veiculo.VersaoMotorizacao,
                AnoFabricacao = veiculo.AnoFabricacao,
                AnoModelo = veiculo.AnoModelo,
                Placa = veiculo.Placa,
                Renavam = veiculo.Renavam,
                Chassi = veiculo.Chassi,
                Cor = veiculo.Cor,
                Categoria = veiculo.Categoria,
                Combustivel = veiculo.Combustivel,
                QuilometragemAtual = veiculo.QuilometragemAtual,
                Capacidade = veiculo.Capacidade,
                Status = veiculo.Status,
                DataAquisicao = veiculo.DataAquisicao,
                ValorCompra = veiculo.ValorCompra,
                ValorMercadoAtual = veiculo.ValorMercadoAtual,
                ApoliceSeguro = veiculo.ApoliceSeguro,
                Seguradora = veiculo.Seguradora,
                VencimentoSeguro = veiculo.VencimentoSeguro,
                Documentacao = veiculo.Documentacao,
                DataUltimaRevisao = veiculo.DataUltimaRevisao,
                DataProximaRevisao = veiculo.DataProximaRevisao,
                Observacoes = veiculo.Observacoes,
                FotosAnexos = veiculo.FotosAnexos,
                LocadoraId = veiculo.LocadoraId,
                LocadoraNome = veiculo.Locadora?.NomeFantasia,
                DataCriacao = veiculo.DataCriacao,
                DataAtualizacao = veiculo.DataAtualizacao
            };

            // Calcular disponibilidade
            response.DisponivelParaLocacao = veiculo.EstaDisponivelParaLocacao();
            response.MotivoIndisponibilidade = ObterMotivoIndisponibilidade(veiculo);

            // Calcular estatísticas
            response.TotalLocacoes = veiculo.Locacoes?.Count ?? 0;
            response.LocacoesAtivas = veiculo.Locacoes?
                .Count(l => l.DataCriacao > DateTime.UtcNow.AddDays(-30)) ?? 0; // Simplificação

            return response;
        }

        private string? ObterMotivoIndisponibilidade(Veiculo veiculo)
        {
            if (veiculo.Status != StatusVeiculo.Disponivel)
                return $"Status: {veiculo.Status}";

            if (veiculo.VencimentoSeguro.HasValue && veiculo.VencimentoSeguro <= DateTime.UtcNow)
                return "Seguro vencido";

            if (veiculo.DataProximaRevisao.HasValue && veiculo.DataProximaRevisao <= DateTime.UtcNow.AddDays(7))
                return "Revisão próxima";

            return null;
        }
    }
}