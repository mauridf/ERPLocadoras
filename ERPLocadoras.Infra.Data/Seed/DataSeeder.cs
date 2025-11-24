using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Core.Interfaces;

namespace ERPLocadoras.Infra.Data.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaHasher _senhaHasher;

        public DataSeeder(ApplicationDbContext context, ISenhaHasher senhaHasher)
        {
            _context = context;
            _senhaHasher = senhaHasher;
        }

        public async Task SeedAsync()
        {
            await SeedUsuariosGlobais();
            await SeedLocadorasExemplo();
            await _context.SaveChangesAsync();
        }

        private async Task SeedLocadorasExemplo()
        {
            if (!_context.Locadoras.Any())
            {
                // Locadora 1 - Especializada em carros
                var locadora1 = new Locadora(
                    "Locadora de Veículos Speed Ltda",
                    "Speed Locadora",
                    "12345678000195",
                    Core.Enums.StatusLocadora.Ativa
                );

                locadora1.AtualizarDadosGerais(
                    inscricaoEstadual: "123.456.789.000",
                    inscricaoMunicipal: "987654",
                    cnaePrincipal: "7711-0/00",
                    regimeTributario: "Simples Nacional",
                    dataFundacao: new DateTime(2015, 3, 15),
                    logotipoUrl: null
                );

                locadora1.AtualizarEndereco(
                    cep: "01234000",
                    logradouro: "Av. Paulista",
                    numero: "1000",
                    complemento: "Sala 501",
                    bairro: "Bela Vista",
                    cidade: "São Paulo",
                    uf: "SP",
                    pais: "Brasil",
                    tipoEndereco: "Matriz"
                );

                locadora1.AtualizarContato(
                    telefoneFixo: "1133334444",
                    celularWhatsApp: "11988887777",
                    emailComercial: "contato@speedlocadora.com.br",
                    site: "www.speedlocadora.com.br",
                    responsavelContato: "Carlos Silva",
                    cargoResponsavel: "Gerente Geral"
                );

                locadora1.AtualizarConfiguracoesOperacionais(
                    modalidadeLocacao: "Carros",
                    padraoCobranca: "Diária",
                    politicaCaucao: "20% do valor total",
                    aceitaTerceiros: true,
                    integracaoSeguradora: true,
                    seguradora: "Porto Seguro",
                    tipoFrota: "Própria"
                );

                // Locadora 2 - Especializada em motos
                var locadora2 = new Locadora(
                    "Moto Loc Express Eireli",
                    "Moto Express",
                    "98765432000110",
                    Core.Enums.StatusLocadora.Ativa
                );

                locadora2.AtualizarDadosGerais(
                    inscricaoEstadual: "987.654.321.000",
                    inscricaoMunicipal: "123456",
                    cnaePrincipal: "7711-0/00",
                    regimeTributario: "Lucro Presumido",
                    dataFundacao: new DateTime(2018, 7, 22),
                    logotipoUrl: null
                );

                locadora2.AtualizarEndereco(
                    cep: "04538000",
                    logradouro: "Rua Funchal",
                    numero: "418",
                    complemento: "Conjunto 102",
                    bairro: "Vila Olímpia",
                    cidade: "São Paulo",
                    uf: "SP",
                    pais: "Brasil",
                    tipoEndereco: "Matriz"
                );

                locadora2.AtualizarContato(
                    telefoneFixo: "1144445555",
                    celularWhatsApp: "11977776666",
                    emailComercial: "vendas@motoexpress.com.br",
                    site: "www.motoexpress.com.br",
                    responsavelContato: "Ana Oliveira",
                    cargoResponsavel: "Diretora Comercial"
                );

                locadora2.AtualizarConfiguracoesOperacionais(
                    modalidadeLocacao: "Motos",
                    padraoCobranca: "Diária + Km Rodado",
                    politicaCaucao: "Cartão de crédito ou depósito",
                    aceitaTerceiros: false,
                    integracaoSeguradora: true,
                    seguradora: "Allianz",
                    tipoFrota: "Mista"
                );

                await _context.Locadoras.AddRangeAsync(locadora1, locadora2);
            }
        }

        private async Task SeedUsuariosGlobais()
        {
            // Verificar se já existe algum usuário global
            if (!_context.Usuarios.Any(u => u.Tipo == UsuarioTipo.Global))
            {
                var senhaHash = _senhaHasher.HashSenha("Admin123!");

                var usuarioGlobal = new Usuario(
                    email: "admin@erplocadoras.com",
                    senhaHash: senhaHash,
                    tipo: UsuarioTipo.Global,
                    ativo: true
                );

                await _context.Usuarios.AddAsync(usuarioGlobal);

                // Criar dados pessoais para o admin global
                var pessoaAdmin = new Pessoa("Administrador Global", usuarioGlobal.Id);
                pessoaAdmin.AtualizarDadosPessoais(
                    nomeSocial: null,
                    sexo: "Masculino",
                    telefone: "+5511999999999",
                    dataNascimento: new DateTime(1980, 1, 1),
                    fotoUrl: null
                );

                await _context.Pessoas.AddAsync(pessoaAdmin);
            }
        }
    }
}