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
    public class RN055_CSU009_Suspender_Pacto : ISpecification<Pacto>
    {
        #region DESCRIÇÃO DA RN055
        //O usuário poderá suspender um pacto inúmeras vezes enquanto o mesmo estiver dentro do período de vigência.
        //Não é permitido suspender um pacto que já está suspenso ou que a data prevista de término do pacto seja anterior a atual.
        #endregion

        public bool IsSatisfiedBy(Pacto entity)
        {
            if((int)entity.SituacaoPacto != (int)eSituacaoPacto.Suspenso && entity.DataPrevistaTermino > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}
