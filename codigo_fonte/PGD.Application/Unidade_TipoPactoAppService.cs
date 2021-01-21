using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System.Collections.Generic;
using PGD.Application.ViewModels.Filtros;
using PGD.Application.ViewModels.Paginacao;
using PGD.Domain.Filtros;

namespace PGD.Application
{
    public class Unidade_TipoPactoAppService : ApplicationService, IUnidade_TipoPactoAppService
    {
        private readonly IUnidade_TipoPactoService _unidade_TipoPactoService;
        private readonly ILogService _logService;
        private readonly IUsuarioAppService _usuarioAppService;
        
        public Unidade_TipoPactoAppService(IUnitOfWork uow, IUnidade_TipoPactoService unidade_TipoPactoService, ILogService logService,
            IUsuarioAppService usuarioAppService)
            : base(uow)
        {
            _usuarioAppService = usuarioAppService;
            _logService = logService;
            _unidade_TipoPactoService = unidade_TipoPactoService;
        }

        public Unidade_TipoPactoViewModel Adicionar(Unidade_TipoPactoViewModel unidade_tipoPactoViewModel, UsuarioViewModel user)
        {
            Unidade_TipoPacto unidade_TipoPacto = Mapper.Map<Unidade_TipoPactoViewModel, Unidade_TipoPacto>(unidade_tipoPactoViewModel);

            BeginTransaction();

            var unidade_TipoPactoReturn = _unidade_TipoPactoService.Adicionar(unidade_TipoPacto);

            if(unidade_TipoPactoReturn.ValidationResult.IsValid)
            {
                _logService.Logar(unidade_TipoPacto, user.CPF.ToString(), Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }

            unidade_tipoPactoViewModel = Mapper.Map<Unidade_TipoPacto, Unidade_TipoPactoViewModel>(unidade_TipoPactoReturn);

            return unidade_tipoPactoViewModel;
        }

        public Unidade_TipoPactoViewModel Atualizar(Unidade_TipoPactoViewModel unidade_tipoPactoViewModel, UsuarioViewModel user)
        {
            var unidade_TipoPacto = Mapper.Map<Unidade_TipoPactoViewModel, Unidade_TipoPacto>(unidade_tipoPactoViewModel);

            BeginTransaction();

            var unidade_TipoPactoReturn = _unidade_TipoPactoService.Atualizar(unidade_TipoPacto);

            if(unidade_TipoPactoReturn.ValidationResult.IsValid)
            {
                _logService.Logar(unidade_TipoPacto, user.CPF.ToString(), Domain.Enums.Operacao.Alteração.ToString());
                Commit();
            }

            unidade_tipoPactoViewModel = Mapper.Map<Unidade_TipoPacto, Unidade_TipoPactoViewModel>(unidade_TipoPactoReturn);

            return unidade_tipoPactoViewModel;
        }

        public Unidade_TipoPactoViewModel BuscarPorIdUnidadeTipoPacto(int idUnidade, int idTipoPacto)
        {
            return Mapper.Map<Unidade_TipoPacto, Unidade_TipoPactoViewModel>(_unidade_TipoPactoService.BuscarPorIdUnidadeTipoPacto(idUnidade, idTipoPacto));
        }

        public IEnumerable<Unidade_TipoPactoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<Unidade_TipoPacto>, IEnumerable<Unidade_TipoPactoViewModel>>(_unidade_TipoPactoService.ObterTodos());
        }

        public PaginacaoViewModel<Unidade_TipoPactoViewModel> Buscar(UnidadeTipoPactoFiltroViewModel filtro)
        {
            var retorno = Mapper.Map<PaginacaoViewModel<Unidade_TipoPactoViewModel>>(_unidade_TipoPactoService.Buscar(Mapper.Map<UnidadeTipoPactoFiltro>(filtro)));
            return retorno;
        }

        public void Remover(int id, UsuarioViewModel user)
        {
            var unidade_TipoPacto = _unidade_TipoPactoService.ObterPorId(id);

            BeginTransaction();

            var unidade_TipoPactoReturn = _unidade_TipoPactoService.Remover(unidade_TipoPacto);

            if(unidade_TipoPactoReturn.ValidationResult.IsValid)
            {
                _logService.Logar(unidade_TipoPacto, user.CPF.ToString(), Domain.Enums.Operacao.Exclusão.ToString());
                Commit();
            }
        }
    }
}
