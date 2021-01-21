using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Cronogramas
{
    public class DiaCronogramaNaoExcedeHorasDiarias : ISpecification<Cronograma>
    {
        public double HorasDiarias { get; set; }
        public double HorasOcupadasOutrosPactos { get; set; }


        public DiaCronogramaNaoExcedeHorasDiarias(double horasDiarias, double horasOcupadasOutrosPactos)
        {
            HorasDiarias = horasDiarias;
            HorasOcupadasOutrosPactos = horasOcupadasOutrosPactos;
        }

        public bool IsSatisfiedBy(Cronograma cronograma)
        {
            return cronograma.HorasCronograma <= (Convert.ToDouble(HorasDiarias) - HorasOcupadasOutrosPactos);
        }
    }
}
