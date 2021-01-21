using PGD.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using AutoMapper;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;

namespace PGD.Application.Interfaces
{
    public interface ISituacaoProdutoAppService
    {

        IEnumerable<SituacaoProdutoViewModel> ObterTodos();
    }
}
