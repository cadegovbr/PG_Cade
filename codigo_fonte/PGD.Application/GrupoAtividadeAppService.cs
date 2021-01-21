using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application
{
    public class GrupoAtividadeAppService : ApplicationService, IGrupoAtividadeAppService
    {
        private readonly ILogService _logService;
        private readonly IAtividadeService _atividadeService;
        private readonly IGrupoAtividadeService _grupoAtividadeService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoPactoService _tipoPactoService;

        public GrupoAtividadeAppService(IUsuarioService usuarioService, IUnitOfWork uow, IAtividadeService atividadeService, 
            ILogService logService, IGrupoAtividadeService grupoatividadeService, ITipoPactoService tipoPactoService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _atividadeService = atividadeService;
            _logService = logService;
            _grupoAtividadeService = grupoatividadeService;
            _tipoPactoService = tipoPactoService;
        }

        public GrupoAtividadeViewModel Adicionar(GrupoAtividadeViewModel grupoatividadeViewModel)
        {
            var grupoatividade = Mapper.Map<GrupoAtividadeViewModel, GrupoAtividade>(grupoatividadeViewModel);

            BeginTransaction();
            grupoatividade.Atividades.Clear();
            if (grupoatividadeViewModel.idsAtividades != null)
                grupoatividadeViewModel.idsAtividades.ForEach(x =>
                {
                    var item = _atividadeService.ObterPorId(x);
                    grupoatividade.Atividades.Add(item);
                });

            grupoatividade.TiposPacto.Clear();
            if (grupoatividadeViewModel.IdsTipoPacto != null)
            {
                grupoatividadeViewModel.IdsTipoPacto.ForEach(x => grupoatividade.TiposPacto.Add(_tipoPactoService.ObterPorId(x)));
            }
                

            var grupoatividadeReturn = _grupoAtividadeService.Adicionar(grupoatividade);
            if (grupoatividadeReturn.ValidationResult.IsValid)
            {
                _logService.Logar(grupoatividade, grupoatividadeViewModel.Usuario.CPF, Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }
            grupoatividadeViewModel = Mapper.Map<GrupoAtividade, GrupoAtividadeViewModel>(grupoatividadeReturn);
            return grupoatividadeViewModel;
        }

        public GrupoAtividadeViewModel Atualizar(GrupoAtividadeViewModel grupoatividadeViewModel)
        {
            var grupoatividade = Mapper.Map<GrupoAtividadeViewModel, GrupoAtividade>(grupoatividadeViewModel);

            BeginTransaction();

            var _grupo = _grupoAtividadeService.ObterPorId(grupoatividadeViewModel.IdGrupoAtividade);

            if (_grupo != null)
            {
                _grupo.NomGrupoAtividade = grupoatividadeViewModel.NomGrupoAtividade;
                _grupo.Atividades.Clear();
                _grupo.TiposPacto.Clear();

                if (grupoatividadeViewModel.idsAtividades != null)
                    grupoatividadeViewModel.idsAtividades.ForEach(x =>
                    {
                        var item = _atividadeService.ObterPorId(x);
                        _grupo.Atividades.Add(item);
                    });

                if (grupoatividadeViewModel.IdsTipoPacto != null)
                {
                    grupoatividadeViewModel.IdsTipoPacto.ForEach(x => _grupo.TiposPacto.Add(_tipoPactoService.ObterPorId(x)));
                }

                var grupoatividadeReturn = _grupoAtividadeService.Atualizar(_grupo);

                if (grupoatividadeReturn.ValidationResult.IsValid)
                {
                    _logService.Logar(grupoatividade, grupoatividadeViewModel.Usuario.CPF, Domain.Enums.Operacao.Alteração.ToString());
                    Commit();
                }

                grupoatividadeViewModel = Mapper.Map<GrupoAtividade, GrupoAtividadeViewModel>(grupoatividadeReturn);
            }
            
            return grupoatividadeViewModel;
        }

        public List<AtividadeViewModel> PreencheList(List<int> identificadores)
        {
            var lista = new List<AtividadeViewModel>();
            if (identificadores != null)
                identificadores.ForEach(x =>
                {
                    var item = _atividadeService.ObterPorId(x);
                    lista.Add(Mapper.Map<Atividade, AtividadeViewModel>(item));
                });
            return lista;
        }
        public List<TipoPactoViewModel> PreencheListTipoPacto(List<int> identificadores)
        {
            var lista = new List<TipoPactoViewModel>();
            if (identificadores != null)
            {
                identificadores.ForEach(x =>
                {
                    var item = _tipoPactoService.ObterPorId(x);
                    lista.Add(Mapper.Map<TipoPacto, TipoPactoViewModel>(item));
                });
            }
            return lista;
        }


        public IEnumerable<GrupoAtividadeViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<GrupoAtividade>, IEnumerable<GrupoAtividadeViewModel>>(_grupoAtividadeService.ObterTodosAtivos());
        }

        public GrupoAtividadeViewModel ObterPorId(int id)
        {
            return Mapper.Map<GrupoAtividade, GrupoAtividadeViewModel>(_grupoAtividadeService.ObterPorId(id));
        }

        public GrupoAtividadeViewModel Remover(GrupoAtividadeViewModel grupoatividadeViewModel)
        {
            var grupoatividade = Mapper.Map<GrupoAtividadeViewModel, GrupoAtividade>(grupoatividadeViewModel);

            BeginTransaction();
            var grupoatividadeReturn = _grupoAtividadeService.Remover(grupoatividade);
            _logService.Logar(grupoatividade, grupoatividadeViewModel.Usuario.CPF, Domain.Enums.Operacao.Exclusão.ToString());
            Commit();

            return Mapper.Map<GrupoAtividade, GrupoAtividadeViewModel>(grupoatividadeReturn); ;
        }
    }
}
