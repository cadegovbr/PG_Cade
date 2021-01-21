using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.CriteriosAvaliacao;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Domain.Services
{
    public class CriterioAvaliacaoService : ICriterioAvaliacaoService
    {
        private readonly ICriterioAvaliacaoRepository _classRepository;

        public CriterioAvaliacaoService(ICriterioAvaliacaoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public CriterioAvaliacao Adicionar(CriterioAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new CriterioAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.AdicionarSave(obj);
        }

        public CriterioAvaliacao Atualizar(CriterioAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new CriterioAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public CriterioAvaliacao ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<CriterioAvaliacao> ObterTodos()
        {
            return _classRepository.ObterTodos().OrderBy(a => a.DescCriterioAvaliacao);
        }

        public IEnumerable<CriterioAvaliacao> ObterTodosAtivos()
        {
            var lista =_classRepository.Buscar(a => !a.Inativo).OrderBy(a=>a.DescCriterioAvaliacao);
            return lista;
        }

        public CriterioAvaliacao Remover(CriterioAvaliacao obj)
        {
            var criterioAvaliacao = ObterPorId(obj.IdCriterioAvaliacao);
            criterioAvaliacao.Inativo = true;
            _classRepository.Atualizar(criterioAvaliacao);
            criterioAvaliacao.ValidationResult.Message = Mensagens.MS_005;
            return criterioAvaliacao;
        }
    }
}
