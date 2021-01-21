using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Cronogramas
{
    public class DiaCronogramaNaoExcedeHorasFeriado : ISpecification<Cronograma>
    {
        public double HorasDiarias { get; set; }
        public double HorasOcupadasOutrosPactos { get; set; }


        public DiaCronogramaNaoExcedeHorasFeriado(double horasDiarias, double horasOcupadasOutrosPactos)
        {
            HorasDiarias = horasDiarias;
            HorasOcupadasOutrosPactos = horasOcupadasOutrosPactos;
        }

        public bool IsSatisfiedBy(Cronograma cronograma)
        {
            var horasFeriado = cronograma.DuracaoFeriado;
            if (horasFeriado.GetValueOrDefault() > Convert.ToDouble(HorasDiarias)) horasFeriado = Convert.ToDouble(HorasDiarias);

            return (!cronograma.Feriado)
                || cronograma.HorasCronograma <= (Convert.ToDouble(HorasDiarias) - horasFeriado - HorasOcupadasOutrosPactos);
            
        }
    }
}
