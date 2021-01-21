using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System.Linq;

namespace PGD.Domain.Specifications.Atividades
{
    public class PercentualInteiroSpecification : ISpecification<Atividade>
    {
        public bool IsSatisfiedBy(Atividade ativ)
        {

                return ativ.PctMinimoReducao.GetType() == typeof(int);

        }
    }
}
