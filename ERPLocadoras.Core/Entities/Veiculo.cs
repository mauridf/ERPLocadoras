namespace ERPLocadoras.Core.Entities
{
    public class Veiculo : EntityBase
    {
        // Foreign Key para Locadora
        public Guid LocadoraId { get; private set; }

        // Navigation Properties
        public virtual Locadora Locadora { get; private set; }

        // Constructor mínimo para não quebrar as relações
        public Veiculo(Guid locadoraId)
        {
            LocadoraId = locadoraId;
        }
    }
}