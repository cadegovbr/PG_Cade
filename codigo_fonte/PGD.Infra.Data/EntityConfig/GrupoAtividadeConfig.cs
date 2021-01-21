using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class GrupoAtividadeConfig : EntityTypeConfiguration<GrupoAtividade>
    {

        public GrupoAtividadeConfig()
        {
            HasKey(x => x.IdGrupoAtividade);
            Property(x => x.NomGrupoAtividade).IsRequired().HasMaxLength(500);

            Ignore(c => c.ValidationResult);
        }
    }
}
