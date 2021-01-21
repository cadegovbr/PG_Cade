using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Cronogramas
{
    public class DiaCronogramaNaoSobrepoeProximoPacto : ISpecification<Cronograma>
    {
        public double HorasDiarias { get; set; }
        public DateTime? DataInicioImpedimento { get; set; }


        public DiaCronogramaNaoSobrepoeProximoPacto(double horasDiarias, DateTime? dataInicioImpedimento)
        {
            HorasDiarias = horasDiarias;
            DataInicioImpedimento = dataInicioImpedimento;
        }

        public bool IsSatisfiedBy(Cronograma cronograma)
        {
            return !DataInicioImpedimento.HasValue || cronograma.DataCronograma < DataInicioImpedimento.Value;
        }
    }
}
