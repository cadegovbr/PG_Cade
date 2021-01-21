using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;

namespace PGD.Domain.Specifications.TiposAtividade
{
    public class DuracaoFaixaPresencialNumericoSpecification : ISpecification<TipoAtividade>
    {
        public bool IsSatisfiedBy(TipoAtividade obj)
        {
            return obj.DuracaoFaixaPresencial <= 9999.99;
        }
    }
}
