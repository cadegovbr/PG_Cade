using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using System.Collections.Generic;

namespace PGD.Domain.Services
{
    public class NivelAvaliacaoService : INivelAvaliacaoService
    {
        private readonly INivelAvaliacaoRepository _classRepository;

        public NivelAvaliacaoService(INivelAvaliacaoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public NivelAvaliacao Adicionar(NivelAvaliacao obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public NivelAvaliacao Atualizar(NivelAvaliacao obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public NivelAvaliacao ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<NivelAvaliacao> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public NivelAvaliacao Remover(NivelAvaliacao obj)
        {
            _classRepository.Remover(obj.IdNivelAvaliacao);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
