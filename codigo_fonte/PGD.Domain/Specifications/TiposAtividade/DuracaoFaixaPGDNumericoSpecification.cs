using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.TiposAtividade
{
    public class DuracaoFaixaPGDNumericoSpecification : ISpecification<TipoAtividade>
    {
        public bool IsSatisfiedBy(TipoAtividade obj)
        {
            return obj.DuracaoFaixa <= 9999.99;
        }
    }
}
