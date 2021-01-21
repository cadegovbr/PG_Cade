using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class OsGrupoAtividadeConfig : EntityTypeConfiguration<OS_GrupoAtividade>
    {

        public OsGrupoAtividadeConfig()
        {
            ToTable("OS_GrupoAtividade");
            HasKey(x => x.IdGrupoAtividade);
            Property(x => x.NomGrupoAtividade).IsRequired().HasMaxLength(500);

            Ignore(c => c.ValidationResult);
            
            // Relacionamentos
            HasRequired(x => x.GrupoAtividade)
                .WithMany(x => x.ListaOsGrupoAtividades)
                .HasForeignKey(x => x.IdGrupoAtividadeOriginal);
        }
    }
}
