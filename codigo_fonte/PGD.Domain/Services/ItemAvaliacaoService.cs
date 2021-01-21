using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.ItensAvaliacao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Domain.Services
{
    public class ItemAvaliacaoService : IItemAvaliacaoService
    {
        private readonly IItemAvaliacaoRepository _classRepository;

        public ItemAvaliacaoService(IItemAvaliacaoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public ItemAvaliacao Adicionar(ItemAvaliacao obj)
        {
            try
            {
                if (!obj.IsValid())
                {
                    return obj;
                }

                obj.ValidationResult = new ItemAvaliacaoValidation().Validate(obj);
                if (!obj.ValidationResult.IsValid)
                {
                    return obj;
                }

                obj.ValidationResult.Message = Mensagens.MS_003;
                return _classRepository.AdicionarSave(obj);
            }
            catch (Exception ex)
            {
                obj.ValidationResult.Add(new DomainValidation.Validation.ValidationError(ex.Message));
                return obj;
            }
        }

        public ItemAvaliacao Atualizar(ItemAvaliacao obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new ItemAvaliacaoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public ItemAvaliacao ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<ItemAvaliacao> ObterTodos()
        {
            return _classRepository.ObterTodos().OrderBy(a=>a.DescItemAvaliacao);
        }

        public IEnumerable<ItemAvaliacao> ObterTodosAtivos()
        {
            return _classRepository.Buscar(a => !a.Inativo).OrderBy(a => a.DescItemAvaliacao);
        }

        public ItemAvaliacao Remover(ItemAvaliacao obj)
        {
            var itemAvaliacao = ObterPorId(obj.IdItemAvaliacao);
            itemAvaliacao.Inativo = true;
            _classRepository.Atualizar(itemAvaliacao);
            itemAvaliacao.ValidationResult.Message = Mensagens.MS_005;
            return itemAvaliacao;
        }
    }
}
