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

namespace PGD.Application
{
    public class SituacaoProdutoAppService : ApplicationService, ISituacaoProdutoAppService
    {
        private readonly ILogService _logService;
        private readonly ISituacaoProdutoService _situacaoProdutoService;

        public SituacaoProdutoAppService(ILogService logService, IUnitOfWork uow, ISituacaoProdutoService situacaoProdutoService)
           : base(uow)
        {
            
            _logService = logService;
            _situacaoProdutoService = situacaoProdutoService;
        }

        public IEnumerable<SituacaoProdutoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<SituacaoProduto>, IEnumerable<SituacaoProdutoViewModel>>(_situacaoProdutoService.ObterTodos());
        }
    }
}
