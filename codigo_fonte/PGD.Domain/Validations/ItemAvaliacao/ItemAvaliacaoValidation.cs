using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.ItensAvaliacao;

namespace PGD.Domain.Validations.ItensAvaliacao
{
    public class ItemAvaliacaoValidation : Validator<ItemAvaliacao>
    {
        public ItemAvaliacaoValidation()
        {
            var impactoNotaMenosDezAZero = new ImpactoNotaMenosDezAZeroSpecification();
            base.Add("ImpactoNotaMenosDezAZero", new Rule<ItemAvaliacao>(impactoNotaMenosDezAZero, "Impacto na Nota deve ser um número de -10 a 0."));

        }
    }
}
