using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;

namespace PGD.Domain.Specifications.ItensAvaliacao
{
    public class ImpactoNotaMenosDezAZeroSpecification : ISpecification<ItemAvaliacao>
    {
        public bool IsSatisfiedBy(ItemAvaliacao entity)
        {          

            return entity.ImpactoNota >= -10 && entity.ImpactoNota <= 0;
            
        }
    }
}
