using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class Administrador
    {
        public Administrador()
        {
            ValidationResult = new ValidationResult();
        }
        public int IdAdministrador { get; set; }
        public string CPF { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
