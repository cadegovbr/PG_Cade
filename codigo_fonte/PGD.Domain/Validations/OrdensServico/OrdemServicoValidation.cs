using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.OrdensServico;

namespace PGD.Domain.Validations.OrdensServico
{
    public class OrdemServicoValidation : Validator<OrdemServico>
    {
        public OrdemServicoValidation()
        {
            var DatasOS = new DataDeveSerMaiorQueAtualSpecification();

            base.Add("DatasOS", new Rule<OrdemServico>(DatasOS, "Data início no sistema não pode ser inferior que a atual."));
        }
    }
}
