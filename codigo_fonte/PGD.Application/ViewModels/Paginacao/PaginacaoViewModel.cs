using System.Collections.Generic;

namespace PGD.Application.ViewModels.Paginacao
{
    public class PaginacaoViewModel<T>
    {
        public int QtRegistros { get; set; }
        public List<T> Lista { get; set; }
    }
}