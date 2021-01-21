using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.OrdensServico
{
    public class DataDeveSerMaiorQueAtualSpecification : ISpecification<OrdemServico>
    {
        public bool IsSatisfiedBy(OrdemServico os)
        {
            if (os.IdOrdemServico == 0)
            {
                return os.DatInicioSistema.Date >= DateTime.Now.Date;
            }
            else
            {
                return true;
            }
            
        }
    }
}
