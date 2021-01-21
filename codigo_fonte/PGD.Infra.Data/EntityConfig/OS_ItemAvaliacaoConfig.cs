using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class OS_ItemAvaliacaoConfig : EntityTypeConfiguration<OS_ItemAvaliacao>
    {
        public OS_ItemAvaliacaoConfig()
        {
            HasKey(x => x.IdItemAvaliacao);

            Property(x => x.DescItemAvaliacao).IsRequired().HasMaxLength(100);
            Property(x => x.ImpactoNota).IsRequired();
            Property(x => x.IdNotaMaxima).IsRequired();
            Property(x => x.IdCriterioAvaliacao).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
