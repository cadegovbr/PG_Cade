using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;

namespace PGD.Domain.Specifications.Atividades
{
    public class PercentualZeroACemSpecification : ISpecification<Atividade>
    {
        public bool IsSatisfiedBy(Atividade ativ)
        {          

            return ativ.PctMinimoReducao >= 0 && ativ.PctMinimoReducao <= 100;
            
        }
    }
}
