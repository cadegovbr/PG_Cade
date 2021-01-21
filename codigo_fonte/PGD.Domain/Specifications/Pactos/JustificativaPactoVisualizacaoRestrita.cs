using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class JustificativaPactoVisualizacaoRestrita : ISpecification<Pacto>
    {
        public bool IsSatisfiedBy(Pacto entity)
        {
            return !entity.IndVisualizacaoRestrita || (entity.IndVisualizacaoRestrita && !string.IsNullOrEmpty(entity.JustificativaVisualizacaoRestrita));
            
        }
    }
}
