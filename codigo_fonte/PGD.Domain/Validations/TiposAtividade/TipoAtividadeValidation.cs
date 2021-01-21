using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.TiposAtividade;

namespace PGD.Domain.Validations.TiposAtividade
{
    public class TipoAtividadeValidation : Validator<TipoAtividade>
    {
        public TipoAtividadeValidation()
        {
            var duracaoFaixa = new DuracaoFaixaPGDNumericoSpecification();
            var duracaoFaixaPresencial = new DuracaoFaixaPresencialNumericoSpecification();

            base.Add("duracaoFaixa", new Rule<TipoAtividade>(duracaoFaixa, "Duração da Faixa no PGD deve ser um número com 4 casas e 2 decimais"));
            base.Add("duracaoFaixaPresencial", new Rule<TipoAtividade>(duracaoFaixaPresencial, "Duração da Faixa Presencial deve ser um número com 4 casas e 2 decimais"));
        }
    }
}
