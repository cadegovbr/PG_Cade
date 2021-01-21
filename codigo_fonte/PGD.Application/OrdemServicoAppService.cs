using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Application
{
    public class OrdemServicoAppService : ApplicationService, IOrdemServicoAppService
    {
        private readonly ILogService _logService;
        private readonly IOrdemServicoService _ordemservicoService;
        private readonly IGrupoAtividadeService _grupoAtividadeService;
        private readonly IOS_GrupoAtividadeService _osgrupoAtividadeService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoPactoService _tipoPactoService;
        private readonly ICriterioAvaliacaoService _criterioAvaliacaoService;
        private readonly IItemAvaliacaoService _itemAvaliacaoService;
        private readonly INotaAvaliacaoService _notaAvaliacaoService;

#pragma warning disable S107 // Methods should not have too many parameters
        public OrdemServicoAppService(IUsuarioService usuarioService, IUnitOfWork uow, IOrdemServicoService ordemservicoService, ILogService logService, 
            IGrupoAtividadeService grupoatividadeService, IOS_GrupoAtividadeService osgrupoAtividadeService, ITipoPactoService tipoPactoService,
            ICriterioAvaliacaoService criterioAvaliacaoService, IItemAvaliacaoService itemAvaliacaoService,
            INotaAvaliacaoService notaAvaliacaoService)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(uow)
        {
            _usuarioService = usuarioService;
            _ordemservicoService = ordemservicoService;
            _logService = logService;
            _grupoAtividadeService = grupoatividadeService;
            _osgrupoAtividadeService = osgrupoAtividadeService;
            _tipoPactoService = tipoPactoService;
            _criterioAvaliacaoService = criterioAvaliacaoService;
            _itemAvaliacaoService = itemAvaliacaoService;
            _notaAvaliacaoService = notaAvaliacaoService;
        }

        public OrdemServicoViewModel Adicionar(OrdemServicoViewModel ordemServicoViewModel)
        {
            var OrdemServico = Mapper.Map<OrdemServicoViewModel, OrdemServico>(ordemServicoViewModel);

            BeginTransaction();

            foreach (var grupo in OrdemServico.Grupos)
            {
                grupo.IdGrupoAtividadeOriginal = grupo.IdGrupoAtividade;

                var listaTipos = new List<TipoPacto>();
                foreach (var tipo in grupo.TiposPacto)
                {
                    var t = _tipoPactoService.ObterPorId(tipo.IdTipoPacto);
                    listaTipos.Add(t);
                }

                grupo.TiposPacto = listaTipos;
            }

            foreach (var criterioAvaliacao in OrdemServico.CriteriosAvaliacao)
            {
                criterioAvaliacao.IdCriterioAvaliacaoOriginal = criterioAvaliacao.IdCriterioAvaliacao;

                foreach (var itemAvaliacao in criterioAvaliacao.ItensAvaliacao)
                {
                    itemAvaliacao.NotaMaxima = _notaAvaliacaoService.ObterPorId(itemAvaliacao.IdNotaMaxima);                    
                }

            }

            var osReturn = _ordemservicoService.Adicionar(OrdemServico);
            if (osReturn.ValidationResult.IsValid)
            {
                _logService.Logar(OrdemServico, ordemServicoViewModel.Usuario.CPF, Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }
            ordemServicoViewModel = Mapper.Map<OrdemServico, OrdemServicoViewModel>(osReturn);
            return ordemServicoViewModel;
        }

        public OrdemServicoViewModel Atualizar(OrdemServicoViewModel ordemServicoViewModel)
        {
            BeginTransaction();

            var os = _ordemservicoService.ObterPorId(ordemServicoViewModel.IdOrdemServico.Value);           
            os.Grupos = null;
            os.CriteriosAvaliacao = null;

            var OrdemServico = Mapper.Map<OrdemServicoViewModel, OrdemServico>(ordemServicoViewModel);
            
            if (OrdemServico != null)
            {
                foreach (var grupo in OrdemServico.Grupos)
                {
                    grupo.IdGrupoAtividadeOriginal = grupo.IdGrupoAtividade;

                    var listaTipos = new List<TipoPacto>();
                    foreach (var tipo in grupo.TiposPacto)
                    {
                        var t = _tipoPactoService.ObterPorId(tipo.IdTipoPacto);
                        listaTipos.Add(t);
                    }

                    grupo.TiposPacto = listaTipos;
                }

                foreach (var criterioAvaliacao in OrdemServico.CriteriosAvaliacao)
                {
                    criterioAvaliacao.IdCriterioAvaliacaoOriginal = criterioAvaliacao.IdCriterioAvaliacao;

                    foreach (var itemAvaliacao in criterioAvaliacao.ItensAvaliacao)
                    {
                        itemAvaliacao.NotaMaxima = _notaAvaliacaoService.ObterPorId(itemAvaliacao.IdNotaMaxima);
                    }

                }

                os.DatInicioNormativo = ordemServicoViewModel.DatInicioNormativo;
                os.DatInicioSistema = ordemServicoViewModel.DatInicioSistema;
                os.DescOrdemServico = ordemServicoViewModel.DescOrdemServico;
                os.IdOrdemServico = ordemServicoViewModel.IdOrdemServico.Value;
                os.Grupos = OrdemServico.Grupos;
                os.CriteriosAvaliacao = OrdemServico.CriteriosAvaliacao;

                var osReturn = _ordemservicoService.Atualizar(os);

                if (osReturn.ValidationResult.IsValid)
                {
                    _logService.Logar(OrdemServico, ordemServicoViewModel.Usuario.CPF, Domain.Enums.Operacao.Alteração.ToString());
                    Commit();
                }

                ordemServicoViewModel = Mapper.Map<OrdemServico, OrdemServicoViewModel>(osReturn);
            }

            return ordemServicoViewModel;
        }

        public List<GrupoAtividadeViewModel> PreencheListGrupoAtividade(List<int> identificadores)
        {
            var lista = new List<GrupoAtividadeViewModel>();
            if (identificadores != null)
                identificadores.ForEach(x =>
                {
                    var item = _grupoAtividadeService.ObterPorId(x);
                    lista.Add(Mapper.Map<GrupoAtividade, GrupoAtividadeViewModel>(item));
                });
            return lista;
        }

        public List<CriterioAvaliacaoViewModel> PreencheListCriterioAvaliacao(List<int> identificadores)
        {
            var lista = new List<CriterioAvaliacaoViewModel>();
            if (identificadores != null)
                identificadores.ForEach(x =>
                {
                    var item = _criterioAvaliacaoService.ObterPorId(x);
                    lista.Add(Mapper.Map<CriterioAvaliacao, CriterioAvaliacaoViewModel>(item));
                });
            return lista;
        }

        public IEnumerable<OrdemServicoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<OrdemServico>, IEnumerable<OrdemServicoViewModel>>(_ordemservicoService.ObterTodosAtivos());
        }

        public OrdemServicoViewModel ObterPorId(int id)
        {
            //return Mapper.Map<OrdemServico, OrdemServicoViewModel>(_ordemservicoService.ObterPorId(id));
            return Mapper.Map<OrdemServico, OrdemServicoViewModel>(_ordemservicoService.ObterPorIdInclude(id));
        }

        public OrdemServicoViewModel Remover(OrdemServicoViewModel ordemServicoViewModel)
        {
            var OrdemServico = Mapper.Map<OrdemServicoViewModel, OrdemServico>(ordemServicoViewModel);

            BeginTransaction();
            var grupoatividadeReturn = _ordemservicoService.Remover(OrdemServico);
            _logService.Logar(OrdemServico, ordemServicoViewModel.Usuario.CPF, Domain.Enums.Operacao.Exclusão.ToString());
            Commit();

            return Mapper.Map<OrdemServico, OrdemServicoViewModel>(grupoatividadeReturn); 
        }
               

        public OrdemServicoViewModel GetOrdemVigente()
        {
            return Mapper.Map<OrdemServico, OrdemServicoViewModel>(
                 //_ordemservicoService.ObterTodos().Where(x => x.DatInicioSistema <= DateTime.Now).OrderByDescending(a => a.DatInicioSistema).OrderByDescending(a => a.IdOrdemServico).FirstOrDefault()
                _ordemservicoService.ObterOrdemVigente()
                );
        }
    }
}

