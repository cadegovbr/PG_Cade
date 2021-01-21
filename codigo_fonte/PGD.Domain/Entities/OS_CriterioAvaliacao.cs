using DomainValidation.Validation;
using System.Collections.Generic;

namespace PGD.Domain.Entities
{
    public class OS_CriterioAvaliacao
    {
        public OS_CriterioAvaliacao()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdCriterioAvaliacao { get; set; }
        public int IdCriterioAvaliacaoOriginal { get; set; }

        public string DescCriterioAvaliacao { get; set; }
        public string StrTextoExplicativo { get; set; }

        public int IdOrdemServico { get; set; }

        public virtual OrdemServico OrdemServico { get; set; }

        public bool Inativo { get; set; }

        public virtual ICollection<OS_ItemAvaliacao> ItensAvaliacao { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
