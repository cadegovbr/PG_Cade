using DomainValidation.Validation;

namespace PGD.Domain.Entities
{
    public class NivelAvaliacao
    {
        public NivelAvaliacao()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdNivelAvaliacao { get; set; }
        public string DescNivelAvaliacao { get; set; }        
        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
