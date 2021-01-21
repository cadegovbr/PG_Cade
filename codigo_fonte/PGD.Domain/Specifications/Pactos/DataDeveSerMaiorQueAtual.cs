using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class DataDeveSerMaiorQueAtual : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            return pacto.DataPrevistaInicio.Date >= DateTime.Now.Date || pacto.IdPacto > 0 || pacto.DataPrevistaInicio == null;
        }
    }
}
