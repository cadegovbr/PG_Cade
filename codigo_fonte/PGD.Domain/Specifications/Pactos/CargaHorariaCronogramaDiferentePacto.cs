using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class CargaHorariaCronogramaDiferentePacto : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            if (pacto.PossuiCargaHoraria)
            {
                if (pacto.Cronogramas != null && pacto.Cronogramas.Count > 0)
                {
                    return !pacto.Cronogramas.Any(x => x.HorasCronograma > Convert.ToDouble(pacto.CargaHoraria));
                }
                
            }
            return true;
        }
    }
}
