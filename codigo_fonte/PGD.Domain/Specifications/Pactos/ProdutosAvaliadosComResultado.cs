using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class ProdutosAvaliadosComResultado : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto pacto)
        {
            return ( pacto.IdSituacaoPacto != (int)eSituacaoPacto.Avaliado && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Interrompido ) ||
                     ( pacto.Produtos.SelectMany(p => p.Avaliacoes).Any() );
        }
    }
}
