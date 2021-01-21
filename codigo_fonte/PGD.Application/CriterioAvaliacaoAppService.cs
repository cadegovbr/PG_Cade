using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.CriteriosAvaliacao;
using PGD.Infra.Data.Interfaces;
using System.Collections.Generic;

namespace PGD.Application
{
    public class CriterioAvaliacaoAppService : ApplicationService, ICriterioAvaliacaoAppService
    {
        private readonly ILogService _logService;
        private readonly IItemAvaliacaoService _itemAvaliacaoService;
        private readonly ICriterioAvaliacaoService _criterioAvaliacaoService;
        private readonly IUsuarioService _usuarioService;

        public CriterioAvaliacaoAppService(IUsuarioService usuarioService, IUnitOfWork uow, IItemAvaliacaoService itemAvaliacaoService, ILogService logService, ICriterioAvaliacaoService criterioAvaliacaoService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _itemAvaliacaoService = itemAvaliacaoService;
            _logService = logService;
            _criterioAvaliacaoService = criterioAvaliacaoService;
        }

        public CriterioAvaliacaoViewModel Adicionar(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel)
        {
            for (var i = criterioAvaliacaoViewModel.ItensAvaliacao.Count - 1; i >= 0; i--)
            {
                if (criterioAvaliacaoViewModel.ItensAvaliacao[i].Excluir)
                    criterioAvaliacaoViewModel.ItensAvaliacao.RemoveAt(i);
            }

            var criterioAvaliacao = Mapper.Map<CriterioAvaliacaoViewModel, CriterioAvaliacao>(criterioAvaliacaoViewModel);

            BeginTransaction();

            var criterioAvaliacaoReturn = _criterioAvaliacaoService.Adicionar(criterioAvaliacao);
            if (criterioAvaliacaoReturn.ValidationResult.IsValid)
            {
                _logService.Logar(criterioAvaliacao, criterioAvaliacaoViewModel.Usuario.CPF, Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }
            criterioAvaliacaoViewModel = Mapper.Map<CriterioAvaliacao, CriterioAvaliacaoViewModel>(criterioAvaliacaoReturn);
            return criterioAvaliacaoViewModel;
        }

        public CriterioAvaliacaoViewModel Atualizar(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel)
        {
            TratarItensAvaliacaoExcluidos(criterioAvaliacaoViewModel);
            
            var criterioAvaliacao = Mapper.Map<CriterioAvaliacaoViewModel, CriterioAvaliacao>(criterioAvaliacaoViewModel);

            criterioAvaliacao.ValidationResult = new CriterioAvaliacaoValidation().Validate(criterioAvaliacao);
            if (!criterioAvaliacao.ValidationResult.IsValid)
            {
                criterioAvaliacaoViewModel.ValidationResult = criterioAvaliacao.ValidationResult;
                return criterioAvaliacaoViewModel;
            }

            BeginTransaction();

            criterioAvaliacao = new CriterioAvaliacao { IdCriterioAvaliacao = criterioAvaliacaoViewModel.IdCriterioAvaliacao, DescCriterioAvaliacao = criterioAvaliacaoViewModel.DescCriterioAvaliacao, StrTextoExplicativo = criterioAvaliacaoViewModel.StrTextoExplicativo, ItensAvaliacao = new List<ItemAvaliacao>() };
            ItemAvaliacao itemAvaliacao = ConfigurarItemAvaliacao(criterioAvaliacaoViewModel, criterioAvaliacao);
            //Se ocorreu erro no tipo atividade, retornar
            if (!itemAvaliacao.ValidationResult.IsValid)
            {
                criterioAvaliacaoViewModel.ValidationResult = itemAvaliacao.ValidationResult;
                return criterioAvaliacaoViewModel;
            }

            var criterioAvaliacaoReturn = _criterioAvaliacaoService.Atualizar(criterioAvaliacao);

            if (criterioAvaliacaoReturn.ValidationResult.IsValid)
            {
                _logService.Logar(criterioAvaliacao, criterioAvaliacaoViewModel.Usuario.CPF, Domain.Enums.Operacao.Alteração.ToString());
                Commit();
            }

            criterioAvaliacaoViewModel = Mapper.Map<CriterioAvaliacao, CriterioAvaliacaoViewModel>(criterioAvaliacaoReturn);
            
            return criterioAvaliacaoViewModel;
        }

        private static void TratarItensAvaliacaoExcluidos(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel)
        {
            for (var i = criterioAvaliacaoViewModel.ItensAvaliacao.Count - 1; i >= 0; i--)
                if (criterioAvaliacaoViewModel.ItensAvaliacao[i].Excluir && criterioAvaliacaoViewModel.ItensAvaliacao[i].IdItemAvaliacao == 0)
                    criterioAvaliacaoViewModel.ItensAvaliacao.RemoveAt(i);
        }

        private ItemAvaliacao ConfigurarItemAvaliacao(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel, CriterioAvaliacao criterioAvaliacao)
        {
            ItemAvaliacao itemAvaliacao = new ItemAvaliacao();

            foreach (var itemAva in criterioAvaliacaoViewModel.ItensAvaliacao)
            {
                if (itemAva.Excluir)
                {
                    itemAvaliacao = _itemAvaliacaoService.ObterPorId(itemAva.IdItemAvaliacao);
                    _itemAvaliacaoService.Remover(itemAvaliacao);
                }
                else
                {
                    itemAvaliacao = Mapper.Map<ItemAvaliacaoViewModel, ItemAvaliacao>(itemAva);
                    itemAvaliacao.IdCriterioAvaliacao = criterioAvaliacaoViewModel.IdCriterioAvaliacao;
                    
                    if (itemAvaliacao.IdItemAvaliacao == 0)
                        _itemAvaliacaoService.Adicionar(itemAvaliacao);
                    else
                        _itemAvaliacaoService.Atualizar(itemAvaliacao);

                    if (!itemAvaliacao.ValidationResult.IsValid)
                        break;

                    criterioAvaliacao.ItensAvaliacao.Add(itemAvaliacao);
                }
            }

            return itemAvaliacao;
        }

        public List<ItemAvaliacaoViewModel> PreencheList(List<int> identificadores)
        {
            var lista = new List<ItemAvaliacaoViewModel>();
            if (identificadores != null)
                identificadores.ForEach(x =>
                {
                    var item = _itemAvaliacaoService.ObterPorId(x);
                    lista.Add(Mapper.Map<ItemAvaliacao, ItemAvaliacaoViewModel>(item));
                });
            return lista;
        }

        public IEnumerable<CriterioAvaliacaoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<CriterioAvaliacao>, IEnumerable<CriterioAvaliacaoViewModel>>(_criterioAvaliacaoService.ObterTodosAtivos());
        }

        public CriterioAvaliacaoViewModel ObterPorId(int id)
        {
            return Mapper.Map<CriterioAvaliacao, CriterioAvaliacaoViewModel>(_criterioAvaliacaoService.ObterPorId(id));
        }

        public CriterioAvaliacaoViewModel Remover(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel)
        {
            var criterioAvaliacao = Mapper.Map<CriterioAvaliacaoViewModel, CriterioAvaliacao>(criterioAvaliacaoViewModel);

            BeginTransaction();
            var criterioAvaliacaoReturn = _criterioAvaliacaoService.Remover(criterioAvaliacao);
            _logService.Logar(criterioAvaliacao, criterioAvaliacaoViewModel.Usuario.CPF, Domain.Enums.Operacao.Exclusão.ToString());
            Commit();

            return Mapper.Map<CriterioAvaliacao, CriterioAvaliacaoViewModel>(criterioAvaliacaoReturn); ;
        }
    }
}
