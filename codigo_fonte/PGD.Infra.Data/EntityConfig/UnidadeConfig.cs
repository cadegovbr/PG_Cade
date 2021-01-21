using PGD.Domain.Entities.RH;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class UnidadeConfig : EntityTypeConfiguration<Unidade>
    {       
        public UnidadeConfig()
        {
            HasKey(x => x.IdUnidade);
            Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(200);

            Property(x => x.Sigla).IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(25);

            Property(x => x.Codigo)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(x => x.Excluido).IsRequired();

            // Relacionamentos
            HasOptional(x => x.UnidadeSuperior)
                .WithMany(x => x.Unidades)
                .HasForeignKey(x => x.IdUnidadeSuperior);
        }
    }
}
