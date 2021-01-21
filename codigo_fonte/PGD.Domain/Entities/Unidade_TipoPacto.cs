using DomainValidation.Validation;
using PGD.Domain.Entities.RH;

namespace PGD.Domain.Entities
{
    public class Unidade_TipoPacto
    {
        public Unidade_TipoPacto()
        {
            ValidationResult = new ValidationResult();
        }
        public int IdUnidade_TipoPacto { get; set; }        
        public int IdUnidade { get; set; } 
        public int IdTipoPacto { get; set; }
        public bool IndPermitePactoExterior { get; set; }        
        public ValidationResult ValidationResult { get; set; }
        public virtual TipoPacto TipoPacto { get; set; }
        public virtual Unidade Unidade { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
