using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class OS_AtividadeConfig : EntityTypeConfiguration<OS_Atividade>
    {
        public OS_AtividadeConfig()
        {
            HasKey(x => x.IdAtividade);

            Property(x => x.NomAtividade).IsRequired().HasMaxLength(1000);
            Property(x => x.PctMinimoReducao).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
