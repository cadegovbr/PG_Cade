using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class UsuarioPerfilUnidadeConfig : EntityTypeConfiguration<UsuarioPerfilUnidade>
    {
        public UsuarioPerfilUnidadeConfig()
        {
            ToTable("UsuarioPerfilUnidade");

            HasKey(x =>  x.Id);
            
            Property(x => x.IdUsuario).IsRequired();
            Property(x => x.IdPerfil).IsRequired();
            Property(x => x.IdUnidade).IsRequired();
            
            // Relacionamentos
            HasRequired(x => x.Usuario)
                .WithMany(x => x.UsuariosPerfisUnidades)
                .HasForeignKey(x => x.IdUsuario);
            
            HasRequired(x => x.Perfil)
                .WithMany(x => x.UsuariosPerfisUnidades)
                .HasForeignKey(x => x.IdPerfil);
            
            HasRequired(x => x.Unidade)
                .WithMany(x => x.UsuariosPerfisUnidades)
                .HasForeignKey(x => x.IdUnidade);

            Ignore(c => c.ValidationResult);
        }
    }
}
