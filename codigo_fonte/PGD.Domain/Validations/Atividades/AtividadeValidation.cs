using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.Atividades;

namespace PGD.Domain.Validations.Atividades
{
    public class AtividadeValidation : Validator<Atividade>
    {
        public AtividadeValidation()
        {
            var TipoAtividadeUnica = new TipoAtividadeUnicaSpecification();
            base.Add("TipoAtividadeUnica", new Rule<Atividade>(TipoAtividadeUnica, "O 'Tipo de Faixa' deve ser único."));

            var PercentualMinimoZeroACem = new PercentualZeroACemSpecification();
            base.Add("PorcentagemZeroACem", new Rule<Atividade>(PercentualMinimoZeroACem, "Percentual Mínimo de Redução deve ser um número de 0 à 100."));

            //var PercentualInteiro = new PercentualInteiroSpecification();
            //base.Add("PorcentagemInteiro", new Rule<Atividade>(PercentualInteiro, "O Percentual Mínimo de Redução deve ser um número inteiro."));
        }
    }
}
