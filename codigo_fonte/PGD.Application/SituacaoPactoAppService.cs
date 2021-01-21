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
    public class SituacaoPactoAppService : ApplicationService, ISituacaoPactoAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;
        private readonly ISituacaoPactoService _situacaoPactoService;

        public SituacaoPactoAppService(IUsuarioService usuarioService, ILogService logService, IUnitOfWork uow, ISituacaoPactoService situacaoPactoService )
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _situacaoPactoService = situacaoPactoService;
        }

        public SituacaoPactoViewModel Adicionar(SituacaoPactoViewModel situacaoPactoViewModel)
        {
            throw new NotImplementedException();
        }

        public SituacaoPactoViewModel Atualizar(SituacaoPactoViewModel situacaoPactoViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SituacaoPactoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<SituacaoPacto>, IEnumerable<SituacaoPactoViewModel>>(_situacaoPactoService.ObterTodos());
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
