using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;
using PGD.Domain.Validations.TiposAtividade;

namespace PGD.Domain.Services
{
    public class TipoAtividadeService : ITipoAtividadeService
    {
        private readonly ITipoAtividadeRepository _classRepository;

        public TipoAtividadeService(ITipoAtividadeRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public TipoAtividade Adicionar(TipoAtividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new TipoAtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public TipoAtividade Atualizar(TipoAtividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new TipoAtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public TipoAtividade ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<TipoAtividade> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public TipoAtividade Remover(TipoAtividade obj)
        {
            _classRepository.Remover(obj.IdTipoAtividade);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
