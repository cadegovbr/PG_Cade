using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{ 

    public class PeloMenosUmTelefone : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            return !string.IsNullOrEmpty(pacto.TelefoneFixoServidor)  || !string.IsNullOrEmpty(pacto.TelefoneMovelServidor);
        }
    }
}
