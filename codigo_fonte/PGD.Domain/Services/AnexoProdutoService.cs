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
    public class AnexoProdutoService : IAnexoProdutoService
    {
        private readonly IAnexoProdutoRepository _classRepository;
        public AnexoProduto Adicionar(AnexoProduto obj)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();
            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public AnexoProduto Atualizar(AnexoProduto obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public AnexoProduto AtualizarIdAnexoProduto(AnexoProduto obj, int idAnexoProduto)
        {
            throw new NotImplementedException();
        }

        public AnexoProduto BuscarPorId(int idProduto, int idAnexoProduto)
        {
            throw new NotImplementedException();
        }

        public AnexoProduto ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AnexoProduto> ObterTodos(AnexoProduto objFiltro)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AnexoProduto> ObterTodos(int idProduto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AnexoProduto> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public AnexoProduto Remover(AnexoProduto obj)
        {
            throw new NotImplementedException();
        }
    }
}
