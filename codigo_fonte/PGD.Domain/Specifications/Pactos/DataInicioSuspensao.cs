using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class DataInicioSuspensao : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto entity)
        {
            if (entity.SuspensoAPartirDe.HasValue && entity.SuspensoAPartirDe.Value.Date > entity.DataPrevistaTermino.Date)
            {
                return false;
            }

            return true;
        }
    }
}
