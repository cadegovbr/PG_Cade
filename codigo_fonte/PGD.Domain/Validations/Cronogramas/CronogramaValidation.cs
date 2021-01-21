using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.Cronogramas;
using System;

namespace PGD.Domain.Validations.Cronogramas
{
    public class CronogramaValidation : Validator<Cronograma>
    {
        public CronogramaValidation(double horasDiarias, DateTime dataCronograma, double qtdHorasOcupadasPorOutroPacto)
        {
            var HorasDiariasExcedentes = new DiaCronogramaNaoExcedeHorasDiarias(horasDiarias: horasDiarias, horasOcupadasOutrosPactos: qtdHorasOcupadasPorOutroPacto);
            base.Add("HorasDiariasExcedentes", new Rule<Cronograma>(HorasDiariasExcedentes, $"{dataCronograma.ToString("dd/MM/yyyy")} - Quantidade de horas superior ao máximo de horas permitidas por dia ({TimeSpan.FromHours(horasDiarias).ToString(@"hh\:mm")})."));

            var HorasFeriadoExcedentes = new DiaCronogramaNaoExcedeHorasFeriado(horasDiarias: horasDiarias, horasOcupadasOutrosPactos: qtdHorasOcupadasPorOutroPacto);
            base.Add("HorasFeriadoExcedentes", new Rule<Cronograma>(HorasFeriadoExcedentes, $"{dataCronograma.ToString("dd/MM/yyyy")} - Quantidade de horas superior ao máximo de horas permitidas no feriado."));

        }
    }
}
