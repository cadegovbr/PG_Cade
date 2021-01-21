using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class NotaAvaliacaoConfig : EntityTypeConfiguration<NotaAvaliacao>
    {
        public NotaAvaliacaoConfig()
        {
            HasKey(x => x.IdNotaAvaliacao);
            Property(x => x.DescNotaAvaliacao).IsRequired().HasMaxLength(20);
            Property(x => x.IndAtivoAvaliacaoDetalhada).IsRequired();
            Property(x => x.IndAtivoAvaliacaoSimplificada).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
