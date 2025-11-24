namespace ERPLocadoras.Core.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }

        protected EntityBase()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
        }

        public void AtualizarDataModificacao()
        {
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}