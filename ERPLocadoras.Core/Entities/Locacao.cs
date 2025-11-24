namespace ERPLocadoras.Core.Entities
{
    public class Locacao : EntityBase
    {
        // Foreign Keys
        public Guid LocadoraId { get; private set; }
        public Guid VeiculoId { get; private set; }
        public Guid ClienteId { get; private set; }

        // Navigation Properties
        public virtual Locadora Locadora { get; private set; }
        public virtual Veiculo Veiculo { get; private set; }
        public virtual Cliente Cliente { get; private set; }

        // Constructor mínimo para não quebrar as relações
        public Locacao(Guid locadoraId, Guid veiculoId, Guid clienteId)
        {
            LocadoraId = locadoraId;
            VeiculoId = veiculoId;
            ClienteId = clienteId;
        }
    }
}