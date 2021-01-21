using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;

namespace PGD.Domain.Specifications.Pactos
{
    public class TAPPacto : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            bool isSatisfied = true;

            isSatisfied = pacto.IdTipoPacto != (int)PGD.Domain.Enums.eTipoPacto.PGD_Projeto || (!String.IsNullOrEmpty(pacto.TAP));

            return isSatisfied;
        }
    }
}
