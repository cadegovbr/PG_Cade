using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class RN032_CSU006_ReducaoDeveSerMenorQueOito : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            if (pacto.PossuiCargaHoraria)
                return pacto.CargaHoraria < 8;
            return pacto.CargaHoraria == 8;
        }
    }
}
