using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;

namespace PGD.Domain.Specifications.Pactos
{
    public class ReducaoDeveSerMenorQueOito : ISpecification<Pacto>
    {

        const double EPSILON = 0.001;
        
        public bool IsSatisfiedBy(Pacto entity)
        {
            if (entity.PossuiCargaHoraria)
                return entity.CargaHoraria < 8 &&
                        entity.CargaHoraria > 0;


            return Math.Abs(entity.CargaHoraria - 8.0f) < EPSILON;
        }
    }
}
