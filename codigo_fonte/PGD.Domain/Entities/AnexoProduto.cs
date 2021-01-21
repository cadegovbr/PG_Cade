using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
   public class AnexoProduto
    {

        public AnexoProduto()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdAnexoProduto { get; set; }

        public string Nome { get; set; }

        public string Tipo { get; set; }

        public int Tamanho { get; set; }

        public int IdProduto { get; set; }

        public IEnumerable<AnexoProduto> FileList { get; set; }

        public virtual Produto Produto { get; set; }

        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
