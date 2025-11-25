using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class ManutencaoService : IManutencaoService
    {
        private readonly ApplicationDbContext _context;

        public ManutencaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ManutencaoResponse?> ObterPorIdAsync(Guid id)
        {
            var manutencao = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .Include(m => m.Locadora)
                .Include(m => m.ResponsavelManutencao)
                .ThenInclude(r => r.Pessoa)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null)
                return null;

            return await MapToResponse(manutencao);
        }

        public async Task<IEnumerable<ManutencaoResponse>> ObterTodasAsync()
        {
            var manutencoes = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .Include(m => m.Locadora)
                .Include(m => m.ResponsavelManutencao)
                .ThenInclude(r => r.Pessoa)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var responses = new List<ManutencaoResponse>();
            foreach (var manutencao in manutencoes)
            {
                responses.Add(await MapToResponse(manutencao));
            }

            return responses;
        }

        public async Task<IEnumerable<ManutencaoResponse>> ObterPorLocadoraAsync(Guid locadoraId)
        {
            var manutencoes = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .Include(m => m.Locadora)
                .Include(m => m.ResponsavelManutencao)
                .ThenInclude(r => r.Pessoa)
                .Where(m => m.LocadoraId == locadoraId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var responses = new List<ManutencaoResponse>();
            foreach (var manutencao in manutencoes)
            {
                responses.Add(await MapToResponse(manutencao));
            }

            return responses;
        }

        public async Task<IEnumerable<ManutencaoResponse>> ObterPorVeiculoAsync(Guid veiculoId)
        {
            var manutencoes = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .Include(m => m.Locadora)
                .Include(m => m.ResponsavelManutencao)
                .ThenInclude(r => r.Pessoa)
                .Where(m => m.VeiculoId == veiculoId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var responses = new List<ManutencaoResponse>();
            foreach (var manutencao in manutencoes)
            {
                responses.Add(await MapToResponse(manutencao));
            }

            return responses;
        }

        public async Task<IEnumerable<ManutencaoResponse>> ObterEmAndamentoPorLocadoraAsync(Guid locadoraId)
        {
            var manutencoes = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .Include(m => m.Locadora)
                .Include(m => m.ResponsavelManutencao)
                .ThenInclude(r => r.Pessoa)
                .Where(m => m.LocadoraId == locadoraId && m.EstaEmAndamento())
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var responses = new List<ManutencaoResponse>();
            foreach (var manutencao in manutencoes)
            {
                responses.Add(await MapToResponse(manutencao));
            }

            return responses;
        }

        public async Task<ManutencaoResponse> CriarAsync(CriarManutencaoRequest request)
        {
            // Validar se veículo existe
            var veiculo = await _context.Veiculos.FindAsync(request.VeiculoId);
            if (veiculo == null)
                throw new InvalidOperationException("Veículo não encontrado.");

            // Validar se locadora existe
            var locadoraExiste = await _context.Locadoras.AnyAsync(l => l.Id == request.LocadoraId);
            if (!locadoraExiste)
                throw new InvalidOperationException("Locadora não encontrada.");

            // Validar responsável se fornecido
            if (request.ResponsavelManutencaoId.HasValue)
            {
                var responsavelExiste = await _context.Usuarios.AnyAsync(u =>
                    u.Id == request.ResponsavelManutencaoId.Value &&
                    u.Tipo == UsuarioTipo.Mecanico);
                if (!responsavelExiste)
                    throw new InvalidOperationException("Responsável pela manutenção não encontrado ou não é um mecânico.");
            }

            var manutencao = new Manutencao(
                request.Tipo,
                request.Descricao,
                request.DataEntrada,
                request.KmEntrada,
                request.VeiculoId,
                request.LocadoraId
            );

            // Atualizar datas
            if (request.DataSaidaPrevista.HasValue)
            {
                manutencao.AtualizarDatas(
                    request.DataSaidaPrevista,
                    null,
                    null
                );
            }

            // Atualizar oficina
            if (!string.IsNullOrEmpty(request.OficinaPrestador))
            {
                manutencao.AtualizarOficina(
                    request.OficinaPrestador,
                    request.CnpjContatoOficina
                );
            }

            // Atualizar responsável
            if (request.ResponsavelManutencaoId.HasValue)
            {
                manutencao.AtualizarResponsavel(request.ResponsavelManutencaoId.Value);
            }

            // Atualizar custos
            if (request.CustoPecas.HasValue || request.CustoMaoDeObra.HasValue)
            {
                var custoTotal = (request.CustoPecas ?? 0) + (request.CustoMaoDeObra ?? 0);
                manutencao.AtualizarCustos(
                    request.CustoPecas,
                    request.CustoMaoDeObra,
                    custoTotal
                );
            }

            // Atualizar garantia e próxima revisão
            if (!string.IsNullOrEmpty(request.GarantiaServico) || request.DataProximaRevisaoSugerida.HasValue)
            {
                manutencao.AtualizarGarantiaProximaRevisao(
                    request.GarantiaServico,
                    request.DataProximaRevisaoSugerida
                );
            }

            // Atualizar observações
            if (!string.IsNullOrEmpty(request.Observacoes))
            {
                manutencao.AtualizarObservacoesAnexos(request.Observacoes, null);
            }

            _context.Manutencoes.Add(manutencao);
            await _context.SaveChangesAsync();

            // Atualizar veículo com data da última revisão se for preventiva
            if (request.Tipo == TipoManutencao.Preventiva)
            {
                veiculo.AtualizarManutencao(request.DataEntrada, request.DataProximaRevisaoSugerida);
                await _context.SaveChangesAsync();
            }

            return await ObterPorIdAsync(manutencao.Id) ?? throw new Exception("Erro ao criar manutenção.");
        }

        public async Task<ManutencaoResponse?> AtualizarAsync(Guid id, AtualizarManutencaoRequest request)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
                return null;

            // Atualizar descrição - USAR O NOVO MÉTODO
            if (!string.IsNullOrEmpty(request.Descricao))
                manutencao.AtualizarDescricao(request.Descricao);

            // Atualizar datas
            if (request.DataSaidaPrevista.HasValue || request.DataSaidaReal.HasValue || request.KmSaida.HasValue)
            {
                manutencao.AtualizarDatas(
                    request.DataSaidaPrevista,
                    request.DataSaidaReal,
                    request.KmSaida
                );
            }

            // Atualizar oficina
            if (request.OficinaPrestador != null || request.CnpjContatoOficina != null)
            {
                manutencao.AtualizarOficina(
                    request.OficinaPrestador,
                    request.CnpjContatoOficina
                );
            }

            // Atualizar responsável
            if (request.ResponsavelManutencaoId.HasValue)
            {
                // Validar responsável
                var responsavelExiste = await _context.Usuarios.AnyAsync(u =>
                    u.Id == request.ResponsavelManutencaoId.Value &&
                    u.Tipo == UsuarioTipo.Mecanico);
                if (!responsavelExiste)
                    throw new InvalidOperationException("Responsável pela manutenção não encontrado ou não é um mecânico.");

                manutencao.AtualizarResponsavel(request.ResponsavelManutencaoId.Value);
            }

            // Atualizar custos
            if (request.CustoPecas.HasValue || request.CustoMaoDeObra.HasValue || request.CustoTotal.HasValue)
            {
                manutencao.AtualizarCustos(
                    request.CustoPecas,
                    request.CustoMaoDeObra,
                    request.CustoTotal
                );
            }

            // Atualizar garantia e próxima revisão
            if (request.GarantiaServico != null || request.DataProximaRevisaoSugerida.HasValue)
            {
                manutencao.AtualizarGarantiaProximaRevisao(
                    request.GarantiaServico,
                    request.DataProximaRevisaoSugerida
                );
            }

            // Atualizar observações e anexos
            if (request.Observacoes != null || request.Anexos != null)
            {
                manutencao.AtualizarObservacoesAnexos(
                    request.Observacoes ?? manutencao.Observacoes,
                    request.Anexos
                );
            }

            // Atualizar status
            if (request.Status.HasValue)
                manutencao.AlterarStatus(request.Status.Value);

            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(id);
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
                return false;

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FinalizarManutencaoAsync(Guid id, FinalizarManutencaoRequest request)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
                return false;

            try
            {
                manutencao.FinalizarManutencao(
                    request.DataSaidaReal,
                    request.KmSaida,
                    request.CustoTotal
                );

                if (!string.IsNullOrEmpty(request.Observacoes))
                {
                    manutencao.AtualizarObservacoesAnexos(request.Observacoes, manutencao.Anexos);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> AlterarStatusAsync(Guid id, StatusManutencao novoStatus)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
                return false;

            manutencao.AlterarStatus(novoStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Manutencao?> ObterEntidadePorIdAsync(Guid id)
        {
            return await _context.Manutencoes.FindAsync(id);
        }

        private async Task<ManutencaoResponse> MapToResponse(Manutencao manutencao)
        {
            var response = new ManutencaoResponse
            {
                Id = manutencao.Id,
                Tipo = manutencao.Tipo,
                Descricao = manutencao.Descricao,
                DataEntrada = manutencao.DataEntrada,
                DataSaidaPrevista = manutencao.DataSaidaPrevista,
                DataSaidaReal = manutencao.DataSaidaReal,
                KmEntrada = manutencao.KmEntrada,
                KmSaida = manutencao.KmSaida,
                OficinaPrestador = manutencao.OficinaPrestador,
                CnpjContatoOficina = manutencao.CnpjContatoOficina,
                ResponsavelManutencaoId = manutencao.ResponsavelManutencaoId,
                ResponsavelManutencaoNome = manutencao.ResponsavelManutencao?.Pessoa?.NomeCompleto,
                CustoPecas = manutencao.CustoPecas,
                CustoMaoDeObra = manutencao.CustoMaoDeObra,
                CustoTotal = manutencao.CustoTotal,
                GarantiaServico = manutencao.GarantiaServico,
                DataProximaRevisaoSugerida = manutencao.DataProximaRevisaoSugerida,
                Observacoes = manutencao.Observacoes,
                Anexos = manutencao.Anexos,
                Status = manutencao.Status,
                VeiculoId = manutencao.VeiculoId,
                LocadoraId = manutencao.LocadoraId,
                VeiculoDescricao = $"{manutencao.Veiculo?.Marca} {manutencao.Veiculo?.Modelo} - {manutencao.Veiculo?.Placa}",
                LocadoraNome = manutencao.Locadora?.NomeFantasia,
                DataCriacao = manutencao.DataCriacao,
                DataAtualizacao = manutencao.DataAtualizacao
            };

            // Informações calculadas
            response.EstaEmAndamento = manutencao.EstaEmAndamento();
            response.EstaConcluida = manutencao.EstaConcluida();
            response.DiasManutencao = manutencao.CalcularDiasManutencao();
            response.KmPercorrido = manutencao.CalcularKmPercorrido();

            return response;
        }
    }
}