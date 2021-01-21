using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class JustificativaConfig : EntityTypeConfiguration<Justificativa>
    {
        public JustificativaConfig()
        {
            HasKey(x => x.IdJustificativa);
            Property(x => x.DescJustificativa).IsRequired();
            
            Ignore(c => c.ValidationResult);
        }
    }
}
