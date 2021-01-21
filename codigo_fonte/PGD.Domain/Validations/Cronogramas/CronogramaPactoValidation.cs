using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.Cronogramas;
using PGD.Domain.Specifications.TiposAtividade;

namespace PGD.Domain.Validations.Cronogramas
{
    public class CronogramaPactoValidation : Validator<CronogramaPacto>
    {
        public CronogramaPactoValidation(bool validarHorasADistribuir = true)
        {
            var HorasExcedentes = new CronogramaNaoPossuiHorasExcedentes();
            base.Add("HorasExcedentes", new Rule<CronogramaPacto>(HorasExcedentes, "Não é possível salvar o cronograma. Existem horas excedentes."));

            if (validarHorasADistribuir)
            {
                var HorasADistribuir = new CronogramaNaoPossuiHorasADistribuir();
                base.Add("HorasADistribuir", new Rule<CronogramaPacto>(HorasADistribuir, "Não é possível salvar o cronograma. Ainda restam horas a distribuir."));
            }
        }
    }
}
