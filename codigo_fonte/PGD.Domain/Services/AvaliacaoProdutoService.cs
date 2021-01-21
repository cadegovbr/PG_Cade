using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.Adminstrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Services
{
    public class AvaliacaoProdutoService : IAvaliacaoProdutoService
    {
        private readonly IAvaliacaoProdutoRepository _classRepository;
        private readonly IAvaliacaoDetalhadaProdutoRepository _classAvaliacaoDetalhadaRepository;

        public AvaliacaoProdutoService(IAvaliacaoProdutoRepository classRepository, IAvaliacaoDetalhadaProdutoRepository classAvaliacaoDetalhadaRepository)
        {
            _classRepository = classRepository;
            _classAvaliacaoDetalhadaRepository = classAvaliacaoDetalhadaRepository;
        }

        AvaliacaoProduto IService<AvaliacaoProduto>.Adicionar(AvaliacaoProduto obj)
        {
            throw new NotImplementedException();
        }

        AvaliacaoProduto IService<AvaliacaoProduto>.Atualizar(AvaliacaoProduto obj)
        {
            throw new NotImplementedException();
        }

        AvaliacaoProduto IService<AvaliacaoProduto>.ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        IEnumerable<AvaliacaoProduto> IAvaliacaoProdutoService.ObterAvaliacoesPorProduto(int idProduto)
        {   
            var lista = _classRepository.Buscar(a => a.IdProduto == idProduto);
            return lista;
        }

        IEnumerable<AvaliacaoProduto> IService<AvaliacaoProduto>.ObterTodos()
        {
            return this._classRepository.ObterTodos();
        }

        AvaliacaoProduto IService<AvaliacaoProduto>.Remover(AvaliacaoProduto obj)
        {
            obj.AvaliacoesDetalhadas.ForEach(ad => _classAvaliacaoDetalhadaRepository.Remover(ad.IdAvaliacaoDetalhadaProduto));
            obj.AvaliacoesDetalhadas.Clear();
            _classRepository.Remover(obj.IdAvaliacaoProduto);
            
            return obj;
        }
    }
}
