using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;
using PGD.Domain.Validations.GruposAtividade;

namespace PGD.Domain.Services
{
    public class OS_GrupoAtividadeService : IOS_GrupoAtividadeService
    {
        private readonly IOS_GrupoAtividadeRepository _classRepository;

        public OS_GrupoAtividadeService(IOS_GrupoAtividadeRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public OS_GrupoAtividade Adicionar(OS_GrupoAtividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OS_GrupoAtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public OS_GrupoAtividade Atualizar(OS_GrupoAtividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OS_GrupoAtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public OS_GrupoAtividade ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<OS_GrupoAtividade> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public IEnumerable<OS_GrupoAtividade> ObterTodosAtivos()
        {
            return _classRepository.Buscar(a => a.Inativo == false);
        }

        public OS_GrupoAtividade Remover(OS_GrupoAtividade obj)
        {
            var OS_GrupoAtividade = ObterPorId(obj.IdGrupoAtividade);
            OS_GrupoAtividade.Inativo = true;
            _classRepository.Atualizar(OS_GrupoAtividade);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
