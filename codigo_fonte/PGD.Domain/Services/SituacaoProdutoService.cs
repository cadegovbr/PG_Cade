using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;

namespace PGD.Domain.Services
{
    public class SituacaoProdutoService : ISituacaoProdutoService
    {
        private readonly ISituacaoProdutoRepository _classRepository;

        public SituacaoProdutoService(ISituacaoProdutoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public SituacaoProduto Adicionar(SituacaoProduto obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public SituacaoProduto Atualizar(SituacaoProduto obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public SituacaoProduto ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<SituacaoProduto> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public SituacaoProduto Remover(SituacaoProduto obj)
        {
            _classRepository.Remover(obj.IdSituacaoProduto);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }
    }
}
