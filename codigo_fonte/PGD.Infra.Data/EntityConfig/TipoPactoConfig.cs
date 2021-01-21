using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class TipoPactoConfig : EntityTypeConfiguration<TipoPacto>
    {
        public TipoPactoConfig()
        {
            HasKey(x => x.IdTipoPacto);
            Property(x => x.DescTipoPacto).IsRequired().HasMaxLength(100);
            
            Ignore(c => c.ValidationResult);
        }
    }
}
