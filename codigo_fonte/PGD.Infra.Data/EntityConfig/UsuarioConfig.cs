using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using PGD.Domain.Entities.Usuario;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            ToTable("Usuario");

            HasKey(x => x.IdUsuario);
            
            Property(x => x.Matricula).IsRequired();
            
            Property(x => x.Cpf)
                .HasColumnName("Cpf")
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(11)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("IDX_CPF") {IsUnique = false}));
            
            Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(250);

            Property(x => x.DataNascimento)
                .IsRequired();
            
            Property(x => x.Email)
                .IsRequired();
            
            Property(x => x.Inativo).IsRequired();
            

            // Não mapeados para o banco
            Ignore(x => x.Unidade);
            Ignore(x => x.Administrador);
            Ignore(x => x.NomeUnidade);
        }
    }
}
