using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Domain.Services
{
    public class NotaAvaliacaoService : INotaAvaliacaoService
    {
        private readonly INotaAvaliacaoRepository _classRepository;

        public NotaAvaliacaoService(INotaAvaliacaoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public NotaAvaliacao Adicionar(NotaAvaliacao obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public NotaAvaliacao Atualizar(NotaAvaliacao obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public NotaAvaliacao ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<NotaAvaliacao> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public IEnumerable<NotaAvaliacao> ObterTodosPorNivelAvaliacao(int idNivelAvaliacao)
        {
            return _classRepository.ObterTodosPorNivelAvaliacao(idNivelAvaliacao).OrderByDescending(a => a.LimiteSuperiorFaixa);
        }

        public NotaAvaliacao Remover(NotaAvaliacao obj)
        {
            _classRepository.Remover(obj.IdNotaAvaliacao);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
