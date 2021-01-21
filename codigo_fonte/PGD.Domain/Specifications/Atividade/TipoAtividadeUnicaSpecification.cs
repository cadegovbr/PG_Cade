using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System.Linq;

namespace PGD.Domain.Specifications.Atividades
{
    public class TipoAtividadeUnicaSpecification : ISpecification<Atividade>
    {
        public bool IsSatisfiedBy(Atividade ativ)
        {
            var duplicateKeys = ativ.Tipos.ToList().GroupBy(x => x.Faixa)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);

            return duplicateKeys.Count() == 0;

            //var NaoexisteDuplicado = true;
            //var Nome = "";
            //foreach (var item in ativ.Tipos)
            //{                
            //    if (Nome == item.Faixa)
            //    {
            //        NaoexisteDuplicado =  false;
            //        break;
            //    }
            //    Nome = item.Faixa;
            //}
            //return NaoexisteDuplicado;
        }
    }
}
