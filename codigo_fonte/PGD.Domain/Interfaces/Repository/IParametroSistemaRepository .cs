using PGD.Domain.Entities;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IParametroSistemaRepository : IRepository<ParametroSistema>
    {
        int? ObterValorInt(eParametrosSistema parametroSistema);

        bool? ObterValorBool(eParametrosSistema parametroSistema);

        string ObterValor(eParametrosSistema parametroSistema);
    }
}
