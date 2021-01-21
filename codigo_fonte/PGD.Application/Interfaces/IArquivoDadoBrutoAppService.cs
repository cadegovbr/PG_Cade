using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IArquivoDadoBrutoAppService
    {
        IEnumerable<ArquivoDadoBrutoViewModel> ObterTodos();

        IEnumerable<ArquivoDadoBrutoViewModel> ObterPorAno(int ano);
    }
}