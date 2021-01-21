using PGD.Domain.Entities.RH;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class FeriadoConfig : EntityTypeConfiguration<Feriado>
    {
        public FeriadoConfig()
        {
            HasKey(x => x.ID);
            Property(x => x.data_feriado).IsOptional();
            Property(x => x.descricao).IsOptional();
            Property(x => x.id_localidade).IsOptional();
            Property(x => x.id_unidade_federativa).IsOptional();
            Property(x => x.categoria).IsOptional();
            Property(x => x.id_municipio).IsOptional();
            Property(x => x.duracao).IsOptional();
        }
    }
}
