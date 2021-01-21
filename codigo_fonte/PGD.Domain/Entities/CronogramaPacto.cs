using DomainValidation.Validation;
using PGD.Domain.Validations;
using PGD.Domain.Validations.Cronogramas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class CronogramaPacto
    {
        public CronogramaPacto()
        {
            Cronogramas = new List<Cronograma>();
            ValidationResult = new ValidationResult();
        }
        
        public int IdPacto { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime? DataInicioSuspensao { get; set; }
        public DateTime? DataFimSuspensao { get; set; }
        public DateTime? DataInicioImpedimento { get; set; }

        public double HorasDiarias { get; set; }
        public double HorasTotais { get; set; }
        
        public string CPFUsuario { get; set; }
        
        
        public List<Cronograma> Cronogramas { get; set; }

        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            // ValidationResult = new CronogramaPactoValidation().Validate(this);

            return ValidationResult.IsValid;
        }

    }
}
