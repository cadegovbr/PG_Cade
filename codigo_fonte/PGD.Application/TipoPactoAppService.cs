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
    public class TipoPactoAppService : ApplicationService, ITipoPactoAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoPactoService _tipoPactoService;

        public TipoPactoAppService(IUsuarioService usuarioService, ILogService logService, IUnitOfWork uow, ITipoPactoService tipoPactoService )
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _tipoPactoService = tipoPactoService;
        }

        public TipoPactoViewModel Adicionar(TipoPactoViewModel tipoPactoViewModel)
        {
            throw new NotImplementedException();
        }

        public TipoPactoViewModel Atualizar(TipoPactoViewModel tipoPactoViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoPactoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<TipoPacto>, IEnumerable<TipoPactoViewModel>>(_tipoPactoService.ObterTodos());
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
