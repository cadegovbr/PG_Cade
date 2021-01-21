using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class DataRetornoSuspensaoDeveSerMaiorQueInicio : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto entity)
        {
            if (entity.SuspensoAte.HasValue && entity.SuspensoAte.Value < entity.SuspensoAPartirDe.GetValueOrDefault())
            {
                return false;
            }
            return true;
        }
    }
}
