using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Cronogramas
{
    public class CronogramaNaoPossuiHorasExcedentes : ISpecification<CronogramaPacto>
    {
        public bool IsSatisfiedBy(CronogramaPacto cronogramaPacto)
        {
            double cargaHorariaDistribuir = cronogramaPacto.HorasTotais - (Math.Round(Convert.ToDouble(cronogramaPacto.Cronogramas.Sum(c => c.HorasCronograma)), 2));
            return cargaHorariaDistribuir >= 0;
        }
    }
}
