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

namespace PGD.Application
{
    public class AtividadeAppService : ApplicationService, IAtividadeAppService
    {
        private readonly ILogService _logService;
        private readonly IAtividadeService _atividadeService;
        private readonly ITipoAtividadeService _tipoatividadeService;
        private readonly IUsuarioService _usuarioService;

        public AtividadeAppService(IUsuarioService usuarioService, IUnitOfWork uow, IAtividadeService atividadeService, ILogService logService, ITipoAtividadeService tipoatividadeService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _atividadeService = atividadeService;
            _logService = logService;
            _tipoatividadeService = tipoatividadeService;
        }

        public AtividadeViewModel Adicionar(AtividadeViewModel atividadeViewModel)
        {
            for (var i = atividadeViewModel.Tipos.Count - 1; i >= 0; i--)
                if (atividadeViewModel.Tipos[i].Excluir)
                    atividadeViewModel.Tipos.RemoveAt(i);

            var atividade = Mapper.Map<AtividadeViewModel, Atividade>(atividadeViewModel);

            BeginTransaction();

            var atividadeReturn = _atividadeService.Adicionar(atividade);
            if (atividadeReturn.ValidationResult.IsValid)
            {
                _logService.Logar(atividade, atividadeViewModel.Usuario.CPF, Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }
            atividadeViewModel = Mapper.Map<Atividade, AtividadeViewModel>(atividadeReturn);
            return atividadeViewModel;
        }

        public AtividadeViewModel Atualizar(AtividadeViewModel atividadeViewModel)
        {
            TratarTiposAtividadeExcluidos(atividadeViewModel);

            var atividade = Mapper.Map<AtividadeViewModel, Atividade>(atividadeViewModel);

            atividade.ValidationResult = new AtividadeValidation().Validate(atividade);
            if (!atividade.ValidationResult.IsValid)
            {
                atividadeViewModel.ValidationResult = atividade.ValidationResult;
                return atividadeViewModel;
            }

            BeginTransaction();

            atividade = new Atividade { IdAtividade = atividadeViewModel.IdAtividade, NomAtividade = atividadeViewModel.NomAtividade, PctMinimoReducao = atividadeViewModel.PctMinimoReducao, Link = atividadeViewModel.Link, Tipos = new List<TipoAtividade>() };
            TipoAtividade tipoAtividade = ConfigurarTipoAtividade(atividadeViewModel, atividade);
            //Se ocorreu erro no tipo atividade, retornar
            if (!tipoAtividade.ValidationResult.IsValid)
            {
                atividadeViewModel.ValidationResult = tipoAtividade.ValidationResult;
                return atividadeViewModel;
            }

            var atividadeReturn = _atividadeService.Atualizar(atividade);

            if (atividadeReturn.ValidationResult.IsValid)
            {
                _logService.Logar(atividade, atividadeViewModel.Usuario.CPF, Domain.Enums.Operacao.Alteração.ToString());
                Commit();
            }
            atividadeViewModel = Mapper.Map<Atividade, AtividadeViewModel>(atividadeReturn);
            return atividadeViewModel;
        }

        private TipoAtividade ConfigurarTipoAtividade(AtividadeViewModel atividadeViewModel, Atividade atividade)
        {
            TipoAtividade tipoAtividade = new TipoAtividade();
            foreach (var tipo in atividadeViewModel.Tipos)
            {
                if (tipo.Excluir)
                {
                    tipoAtividade = _tipoatividadeService.ObterPorId(tipo.IdTipoAtividade);
                    _tipoatividadeService.Remover(tipoAtividade);
                }
                else
                {
                    tipoAtividade = Mapper.Map<TipoAtividadeViewModel, TipoAtividade>(tipo);
                    tipoAtividade.IdAtividade = atividadeViewModel.IdAtividade;
                    if (tipoAtividade.Faixa == null)
                        tipoAtividade.Faixa = "";
                    if (tipo.IdTipoAtividade == 0)
                        _tipoatividadeService.Adicionar(tipoAtividade);
                    else
                        _tipoatividadeService.Atualizar(tipoAtividade);

                    if (!tipoAtividade.ValidationResult.IsValid)
                        break;

                    atividade.Tipos.Add(tipoAtividade);
                }
            }

            return tipoAtividade;
        }

        private static void TratarTiposAtividadeExcluidos(AtividadeViewModel atividadeViewModel)
        {
            for (var i = atividadeViewModel.Tipos.Count - 1; i >= 0; i--)
                if (atividadeViewModel.Tipos[i].Excluir && atividadeViewModel.Tipos[i].IdTipoAtividade == 0)
                    atividadeViewModel.Tipos.RemoveAt(i);
        }

        public IEnumerable<AtividadeViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<Atividade>, IEnumerable<AtividadeViewModel>>(_atividadeService.ObterTodosAtivos());
        }

        public AtividadeViewModel ObterPorId(int id)
        {
            return Mapper.Map<Atividade, AtividadeViewModel>(_atividadeService.ObterPorId(id));
        }

        public AtividadeViewModel Remover(AtividadeViewModel atividadeViewModel)
        {
            var atividade = Mapper.Map<AtividadeViewModel, Atividade>(atividadeViewModel);

            BeginTransaction();
            var atividadeReturn = _atividadeService.Remover(atividade);
            _logService.Logar(atividade, atividadeViewModel.Usuario.CPF, Domain.Enums.Operacao.Exclusão.ToString());
            Commit();

            return Mapper.Map<Atividade, AtividadeViewModel>(atividadeReturn); ;
        }
    }
}
