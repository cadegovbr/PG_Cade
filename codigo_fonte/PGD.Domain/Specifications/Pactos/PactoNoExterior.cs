using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class PactoNoExterior : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            bool isSatisfied = true;

            isSatisfied = !pacto.PactoExecutadoNoExterior || (pacto.PactoExecutadoNoExterior && (!String.IsNullOrEmpty(pacto.ProcessoSEI)));

            return isSatisfied;
        }
    }
}
