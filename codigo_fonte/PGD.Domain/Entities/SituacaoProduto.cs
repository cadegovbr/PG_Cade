using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainValidation.Validation;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class SituacaoProduto
    {

        public SituacaoProduto()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdSituacaoProduto { get; set; }
        public string DescSituacaoProduto { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
