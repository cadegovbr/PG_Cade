using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class ArquivoDadoBruto
    {
        public ArquivoDadoBruto()
        {
            ValidationResult = new ValidationResult();
        }
        public string Ano { get; set; }
        public string NomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
        public string DataArquivo { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
