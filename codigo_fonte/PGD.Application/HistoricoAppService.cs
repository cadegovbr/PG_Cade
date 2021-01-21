using PGD.Application.Interfaces;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Application.ViewModels;
using AutoMapper;
using PGD.Domain.Entities;

namespace PGD.Application
{
    public class HistoricoAppService : ApplicationService, IHistoricoAppService
    {
        private readonly ILogService _logService;
        private readonly IHistoricoService _historicoService;

        public HistoricoAppService(IUsuarioService usuarioService, IUnitOfWork uow, IHistoricoService historicoService, ILogService logService)
            : base(uow)
        {
            _historicoService = historicoService;
            _logService = logService;
        }

        public HistoricoViewModel Adicionar(HistoricoViewModel historicoViewModel)
        {
            var historico = Mapper.Map<HistoricoViewModel, Historico>(historicoViewModel);

            BeginTransaction();

            var historicoReturn = _historicoService.Adicionar(historico);

            Commit();

            historicoViewModel = Mapper.Map<Historico, HistoricoViewModel>(historicoReturn);
            return historicoViewModel;
        }

        public HistoricoViewModel Atualizar(HistoricoViewModel historicoViewModel)
        {
            var historico = Mapper.Map<HistoricoViewModel, Historico>(historicoViewModel);

            BeginTransaction();

            var historicoReturn = _historicoService.Atualizar(historico);

            Commit();

            historicoViewModel = Mapper.Map<Historico, HistoricoViewModel>(historicoReturn);
            return historicoViewModel;
        }

        public HistoricoViewModel BuscarPorId(int idpacto, int idhistorico)
        {
            return Mapper.Map<Historico, HistoricoViewModel>(_historicoService.BuscarPorId(idpacto, idhistorico));
        }

        public IEnumerable<HistoricoViewModel> ObterTodos(HistoricoViewModel objFiltro)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HistoricoViewModel> ObterTodos(int idpacto)
        {
            return Mapper.Map<IEnumerable<Historico>, IEnumerable<HistoricoViewModel>>(_historicoService.ObterTodos(idpacto));
        }

        public HistoricoViewModel Remover(HistoricoViewModel historicoViewModel)
        {
            throw new NotImplementedException();
        }
    }
}