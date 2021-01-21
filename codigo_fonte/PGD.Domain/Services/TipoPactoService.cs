using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using System.Collections.Generic;

namespace PGD.Domain.Services
{
    public class TipoPactoService : ITipoPactoService
    {
        private readonly ITipoPactoRepository _classRepository;

        public TipoPactoService(ITipoPactoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public TipoPacto Adicionar(TipoPacto obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public TipoPacto Atualizar(TipoPacto obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public TipoPacto ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<TipoPacto> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public TipoPacto Remover(TipoPacto obj)
        {
            _classRepository.Remover(obj.IdTipoPacto);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
