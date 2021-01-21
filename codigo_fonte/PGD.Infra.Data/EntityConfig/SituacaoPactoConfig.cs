using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class SituacaoPactoConfig : EntityTypeConfiguration<SituacaoPacto>
    {
        public SituacaoPactoConfig()
        {
            HasKey(x => x.IdSituacaoPacto);
            Property(x => x.DescSituacaoPacto).IsRequired();
            
            Ignore(c => c.ValidationResult);
        }
    }
}
