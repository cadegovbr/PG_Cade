using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.OrdensServico;
using PGD.Domain.Specifications.Pactos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Validations.Pactos
{
    public class ProdutoValidation : Validator<Produto>
    {
        public ProdutoValidation()
        {
          

        }
    }
}