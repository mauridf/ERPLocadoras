using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.Entities
{
    public class Usuario : EntityBase
    {
        public string Email { get; private set; }
        public string SenhaHash { get; private set; }
        public UsuarioTipo Tipo { get; private set; }
        public string? Permissoes { get; private set; } // JSON com módulos habilitados
        public DateTime? DataExpiracao { get; private set; }
        public bool Ativo { get; private set; }

        // Foreign Key para Locadora (pode ser null para usuários Global)
        public Guid? LocadoraId { get; private set; }

        // Navigation Properties
        public virtual Locadora? Locadora { get; private set; }
        public virtual Pessoa? Pessoa { get; private set; }

        // Constructor
        public Usuario(
            string email,
            string senhaHash,
            UsuarioTipo tipo,
            bool ativo = true,
            Guid? locadoraId = null)
        {
            Email = email;
            SenhaHash = senhaHash;
            Tipo = tipo;
            Ativo = ativo;
            LocadoraId = locadoraId;
        }

        // Methods
        public void AtualizarEmail(string email)
        {
            Email = email;
            AtualizarDataModificacao();
        }

        public void AtualizarSenha(string senhaHash)
        {
            SenhaHash = senhaHash;
            AtualizarDataModificacao();
        }

        public void AtualizarPermissoes(string? permissoes)
        {
            Permissoes = permissoes;
            AtualizarDataModificacao();
        }

        public void DefinirDataExpiracao(DateTime? dataExpiracao)
        {
            DataExpiracao = dataExpiracao;
            AtualizarDataModificacao();
        }

        public void AlterarStatus(bool ativo)
        {
            Ativo = ativo;
            AtualizarDataModificacao();
        }

        public void VincularLocadora(Guid? locadoraId)
        {
            LocadoraId = locadoraId;
            AtualizarDataModificacao();
        }

        // Business Rules
        public bool EstaExpirado()
        {
            return DataExpiracao.HasValue && DataExpiracao.Value < DateTime.UtcNow;
        }

        public bool PodeAcessarLocadora(Guid locadoraId)
        {
            return Tipo == UsuarioTipo.Global ||
                   (LocadoraId.HasValue && LocadoraId.Value == locadoraId);
        }
    }
}