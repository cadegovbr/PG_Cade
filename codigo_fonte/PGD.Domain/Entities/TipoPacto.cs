using DomainValidation.Validation;
using System.Collections.Generic;

namespace PGD.Domain.Entities
{
    public class TipoPacto
    {
        public TipoPacto()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdTipoPacto { get; set; }
        public string DescTipoPacto { get; set; }
        public virtual ICollection<GrupoAtividade> Grupos { get; set; }
        public virtual ICollection<OS_GrupoAtividade> OS_Grupos { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public virtual ICollection<Unidade_TipoPacto> UnidadesTiposPactos { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
