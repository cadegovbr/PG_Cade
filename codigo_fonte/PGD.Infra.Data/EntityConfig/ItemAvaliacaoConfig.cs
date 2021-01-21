using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class ItemAvaliacaoConfig : EntityTypeConfiguration<ItemAvaliacao>
    {
        public ItemAvaliacaoConfig()
        {
            HasKey(x => x.IdItemAvaliacao);
            Property(x => x.DescItemAvaliacao).IsRequired().HasMaxLength(500);
            Property(x => x.ImpactoNota).IsRequired();
            Property(x => x.IdNotaMaxima).IsRequired();
            Property(x => x.IdCriterioAvaliacao).IsRequired();
            Property(x => x.Inativo).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
