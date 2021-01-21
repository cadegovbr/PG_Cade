using DomainValidation.Validation;
using System.Collections.Generic;

namespace PGD.Domain.Entities
{
    public class CriterioAvaliacao
    {
        public CriterioAvaliacao()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdCriterioAvaliacao { get; set; }
        public string DescCriterioAvaliacao { get; set; }
        public string StrTextoExplicativo { get; set; }
        public bool Inativo { get; set; }
        public virtual ICollection<ItemAvaliacao> ItensAvaliacao { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
