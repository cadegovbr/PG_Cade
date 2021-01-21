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
    public class GrupoAtividadeService : IGrupoAtividadeService
    {
        private readonly IGrupoAtividadeRepository _classRepository;

        public GrupoAtividadeService(IGrupoAtividadeRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public GrupoAtividade Adicionar(GrupoAtividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new GrupoAtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.AdicionarSave(obj);
        }

        public GrupoAtividade Atualizar(GrupoAtividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new GrupoAtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public GrupoAtividade ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<GrupoAtividade> ObterTodos()
        {
            return _classRepository.ObterTodos().OrderBy(a => a.NomGrupoAtividade);
        }

        public IEnumerable<GrupoAtividade> ObterTodosAtivos()
        {
            var lista =_classRepository.Buscar(a => a.Inativo == false).OrderBy(a=>a.NomGrupoAtividade);
            return lista;
        }

        public GrupoAtividade Remover(GrupoAtividade obj)
        {
            var grupoatividade = ObterPorId(obj.IdGrupoAtividade);
            grupoatividade.Inativo = true;
            _classRepository.Atualizar(grupoatividade);
            grupoatividade.ValidationResult.Message = Mensagens.MS_005;
            return grupoatividade;
        }
    }
}
