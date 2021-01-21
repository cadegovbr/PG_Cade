using PGD.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using AutoMapper;
using PGD.Domain.Entities;
using DomainValidation.Validation;
using PGD.Domain.Validations.Atividades;
using PGD.Domain.Validations.ItensAvaliacao;

namespace PGD.Application
{
    public class ItemAvaliacaoAppService : ApplicationService, IItemAvaliacaoAppService
    {
        private readonly ILogService _logService;
        private readonly IItemAvaliacaoService _itemAvaliacaoService;        
        private readonly IUsuarioService _usuarioService;

        public ItemAvaliacaoAppService(IUsuarioService usuarioService, IUnitOfWork uow, IItemAvaliacaoService itemAvaliacaoService, ILogService logService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _itemAvaliacaoService = itemAvaliacaoService;
            _logService = logService;            
        }

        public ItemAvaliacaoViewModel Adicionar(ItemAvaliacaoViewModel itemAvaliacaoViewModel)
        {            
            var itemAvaliacao = Mapper.Map<ItemAvaliacaoViewModel, ItemAvaliacao>(itemAvaliacaoViewModel);

            BeginTransaction();

            var itemAvaliacaoReturn = _itemAvaliacaoService.Adicionar(itemAvaliacao);
            if (itemAvaliacaoReturn.ValidationResult.IsValid)
            {
                _logService.Logar(itemAvaliacao, itemAvaliacaoViewModel.Usuario.CPF, Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }
            itemAvaliacaoViewModel = Mapper.Map<ItemAvaliacao, ItemAvaliacaoViewModel>(itemAvaliacaoReturn);
            return itemAvaliacaoViewModel;
        }

        public ItemAvaliacaoViewModel Atualizar(ItemAvaliacaoViewModel itemAvaliacaoViewModel)
        {
            var itemAvaliacao = Mapper.Map<ItemAvaliacaoViewModel, ItemAvaliacao>(itemAvaliacaoViewModel);

            itemAvaliacao.ValidationResult = new ItemAvaliacaoValidation().Validate(itemAvaliacao);

            if (!itemAvaliacao.ValidationResult.IsValid)
            {
                itemAvaliacaoViewModel.ValidationResult = itemAvaliacao.ValidationResult;
                return itemAvaliacaoViewModel;
            }

            BeginTransaction();

            //itemAvaliacao = new Atividade { IdAtividade = itemAvaliacaoViewModel.IdAtividade, NomAtividade = itemAvaliacaoViewModel.NomAtividade, PctMinimoReducao = itemAvaliacaoViewModel.PctMinimoReducao, Tipos = new List<TipoAtividade>() };
            //TipoAtividade tipoAtividade = ConfigurarTipoAtividade(itemAvaliacaoViewModel, itemAvaliacao);
            ////Se ocorreu erro no tipo atividade, retornar
            //if (!tipoAtividade.ValidationResult.IsValid)
            //{
            //    itemAvaliacaoViewModel.ValidationResult = tipoAtividade.ValidationResult;
            //    return itemAvaliacaoViewModel;
            //}

            //var atividadeReturn = _itemAvaliacaoService.Atualizar(itemAvaliacao);

            //if (atividadeReturn.ValidationResult.IsValid)
            //{
            //    _logService.Logar(itemAvaliacao, itemAvaliacaoViewModel.Usuario.Cpf, Domain.Enums.Operacao.Alteração.ToString());
            //    Commit();
            //}
            //itemAvaliacaoViewModel = Mapper.Map<Atividade, AtividadeViewModel>(atividadeReturn);
            return itemAvaliacaoViewModel;
        }
                
        public IEnumerable<ItemAvaliacaoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<ItemAvaliacao>, IEnumerable<ItemAvaliacaoViewModel>>(_itemAvaliacaoService.ObterTodosAtivos());
        }

        public ItemAvaliacaoViewModel ObterPorId(int id)
        {
            return Mapper.Map<ItemAvaliacao, ItemAvaliacaoViewModel>(_itemAvaliacaoService.ObterPorId(id));
        }

        public ItemAvaliacaoViewModel Remover(ItemAvaliacaoViewModel itemAvaliacaoViewModel)
        {
            var itemAvaliacao = Mapper.Map<ItemAvaliacaoViewModel, ItemAvaliacao>(itemAvaliacaoViewModel);

            BeginTransaction();
            var itemAvaliacaoReturn = _itemAvaliacaoService.Remover(itemAvaliacao);
            _logService.Logar(itemAvaliacao, itemAvaliacaoViewModel.Usuario.CPF, Domain.Enums.Operacao.Exclusão.ToString());
            Commit();

            return Mapper.Map<ItemAvaliacao, ItemAvaliacaoViewModel>(itemAvaliacaoReturn); ;
        }
    }
}
