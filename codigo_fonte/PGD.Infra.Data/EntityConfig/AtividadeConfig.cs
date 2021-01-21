using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class AtividadeConfig : EntityTypeConfiguration<Atividade>
    {
        public AtividadeConfig()
        {
            HasKey(x => x.IdAtividade);

            Property(x => x.NomAtividade).IsRequired().HasMaxLength(1000);
            Property(x => x.PctMinimoReducao).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
