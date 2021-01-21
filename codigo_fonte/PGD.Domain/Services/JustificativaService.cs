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
    public class JustificativaService : IJustificativaService
    {
        private readonly IJustificativaRepository _classRepository;

        public JustificativaService(IJustificativaRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public Justificativa Adicionar(Justificativa obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public Justificativa Atualizar(Justificativa obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public Justificativa ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Justificativa> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public Justificativa Remover(Justificativa obj)
        {
            _classRepository.Remover(obj.IdJustificativa);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
