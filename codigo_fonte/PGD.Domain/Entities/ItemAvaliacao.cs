using DomainValidation.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGD.Domain.Entities
{
    public class ItemAvaliacao
    {
        public ItemAvaliacao()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdItemAvaliacao { get; set; }
        public string DescItemAvaliacao { get; set; }
        public decimal ImpactoNota { get; set; }

        [ForeignKey("NotaMaxima")]
        public int IdNotaMaxima { get; set; }
        public virtual NotaAvaliacao NotaMaxima { get; set; }

        [ForeignKey("CriterioAvaliacao")]
        public int IdCriterioAvaliacao { get; set; }
        public virtual CriterioAvaliacao CriterioAvaliacao { get; set; }

        public bool Inativo { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
