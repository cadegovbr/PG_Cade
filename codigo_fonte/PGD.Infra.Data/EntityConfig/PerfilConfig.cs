using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class PerfilConfig : EntityTypeConfiguration<Perfil>
    {
        public PerfilConfig()
        {
            ToTable("Perfil");

            HasKey(x =>  x.Id);
            
            Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);
        }
    }
}
