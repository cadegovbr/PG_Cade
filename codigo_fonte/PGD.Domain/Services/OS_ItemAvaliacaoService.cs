using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.CriteriosAvaliacao;
using System.Collections.Generic;

namespace PGD.Domain.Services
{
    public class OS_ItemAvaliacaoService : IOS_ItemAvaliacaoService
    {
        private readonly IOS_ItemAvaliacaoRepository _classRepository;

        public OS_ItemAvaliacaoService(IOS_ItemAvaliacaoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public OS_ItemAvaliacao Adicionar(OS_ItemAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OS_ItemAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public OS_ItemAvaliacao Atualizar(OS_ItemAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OS_ItemAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public OS_ItemAvaliacao ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<OS_ItemAvaliacao> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public OS_ItemAvaliacao Remover(OS_ItemAvaliacao obj)
        {
            var oS_ItemAvaliacao = ObterPorId(obj.IdItemAvaliacao);            
            _classRepository.Atualizar(oS_ItemAvaliacao);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
