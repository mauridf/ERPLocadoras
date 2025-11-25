using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Core.Interfaces;

namespace ERPLocadoras.Infra.Data.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaHasher _senhaHasher;

        public DataSeeder(
            ApplicationDbContext context,
            ISenhaHasher senhaHasher)
        {
            _context = context;
            _senhaHasher = senhaHasher;
        }

        public async Task SeedAsync()
        {
            await SeedUsuariosGlobais();
            await SeedLocadorasExemplo();
            await SeedUsuariosLocadoras();
            await SeedClientesExemplo();
            await _context.SaveChangesAsync();
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

        private async Task SeedLocadorasExemplo()
        {
            if (!_context.Locadoras.Any())
            {
                // Locadora 1 - Especializada em carros
                var locadora1 = new Locadora(
                    "Locadora de Veículos Speed Ltda",
                    "Speed Locadora",
                    "12345678000195",
                    StatusLocadora.Ativa
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
                    StatusLocadora.Ativa
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

        private async Task SeedUsuariosLocadoras()
        {
            var locadoras = await _context.Locadoras.ToListAsync();

            foreach (var locadora in locadoras)
            {
                // Criar usuário Admin para cada locadora
                var emailAdmin = $"admin@{locadora.NomeFantasia.ToLower().Replace(" ", "")}.com.br";

                if (!await _context.Usuarios.AnyAsync(u => u.Email == emailAdmin))
                {
                    var senhaHash = _senhaHasher.HashSenha("Admin123!");
                    var usuarioAdmin = new Usuario(
                        email: emailAdmin,
                        senhaHash: senhaHash,
                        tipo: UsuarioTipo.Admin,
                        ativo: true,
                        locadoraId: locadora.Id
                    );

                    _context.Usuarios.Add(usuarioAdmin);

                    // Criar dados pessoais do admin
                    var pessoaAdmin = new Pessoa($"Administrador {locadora.NomeFantasia}", usuarioAdmin.Id);
                    pessoaAdmin.AtualizarDadosPessoais(
                        nomeSocial: null,
                        sexo: "Masculino",
                        telefone: "+5511999999999",
                        dataNascimento: new DateTime(1985, 1, 1),
                        fotoUrl: null
                    );

                    _context.Pessoas.Add(pessoaAdmin);

                    // Criar usuário Atendente para cada locadora
                    var emailAtendente = $"atendente@{locadora.NomeFantasia.ToLower().Replace(" ", "")}.com.br";

                    var usuarioAtendente = new Usuario(
                        email: emailAtendente,
                        senhaHash: senhaHash,
                        tipo: UsuarioTipo.Atendente,
                        ativo: true,
                        locadoraId: locadora.Id
                    );

                    _context.Usuarios.Add(usuarioAtendente);

                    // Criar dados pessoais do atendente
                    var pessoaAtendente = new Pessoa($"Atendente {locadora.NomeFantasia}", usuarioAtendente.Id);
                    pessoaAtendente.AtualizarDadosPessoais(
                        nomeSocial: null,
                        sexo: "Feminino",
                        telefone: "+5511888888888",
                        dataNascimento: new DateTime(1990, 5, 15),
                        fotoUrl: null
                    );

                    _context.Pessoas.Add(pessoaAtendente);
                }
            }
        }

        private async Task SeedClientesExemplo()
        {
            if (!_context.Clientes.Any())
            {
                // Cliente 1 - Com acesso ao sistema
                var cliente1 = new Cliente("João Carlos Silva");
                cliente1.AtualizarDadosPessoais(
                    nomeSocial: null,
                    sexo: "Masculino",
                    telefone: "11999991111",
                    fotoUrl: null
                );
                cliente1.AtualizarEndereco(
                    cep: "01311000",
                    logradouro: "Rua Augusta",
                    numero: "1500",
                    complemento: "Apto 101",
                    bairro: "Consolação",
                    cidade: "São Paulo",
                    uf: "SP",
                    pais: "Brasil"
                );

                // Criar usuário para o cliente
                var usuarioCliente1 = await CriarUsuarioClienteAsync(
                    "joao.silva@email.com",
                    "Cliente123!"
                );

                if (usuarioCliente1 != null)
                {
                    cliente1.VincularUsuario(usuarioCliente1.Id);
                }

                // Cliente 2 - Sem acesso ao sistema (cadastrado por atendente)
                var cliente2 = new Cliente("Maria Oliveira Santos");
                cliente2.AtualizarDadosPessoais(
                    nomeSocial: null,
                    sexo: "Feminino",
                    telefone: "11988882222",
                    fotoUrl: null
                );
                cliente2.AtualizarEndereco(
                    cep: "01415000",
                    logradouro: "Alameda Santos",
                    numero: "2100",
                    complemento: "Sala 305",
                    bairro: "Jardim Paulista",
                    cidade: "São Paulo",
                    uf: "SP",
                    pais: "Brasil"
                );

                // Cliente 3 - Com acesso ao sistema
                var cliente3 = new Cliente("Pedro Henrique Lima");
                cliente3.AtualizarDadosPessoais(
                    nomeSocial: null,
                    sexo: "Masculino",
                    telefone: "11977773333",
                    fotoUrl: null
                );

                // Criar usuário para o cliente 3
                var usuarioCliente3 = await CriarUsuarioClienteAsync(
                    "pedro.lima@email.com",
                    "Cliente123!"
                );

                if (usuarioCliente3 != null)
                {
                    cliente3.VincularUsuario(usuarioCliente3.Id);
                }

                await _context.Clientes.AddRangeAsync(cliente1, cliente2, cliente3);
            }
        }

        private async Task<Usuario?> CriarUsuarioClienteAsync(string email, string senha)
        {
            // Verificar se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == email))
                return null;

            var senhaHash = _senhaHasher.HashSenha(senha);
            var usuario = new Usuario(email, senhaHash, UsuarioTipo.Cliente, true);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync(); // Salvar para obter o ID

            return usuario;
        }
    }
}