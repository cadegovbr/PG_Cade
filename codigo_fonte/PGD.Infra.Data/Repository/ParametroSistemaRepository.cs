using PGD.Domain.Entities;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
        Justification = "False positive.")]
    public class ParametroSistemaRepository : Repository<ParametroSistema>, IParametroSistemaRepository
    {
        public ParametroSistemaRepository(PGDDbContext context)
           : base(context)
        {
        }

        public int? ObterValorInt(eParametrosSistema parametroSistema)
        {
            return ObterPorId((int)parametroSistema)?.IntValue;
        }

        public bool? ObterValorBool(eParametrosSistema parametroSistema)
        {
            return ObterPorId((int)parametroSistema)?.BoolValue;
        }

        public string ObterValor(eParametrosSistema parametroSistema)
        {
            return ObterPorId((int)parametroSistema)?.Valor;
        }

        
    }
}
