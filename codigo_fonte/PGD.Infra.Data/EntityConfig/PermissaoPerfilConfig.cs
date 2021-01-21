using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class PermissaoPerfilConfig : EntityTypeConfiguration<PermissaoPerfil>
    {
        public PermissaoPerfilConfig()
        {
            ToTable("PermissaoPerfil");

            HasKey(x => new { x.IdPermissao, x.IdPerfil});
            
            // Relacionamentos
            //HasRequired(x => x.Perfil)
            //    .WithMany(x => x.PermissoesPerfil)
            //    .HasForeignKey(x => x.IdPerfil);

            HasRequired(x => x.Permissao)
                .WithMany(x => x.PermissoesPerfil)
                .HasForeignKey(x => x.IdPermissao);

        }
    }
}
