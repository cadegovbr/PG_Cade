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
    public class AvaliacaoDetalhadaValida : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto entity)
        {
            if (entity.Produtos != null)
            {
                var lstAvaliacoes = entity.Produtos.SelectMany(p => p.Avaliacoes).Where(a => a.IdNivelAvaliacao == (int)eNivelAvaliacao.Detalhada).ToList();

                if (lstAvaliacoes.Count > 0)
                {

                    return !lstAvaliacoes.SelectMany(a => a.AvaliacoesDetalhadas).Any(ad => (ad.IdOS_CriterioAvaliacao == 0 && ad.OS_CriterioAvaliacao == null) ||
                                                                                    (ad.IdOS_ItemAvaliacao == 0 && ad.OS_ItemAvaliacao == null));

                }
            }
            return true;
        }
    }
}
