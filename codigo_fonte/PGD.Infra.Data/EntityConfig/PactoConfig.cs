using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class PactoConfig : EntityTypeConfiguration<Pacto>
    {
        public PactoConfig()
        {
            HasKey(x => x.IdPacto);
            Property(x => x.Nome).IsRequired();
            Property(x => x.DataPrevistaInicio).IsRequired();
            Property(x => x.IdOrdemServico).IsRequired();
            Property(x => x.SuspensoAPartirDe).IsOptional();
            Property(x => x.SuspensoAte).IsOptional();            
            Ignore(c => c.ValidationResult);
            Property(x => x.DataTerminoReal).IsOptional();
            Property(x => x.CpfUsuarioDirigente).IsOptional();
            Property(x => x.CpfUsuarioSolicitante).IsOptional();
            Property(x => x.DataAprovacaoDirigente).IsOptional();
            Property(x => x.DataAprovacaoSolicitante).IsOptional();
            Property(x => x.StatusAprovacaoDirigente).IsOptional();
            Property(x => x.StatusAprovacaoSolicitante).IsOptional();
            Property(x => x.IdTipoPacto).IsRequired();
            Property(x => x.TAP).IsOptional();


        }
    }
}
