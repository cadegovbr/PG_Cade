using DomainValidation.Validation;
using System;

namespace PGD.Domain.Entities
{
    public class Log
    {
        public Log()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdLog { get; set; }
        public string CpfUsuario { get; set; }
        //public  Usuario Usuario { get; set; }
        public DateTime Data { get; set; }
        public string Operacao { get; set; }
        public string Tabela { get; set; }
        public int IdTabela { get; set; }
        public string Valores { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
