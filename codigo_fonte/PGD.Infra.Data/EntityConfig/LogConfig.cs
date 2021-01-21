using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class LogConfig : EntityTypeConfiguration<Log>
    {
        public LogConfig()
        {
            HasKey(x => x.IdLog);

            Property(x => x.CpfUsuario).IsRequired();
            Property(x => x.Data).IsRequired();
            Property(x => x.Operacao).IsRequired().HasMaxLength(20);
            Property(x => x.Tabela).IsRequired().HasMaxLength(50);
            Property(x => x.IdTabela).IsRequired();
            Property(x => x.Valores).IsRequired().HasMaxLength(int.MaxValue).HasColumnType("varchar(max)");

            Ignore(c => c.ValidationResult);
        }
    }
}
