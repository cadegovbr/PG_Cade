using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class NumerodeHorasCronogramaDiferentePacto : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            if (pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Suspenso && pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Interrompido)
            {
                if (pacto.Cronogramas != null && pacto.Cronogramas.Count > 0)
                {
                    double resultado = 0;
                    pacto.Cronogramas.ToList().ForEach(x => resultado += x.HorasCronograma);
                    if (pacto.CargaHorariaTotal > resultado)
                    {
                        var valorArr = Math.Ceiling(pacto.CargaHorariaTotal);
                        resultado = resultado + 1;
                        if (valorArr == resultado)
                            return true;
                    }
                    return pacto.CargaHorariaTotal == resultado;
                }
            }
            return true;
        }
    }
}
