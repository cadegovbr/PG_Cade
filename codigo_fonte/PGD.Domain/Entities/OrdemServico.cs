using DomainValidation.Validation;
using PGD.Domain.Validations.OrdensServico;
using System;
using System.Collections.Generic;

namespace PGD.Domain.Entities
{
    public class OrdemServico
    {
        public OrdemServico()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdOrdemServico { get; set; }
        public DateTime DatInicioSistema { get; set; }
        public DateTime DatInicioNormativo { get; set; }
        public string DescOrdemServico { get; set; }
        public bool Inativo { get; set; }
       
        public virtual ICollection<OS_GrupoAtividade> Grupos { get; set; }
        public virtual ICollection<OS_CriterioAvaliacao> CriteriosAvaliacao { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            ValidationResult = new OrdemServicoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
