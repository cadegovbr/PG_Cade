using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class CronogramaConfig : EntityTypeConfiguration<Cronograma>
    {
        public CronogramaConfig()
        {
            HasKey(x => x.IdCronograma);
            Property(x => x.HorasCronograma).IsRequired();
            Property(x => x.DataCronograma).IsRequired();
            Property(x => x.IdPacto).IsRequired();
            Property(x => x.Feriado).IsRequired();
            Property(x => x.DuracaoFeriado).IsOptional();
            Property(x => x.Suspenso).IsRequired();
            Ignore(c => c.ValidationResult);
        }
    }
}
