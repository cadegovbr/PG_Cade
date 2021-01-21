using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class   DevemExistirProdutosNoPacto : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            return pacto.CargaHorariaTotal > 0;
        }
    }
}
