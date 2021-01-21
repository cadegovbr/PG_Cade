using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Cronogramas
{
    public class CronogramaNaoPossuiHorasADistribuir : ISpecification<CronogramaPacto>
    {
        public bool IsSatisfiedBy(CronogramaPacto cronogramaPacto)
        {
            double cargaHorariaDistribuir = cronogramaPacto.HorasTotais - (Convert.ToDouble(cronogramaPacto.Cronogramas.Sum(c => Math.Round(c.HorasCronograma, 2) )));
            return cargaHorariaDistribuir <= 0;
            
        }
    }
}
