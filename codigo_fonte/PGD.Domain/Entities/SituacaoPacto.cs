using DomainValidation.Validation;
namespace PGD.Domain.Entities
{
    public class SituacaoPacto
    {
        public SituacaoPacto()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdSituacaoPacto { get; set; }
        public string DescSituacaoPacto { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
