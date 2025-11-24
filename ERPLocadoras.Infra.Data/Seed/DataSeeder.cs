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
    }
}