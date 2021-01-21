using System.Collections.Generic;

namespace PGD.Domain.Paginacao
{
    public class Paginacao<T>
    {
        public Paginacao()
        {
            Lista = new List<T>();
        }
        public int QtRegistros { get; set; }
        public List<T> Lista { get; set; }
    }
}