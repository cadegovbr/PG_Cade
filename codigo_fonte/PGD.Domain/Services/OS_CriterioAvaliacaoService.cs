using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.CriteriosAvaliacao;
using System.Collections.Generic;

namespace PGD.Domain.Services
{
    public class OS_CriterioAvaliacaoService : IOS_CriterioAvaliacaoService
    {
        private readonly IOS_CriterioAvaliacaoRepository _classRepository;

        public OS_CriterioAvaliacaoService(IOS_CriterioAvaliacaoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public OS_CriterioAvaliacao Adicionar(OS_CriterioAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OS_CriterioAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public OS_CriterioAvaliacao Atualizar(OS_CriterioAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OS_CriterioAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public IEnumerable<OS_CriterioAvaliacao> BuscarPorIdOS(int idOS)
        {
            return _classRepository.Buscar(a => a.IdOrdemServico == idOS);
        }

        public OS_CriterioAvaliacao ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<OS_CriterioAvaliacao> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public IEnumerable<OS_CriterioAvaliacao> ObterTodosAtivos()
        {
            return _classRepository.Buscar(a => !a.Inativo);
        }

        public OS_CriterioAvaliacao Remover(OS_CriterioAvaliacao obj)
        {
            var OS_CriterioAvaliacao = ObterPorId(obj.IdCriterioAvaliacao);
            OS_CriterioAvaliacao.Inativo = true;
            _classRepository.Atualizar(OS_CriterioAvaliacao);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
