using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class CriterioAvaliacaoConfig : EntityTypeConfiguration<CriterioAvaliacao>
    {
        public CriterioAvaliacaoConfig()
        {
            HasKey(x => x.IdCriterioAvaliacao);
            Property(x => x.DescCriterioAvaliacao).IsRequired().HasMaxLength(100);
            Property(x => x.StrTextoExplicativo).IsRequired().HasMaxLength(1000);
            Property(x => x.Inativo).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
