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
    public class SituacaoPactoService : ISituacaoPactoService
    {
        private readonly ISituacaoPactoRepository _classRepository;

        public SituacaoPactoService(ISituacaoPactoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public SituacaoPacto Adicionar(SituacaoPacto obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public SituacaoPacto Atualizar(SituacaoPacto obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public SituacaoPacto ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<SituacaoPacto> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public SituacaoPacto Remover(SituacaoPacto obj)
        {
            _classRepository.Remover(obj.IdSituacaoPacto);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
