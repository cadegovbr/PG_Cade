using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class CamposObrigatoriosSuspensao : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto entity)
        {
            if (entity.SuspensoAPartirDe.HasValue && string.IsNullOrEmpty(entity.Motivo))
            {
                return false;
            }

            return true;
        }
    }
}
