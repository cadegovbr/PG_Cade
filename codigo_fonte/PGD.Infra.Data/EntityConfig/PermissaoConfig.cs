using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class PermissaoConfig : EntityTypeConfiguration<Permissao>
    {
        public PermissaoConfig()
        {
            ToTable("Permissao");

            HasKey(x => x.IdPermissao);
            
            Property(x => x.Descricao)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(x => x.Controller)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(70);

            Property(x => x.Action)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);
        }
    }
}
