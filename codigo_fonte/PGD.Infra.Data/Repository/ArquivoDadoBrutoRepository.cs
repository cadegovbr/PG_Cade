using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace PGD.Infra.Data.Repository
{

    public class ArquivoDadoBrutoRepository : Repository<ArquivoDadoBruto>, IArquivoDadoBrutoRepository
    {

        public ArquivoDadoBrutoRepository(PGDDbContext context)
            : base(context)
        {

        }

        public IEnumerable<ArquivoDadoBruto> ObterPorAno(int ano)
        {
            throw new NotImplementedException();
        }
    }
}
