using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ApplicationDbContext _context;

        public LocacaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LocacaoResponse?> ObterPorIdAsync(Guid id)
        {
            var locacao = await _context.Locacoes
                .Include(l => l.Locadora)
                .Include(l => l.Veiculo)
                .Include(l => l.Cliente)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (locacao == null)
                return null;

            return await MapToResponse(locacao);
        }

        public async Task<IEnumerable<LocacaoResponse>> ObterTodasAsync()
        {
            var locacoes = await _context.Locacoes
                .Include(l => l.Locadora)
                .Include(l => l.Veiculo)
                .Include(l => l.Cliente)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();

            var responses = new List<LocacaoResponse>();
            foreach (var locacao in locacoes)
            {
                responses.Add(await MapToResponse(locacao));
            }

            return responses;
        }

        public async Task<IEnumerable<LocacaoResponse>> ObterPorLocadoraAsync(Guid locadoraId)
        {
            var locacoes = await _context.Locacoes
                .Include(l => l.Locadora)
                .Include(l => l.Veiculo)
                .Include(l => l.Cliente)
                .Where(l => l.LocadoraId == locadoraId)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();

            var responses = new List<LocacaoResponse>();
            foreach (var locacao in locacoes)
            {
                responses.Add(await MapToResponse(locacao));
            }

            return responses;
        }

        public async Task<IEnumerable<LocacaoResponse>> ObterPorClienteAsync(Guid clienteId)
        {
            var locacoes = await _context.Locacoes
                .Include(l => l.Locadora)
                .Include(l => l.Veiculo)
                .Include(l => l.Cliente)
                .Where(l => l.ClienteId == clienteId)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();

            var responses = new List<LocacaoResponse>();
            foreach (var locacao in locacoes)
            {
                responses.Add(await MapToResponse(locacao));
            }

            return responses;
        }

        public async Task<IEnumerable<LocacaoResponse>> ObterAtivasPorLocadoraAsync(Guid locadoraId)
        {
            var locacoes = await _context.Locacoes
                .Include(l => l.Locadora)
                .Include(l => l.Veiculo)
                .Include(l => l.Cliente)
                .Where(l => l.LocadoraId == locadoraId && l.EstaAtiva())
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();

            var responses = new List<LocacaoResponse>();
            foreach (var locacao in locacoes)
            {
                responses.Add(await MapToResponse(locacao));
            }

            return responses;
        }

        public async Task<LocacaoResponse> CriarAsync(CriarLocacaoRequest request)
        {
            // Validar se veículo existe e está disponível
            var veiculo = await _context.Veiculos
                .Include(v => v.Locacoes)
                .FirstOrDefaultAsync(v => v.Id == request.VeiculoId);

            if (veiculo == null)
                throw new InvalidOperationException("Veículo não encontrado.");

            if (!veiculo.EstaDisponivelParaLocacao())
                throw new InvalidOperationException("Veículo não está disponível para locação.");

            // Validar se cliente existe
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == request.ClienteId);
            if (!clienteExiste)
                throw new InvalidOperationException("Cliente não encontrado.");

            // Validar se locadora existe
            var locadoraExiste = await _context.Locadoras.AnyAsync(l => l.Id == request.LocadoraId);
            if (!locadoraExiste)
                throw new InvalidOperationException("Locadora não encontrada.");

            // Validar datas
            if (request.DataInicio < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Data de início não pode ser no passado.");

            if (request.DataPrevistaDevolucao <= request.DataInicio)
                throw new InvalidOperationException("Data prevista de devolução deve ser após a data de início.");

            var locacao = new Locacao(
                request.DataInicio,
                request.DataPrevistaDevolucao,
                request.TipoLocacao,
                request.ValorDiaria,
                request.FormaCobranca,
                request.FormaCaucao,
                request.ValorTotalPrevisto,
                request.KmEntrega,
                request.LocadoraId,
                request.VeiculoId,
                request.ClienteId
            );

            // Atualizar plano de locação
            if (!string.IsNullOrEmpty(request.PlanoLocacao) || request.ValorKmAdicional.HasValue || request.FranquiaKmInclusa.HasValue)
            {
                locacao.AtualizarPlanoLocacao(
                    request.PlanoLocacao,
                    request.ValorKmAdicional,
                    request.FranquiaKmInclusa
                );
            }

            // Atualizar caução
            if (request.ValorCaucao.HasValue)
            {
                locacao.AtualizarCaucao(request.ValorCaucao.Value, request.FormaCaucao);
            }

            // Atualizar observações
            if (!string.IsNullOrEmpty(request.ObservacoesInternas))
            {
                locacao.AtualizarObservacoes(request.ObservacoesInternas, null);
            }

            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(locacao.Id) ?? throw new Exception("Erro ao criar locação.");
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return false;

            // Só permite excluir locações reservadas
            if (locacao.Situacao != SituacaoLocacao.Reservada)
                throw new InvalidOperationException("Só é possível excluir locações com status Reservada.");

            _context.Locacoes.Remove(locacao);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IniciarLocacaoAsync(Guid id, ChecklistEntregaRequest checklist)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return false;

            try
            {
                locacao.AtualizarChecklistEntrega(
                    checklist.Checklist,
                    checklist.NivelCombustivel,
                    checklist.Responsavel
                );

                locacao.IniciarLocacao();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> FinalizarLocacaoAsync(Guid id, FinalizarLocacaoRequest request)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return false;

            try
            {
                locacao.AtualizarChecklistDevolucao(
                    request.ChecklistDevolucao,
                    request.NivelCombustivelDevolucao,
                    request.ResponsavelDevolucao
                );

                locacao.FinalizarLocacao(
                    request.DataRealDevolucao,
                    request.KmDevolucao,
                    request.ValorTotalFinal,
                    request.DescontosAcrescimos
                );

                await _context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> CancelarLocacaoAsync(Guid id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return false;

            try
            {
                locacao.CancelarLocacao();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> MarcarComoAtrasoAsync(Guid id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return false;

            try
            {
                locacao.MarcarComoAtraso();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<Locacao?> ObterEntidadePorIdAsync(Guid id)
        {
            return await _context.Locacoes.FindAsync(id);
        }

        private async Task<LocacaoResponse> MapToResponse(Locacao locacao)
        {
            var response = new LocacaoResponse
            {
                Id = locacao.Id,
                DataInicio = locacao.DataInicio,
                DataPrevistaDevolucao = locacao.DataPrevistaDevolucao,
                DataRealDevolucao = locacao.DataRealDevolucao,
                TipoLocacao = locacao.TipoLocacao,
                PlanoLocacao = locacao.PlanoLocacao,
                ValorDiaria = locacao.ValorDiaria,
                ValorKmAdicional = locacao.ValorKmAdicional,
                FranquiaKmInclusa = locacao.FranquiaKmInclusa,
                FormaCobranca = locacao.FormaCobranca,
                ValorCaucao = locacao.ValorCaucao,
                FormaCaucao = locacao.FormaCaucao,
                ValorTotalPrevisto = locacao.ValorTotalPrevisto,
                ValorTotalFinal = locacao.ValorTotalFinal,
                DescontosAcrescimos = locacao.DescontosAcrescimos,
                Situacao = locacao.Situacao,
                ResponsavelEntrega = locacao.ResponsavelEntrega,
                ResponsavelDevolucao = locacao.ResponsavelDevolucao,
                ChecklistEntrega = locacao.ChecklistEntrega,
                ChecklistDevolucao = locacao.ChecklistDevolucao,
                NivelCombustivelEntrega = locacao.NivelCombustivelEntrega,
                NivelCombustivelDevolucao = locacao.NivelCombustivelDevolucao,
                KmEntrega = locacao.KmEntrega,
                KmDevolucao = locacao.KmDevolucao,
                ObservacoesInternas = locacao.ObservacoesInternas,
                Anexos = locacao.Anexos,
                LocadoraId = locacao.LocadoraId,
                VeiculoId = locacao.VeiculoId,
                ClienteId = locacao.ClienteId,
                LocadoraNome = locacao.Locadora?.NomeFantasia,
                VeiculoDescricao = $"{locacao.Veiculo?.Marca} {locacao.Veiculo?.Modelo} - {locacao.Veiculo?.Placa}",
                ClienteNome = locacao.Cliente?.NomeCompleto,
                DataCriacao = locacao.DataCriacao,
                DataAtualizacao = locacao.DataAtualizacao
            };

            // Informações calculadas
            response.EstaAtiva = locacao.EstaAtiva();
            response.EstaEmAtraso = locacao.EstaEmAtraso();
            response.DiasLocacao = locacao.CalcularDiasLocacao();
            response.KmRodado = locacao.CalcularKmRodado();
            response.ValorExcedenteKm = locacao.CalcularValorExcedenteKm();
            response.ValorTotalComExcedente = response.ValorTotalFinal + response.ValorExcedenteKm;

            return response;
        }

        public async Task<LocacaoResponse?> AtualizarAsync(Guid id, AtualizarLocacaoRequest request)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return null;

            // Só permite atualizar locações reservadas
            if (locacao.Situacao != SituacaoLocacao.Reservada)
                throw new InvalidOperationException("Só é possível atualizar locações com status Reservada.");

            // Atualizar data prevista de devolução
            if (request.DataPrevistaDevolucao.HasValue)
            {
                locacao.AtualizarDataPrevistaDevolucao(request.DataPrevistaDevolucao.Value);
            }

            // Atualizar plano de locação
            if (request.PlanoLocacao != null || request.ValorKmAdicional.HasValue || request.FranquiaKmInclusa.HasValue)
            {
                locacao.AtualizarPlanoLocacao(
                    request.PlanoLocacao,
                    request.ValorKmAdicional,
                    request.FranquiaKmInclusa
                );
            }

            // Atualizar valores - USAR OS MÉTODOS DA ENTIDADE
            if (request.ValorDiaria.HasValue)
                locacao.AtualizarValorDiaria(request.ValorDiaria.Value);

            if (request.ValorCaucao.HasValue && request.FormaCaucao.HasValue)
            {
                locacao.AtualizarCaucao(request.ValorCaucao.Value, request.FormaCaucao.Value);
            }

            if (request.ValorTotalPrevisto.HasValue)
                locacao.AtualizarValorTotalPrevisto(request.ValorTotalPrevisto.Value);

            // Atualizar forma de cobrança se fornecida
            if (request.FormaCobranca.HasValue)
                locacao.AtualizarFormaCobranca(request.FormaCobranca.Value);

            // Atualizar observações e anexos
            if (request.ObservacoesInternas != null || request.Anexos != null)
            {
                locacao.AtualizarObservacoes(
                    request.ObservacoesInternas ?? locacao.ObservacoesInternas,
                    request.Anexos
                );
            }

            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(id);
        }
    }
}