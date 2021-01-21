using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;

namespace PGD.Domain.Services
{
    public class ProdutoService : IProdutoService
    {

        private readonly IProdutoRepository _classRepository;

        public ProdutoService(IProdutoRepository classRepository)
        {
            _classRepository = classRepository;
        }
        public Produto Adicionar(Produto obj)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();
            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);

        }

        public Produto Atualizar(Produto obj)
        {
            return _classRepository.Atualizar(obj);
        }
        public Produto AtualizarIdProduto(Produto obj, int idProduto)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();
            obj.ValidationResult.Message = Mensagens.MS_004;
            return _classRepository.Atualizar(obj, idProduto);

        }
        public Produto ObterPorId(int id)
        {
            return this._classRepository.Buscar(x => x.IdProduto == id).FirstOrDefault();
        }

        public IEnumerable<Produto> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Produto> ObterTodos(int idpacto)
        {
            return this._classRepository.Buscar(x => x.IdPacto == idpacto).ToList();
        }

        public IEnumerable<Produto> ObterTodos(Produto objFiltro)
        {
            return this._classRepository.Buscar(x => x.IdPacto == objFiltro.IdPacto && x.IdProduto == objFiltro.IdProduto).ToList();
        }

        public Produto Remover(Produto obj)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();

            obj.ValidationResult.Message = Mensagens.MS_005;
            _classRepository.Remover(obj.IdProduto);
            return obj; 
        }

        public Produto BuscarPorId(int idpacto, int idproduto)
        {
            return this._classRepository.BuscarPorId(idpacto, idproduto);
        }

        public Produto AtualizarObservacaoProduto(int idProduto, string observacoes)
        {
            Produto produto = ObterPorId(idProduto);
            produto.Observacoes = observacoes;

            _classRepository.SaveChanges();
            return produto;
        }

        public Produto AtualizarSituacaoProduto (int idProduto, int idSituacaoProduto)
        {
            Produto produto = ObterPorId(idProduto);
            produto.IdSituacaoProduto = idSituacaoProduto;

            _classRepository.SaveChanges();
            return produto;
        }
    }
}
