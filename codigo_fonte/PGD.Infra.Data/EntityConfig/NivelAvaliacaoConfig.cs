using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class NivelAvaliacaoConfig : EntityTypeConfiguration<NivelAvaliacao>
    {
        public NivelAvaliacaoConfig()
        {
            HasKey(x => x.IdNivelAvaliacao);
            Property(x => x.DescNivelAvaliacao).IsRequired().HasMaxLength(100);
            
            Ignore(c => c.ValidationResult);
        }
    }
}
