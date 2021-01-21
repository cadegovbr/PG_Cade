using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IArquivoDadoBrutoRepository
    {
        IEnumerable<ArquivoDadoBruto> ObterTodos();

        IEnumerable<ArquivoDadoBruto> ObterPorAno(int ano);
    }
}
