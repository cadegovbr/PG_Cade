using PGD.Domain.Entities.RH;
using System;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IFeriadoService : IService<Feriado>
    {
        IEnumerable<Feriado> ObterFeriados(DateTime dtAPartirDe);
        Feriado ObterFeriado(DateTime data);
        bool VerificaFeriado(DateTime dataAVerificar);
    }
}
