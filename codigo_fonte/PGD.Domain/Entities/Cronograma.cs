using DomainValidation.Validation;
using System;

namespace PGD.Domain.Entities
{
    public class Cronograma
    {
        public Cronograma()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdCronograma { get; set; }
        public int IdPacto { get; set; }
        public DateTime DataCronograma { get; set; }
        public double HorasCronograma { get; set; }
        public bool DiaUtil { get; set; }
        public bool Feriado { get; set; }
        public double? DuracaoFeriado { get; set; }
        public bool Suspenso { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
