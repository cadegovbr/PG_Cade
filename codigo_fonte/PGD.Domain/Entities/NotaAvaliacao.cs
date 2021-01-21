using DomainValidation.Validation;
namespace PGD.Domain.Entities
{
    public class NotaAvaliacao
    {
        public NotaAvaliacao()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdNotaAvaliacao { get; set; }
        public string DescNotaAvaliacao { get; set; }

        public bool IndAtivoAvaliacaoSimplificada { get; set; }
        public bool IndAtivoAvaliacaoDetalhada { get; set; }

        public decimal LimiteSuperiorFaixa { get; set; }
        public decimal LimiteInferiorFaixa { get; set; }

        public int? Conceito{ get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid() 
        {
            return true;
        }
    }
}
