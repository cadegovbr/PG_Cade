using DomainValidation.Validation;
namespace PGD.Domain.Entities
{
    public class Justificativa
    {
        public Justificativa()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdJustificativa { get; set; }
        public string DescJustificativa { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
