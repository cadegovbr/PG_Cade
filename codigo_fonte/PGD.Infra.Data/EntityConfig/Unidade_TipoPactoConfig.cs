using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class Unidade_TipoPactoConfig : EntityTypeConfiguration<Unidade_TipoPacto>
    {
        public Unidade_TipoPactoConfig()
        {
            HasKey(x => x.IdUnidade_TipoPacto);
            Property(x => x.IdUnidade).IsRequired();
            Property(x => x.IdTipoPacto).IsRequired();
            Property(x => x.IndPermitePactoExterior).IsRequired();
            
            Ignore(c => c.ValidationResult);

            // Relacionamentos
            HasRequired(x => x.Unidade)
                .WithMany(x => x.UnidadesTiposPactos)
                .HasForeignKey(x => x.IdUnidade);

            HasRequired(x => x.TipoPacto)
                .WithMany(x => x.UnidadesTiposPactos)
                .HasForeignKey(x => x.IdTipoPacto);
        }
    }
}
