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
            await SeedVeiculosExemplo();
            await SeedLocacoesExemplo();
            await _context.SaveChangesAsync();
        }

        private async Task SeedLocacoesExemplo()
        {
            if (!_context.Locacoes.Any())
            {
                var locadoraSpeed = await _context.Locadoras.FirstAsync(l => l.NomeFantasia.Contains("Speed"));
                var locadoraMoto = await _context.Locadoras.FirstAsync(l => l.NomeFantasia.Contains("Moto Express"));

                var clienteJoao = await _context.Clientes.FirstAsync(c => c.NomeCompleto.Contains("João"));
                var clienteMaria = await _context.Clientes.FirstAsync(c => c.NomeCompleto.Contains("Maria"));
                var clientePedro = await _context.Clientes.FirstAsync(c => c.NomeCompleto.Contains("Pedro"));

                var veiculoCivic = await _context.Veiculos.FirstAsync(v => v.Placa == "ABC1D23");
                var veiculoCorolla = await _context.Veiculos.FirstAsync(v => v.Placa == "DEF2G34");
                var veiculoCG160 = await _context.Veiculos.FirstAsync(v => v.Placa == "MNO5P67");

                // Locação 1 - Finalizada
                var locacao1 = new Locacao(
                    new DateTime(2024, 1, 10),
                    new DateTime(2024, 1, 15),
                    TipoLocacao.Diaria,
                    150.00m,
                    FormaCobranca.CartaoCredito,
                    FormaCaucao.CartaoCredito,
                    750.00m,
                    15000,
                    locadoraSpeed.Id,
                    veiculoCivic.Id,
                    clienteJoao.Id
                );

                locacao1.AtualizarPlanoLocacao("Plano Básico", 1.50m, 100);
                locacao1.AtualizarCaucao(500.00m, FormaCaucao.CartaoCredito);
                locacao1.AtualizarChecklistEntrega(
                    "{\"pneus\":\"bom\", \"lataria\":\"boa\", \"documentos\":\"ok\"}",
                    "3/4",
                    "Carlos Silva"
                );
                locacao1.IniciarLocacao();
                locacao1.AtualizarChecklistDevolucao(
                    "{\"pneus\":\"bom\", \"lataria\":\"boa\", \"documentos\":\"ok\"}",
                    "1/2",
                    "Carlos Silva"
                );
                locacao1.FinalizarLocacao(
                    new DateTime(2024, 1, 15),
                    15200,
                    750.00m,
                    0
                );

                // Locação 2 - Ativa
                var locacao2 = new Locacao(
                    DateTime.UtcNow.AddDays(-2),
                    DateTime.UtcNow.AddDays(3),
                    TipoLocacao.Diaria,
                    180.00m,
                    FormaCobranca.Pix,
                    FormaCaucao.Transferencia,
                    900.00m,
                    8000,
                    locadoraSpeed.Id,
                    veiculoCorolla.Id,
                    clienteMaria.Id
                );

                locacao2.AtualizarPlanoLocacao("Plano Premium", 2.00m, 200);
                locacao2.AtualizarCaucao(600.00m, FormaCaucao.Transferencia);
                locacao2.AtualizarChecklistEntrega(
                    "{\"pneus\":\"excelente\", \"lataria\":\"excelente\", \"documentos\":\"ok\"}",
                    "cheio",
                    "Ana Oliveira"
                );
                locacao2.IniciarLocacao();

                // Locação 3 - Reservada
                var locacao3 = new Locacao(
                    DateTime.UtcNow.AddDays(5),
                    DateTime.UtcNow.AddDays(10),
                    TipoLocacao.Semanal,
                    80.00m,
                    FormaCobranca.CartaoCredito,
                    FormaCaucao.CartaoCredito,
                    400.00m,
                    5000,
                    locadoraMoto.Id,
                    veiculoCG160.Id,
                    clientePedro.Id
                );

                locacao3.AtualizarPlanoLocacao("Plano Moto", 0.50m, 50);
                locacao3.AtualizarCaucao(200.00m, FormaCaucao.CartaoCredito);
                locacao3.AtualizarObservacoes("Cliente preferiu retirar na sexta-feira", null);

                await _context.Locacoes.AddRangeAsync(locacao1, locacao2, locacao3);
            }
        }

        private async Task SeedVeiculosExemplo()
        {
            if (!_context.Veiculos.Any())
            {
                var locadoras = await _context.Locadoras.ToListAsync();

                foreach (var locadora in locadoras)
                {
                    if (locadora.NomeFantasia.Contains("Speed"))
                    {
                        // Veículos para locadora Speed (carros)
                        await SeedVeiculosSpeed(locadora.Id);
                    }
                    else if (locadora.NomeFantasia.Contains("Moto Express"))
                    {
                        // Veículos para locadora Moto Express (motos)
                        await SeedVeiculosMotoExpress(locadora.Id);
                    }
                }
            }
        }

        private async Task SeedVeiculosMotoExpress(Guid locadoraId)
        {
            // Moto 1 - Honda CG 160
            var cg160 = new Veiculo(
                TipoVeiculo.Moto,
                "Honda",
                "CG 160",
                2023,
                2024,
                "MNO5P67",
                "45678901234",
                "ML3PC26X6LJ123456",
                "Vermelha",
                CategoriaVeiculo.Utilitario,
                Combustivel.Gasolina,
                5000,
                new DateTime(2023, 4, 5),
                15000,
                locadoraId
            );

            cg160.AtualizarDadosBasicos(
                "160cc",
                "2 passageiros",
                "Moto econômica e confiável"
            );

            cg160.AtualizarValorMercado(14000);
            cg160.AtualizarDadosSeguro(
                "5678901234",
                "Allianz",
                new DateTime(2024, 12, 31)
            );

            // Moto 2 - Yamaha Factor 150
            var factor150 = new Veiculo(
                TipoVeiculo.Moto,
                "Yamaha",
                "Factor 150",
                2023,
                2024,
                "PQR6S78",
                "56789012345",
                "ML3PC26X6LJ123457",
                "Azul",
                CategoriaVeiculo.Utilitario,
                Combustivel.Gasolina,
                3000,
                new DateTime(2023, 5, 15),
                14000,
                locadoraId
            );

            factor150.AtualizarDadosBasicos(
                "150cc",
                "2 passageiros",
                "Nova, com garantia"
            );

            factor150.AtualizarValorMercado(13500);
            factor150.AtualizarDadosSeguro(
                "6789012345",
                "Allianz",
                new DateTime(2024, 12, 31)
            );

            await _context.Veiculos.AddRangeAsync(cg160, factor150);
        }

        private async Task SeedVeiculosSpeed(Guid locadoraId)
        {
            // Carro 1 - Honda Civic
            var civic = new Veiculo(
                TipoVeiculo.Carro,
                "Honda",
                "Civic",
                2023,
                2024,
                "ABC1D23",
                "12345678901",
                "9BWZZZ377VT004251",
                "Prata",
                CategoriaVeiculo.Sedan,
                Combustivel.Flex,
                15000,
                new DateTime(2023, 1, 15),
                120000,
                locadoraId
            );

            civic.AtualizarDadosBasicos(
                "2.0 Flexone",
                "5 passageiros",
                "Veículo em excelente estado, único dono"
            );

            civic.AtualizarValorMercado(115000);
            civic.AtualizarDadosSeguro(
                "2345678901",
                "Porto Seguro",
                new DateTime(2024, 12, 31)
            );
            civic.AtualizarManutencao(
                new DateTime(2023, 12, 1),
                new DateTime(2024, 6, 1)
            );

            // Carro 2 - Toyota Corolla
            var corolla = new Veiculo(
                TipoVeiculo.Carro,
                "Toyota",
                "Corolla",
                2023,
                2024,
                "DEF2G34",
                "23456789012",
                "9BWZZZ377VT004252",
                "Preto",
                CategoriaVeiculo.Sedan,
                Combustivel.Flex,
                8000,
                new DateTime(2023, 3, 20),
                130000,
                locadoraId
            );

            corolla.AtualizarDadosBasicos(
                "2.0 Flex",
                "5 passageiros",
                "Veículo com pouquíssimo uso"
            );

            corolla.AtualizarValorMercado(125000);
            corolla.AtualizarDadosSeguro(
                "3456789012",
                "Porto Seguro",
                new DateTime(2024, 12, 31)
            );

            // Carro 3 - Fiat Strada
            var strada = new Veiculo(
                TipoVeiculo.Carro,
                "Fiat",
                "Strada",
                2023,
                2024,
                "GHI3J45",
                "34567890123",
                "9BWZZZ377VT004253",
                "Branco",
                CategoriaVeiculo.Pickup,
                Combustivel.Flex,
                25000,
                new DateTime(2023, 2, 10),
                80000,
                locadoraId
            );

            strada.AtualizarDadosBasicos(
                "1.4 Firefly",
                "2 passageiros + caçamba",
                "Ideal para trabalho"
            );

            strada.AtualizarValorMercado(75000);
            strada.AtualizarDadosSeguro(
                "4567890123",
                "Porto Seguro",
                new DateTime(2024, 12, 31)
            );
            strada.AlterarStatus(StatusVeiculo.Manutencao); // Em manutenção

            await _context.Veiculos.AddRangeAsync(civic, corolla, strada);
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