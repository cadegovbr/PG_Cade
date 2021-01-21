using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class OS_CriterioAvaliacaoConfig : EntityTypeConfiguration<OS_CriterioAvaliacao>
    {

        public OS_CriterioAvaliacaoConfig()
        {
            HasKey(x => x.IdCriterioAvaliacao);
            Property(x => x.DescCriterioAvaliacao).IsRequired().HasMaxLength(100);
            Property(x => x.StrTextoExplicativo).IsRequired().HasMaxLength(1000);

            Ignore(c => c.ValidationResult);
        }
    }
}
