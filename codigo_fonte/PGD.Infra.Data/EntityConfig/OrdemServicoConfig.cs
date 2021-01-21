using PGD.Domain.Entities;
using PGD.Infra.Data.Repository;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class OrdemServicoConfig : EntityTypeConfiguration<OrdemServico>
    {
        public OrdemServicoConfig()
        {
            HasKey(x => x.IdOrdemServico);
            Property(x => x.DatInicioNormativo).IsRequired();
            Property(x => x.DatInicioSistema).IsRequired();
            Property(x => x.DescOrdemServico).IsRequired().HasMaxLength(1000);
            Ignore(c => c.ValidationResult);
        }
    }
}
