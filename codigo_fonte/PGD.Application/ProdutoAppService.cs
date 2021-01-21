using PGD.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using AutoMapper;
using PGD.Domain.Entities;

namespace PGD.Application
{
    public class ProdutoAppService : ApplicationService, IProdutoAppService
    {
        private readonly ILogService _logService;
        private readonly IProdutoService _produtoService;
        private readonly IUsuarioService _usuarioService;

        public ProdutoAppService(IUsuarioService usuarioService, IUnitOfWork uow, IProdutoService produtoService, ILogService logService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _produtoService = produtoService;
            _logService = logService;
        }

        public ProdutoViewModel Adicionar(ProdutoViewModel produtoViewModel)
        {
            var produto = Mapper.Map<ProdutoViewModel, Produto>(produtoViewModel);

            BeginTransaction();

            var produtoReturn = _produtoService.Adicionar(produto);

            Commit();

            produtoViewModel = Mapper.Map<Produto, ProdutoViewModel>(produtoReturn);
            return produtoViewModel;
        }

        public ProdutoViewModel Atualizar(ProdutoViewModel produtoViewModel)
        {
            var produto = Mapper.Map<ProdutoViewModel, Produto>(produtoViewModel);

            BeginTransaction();

            var produtoReturn = _produtoService.Atualizar(produto);

            Commit();

            produtoViewModel = Mapper.Map<Produto, ProdutoViewModel>(produtoReturn);
            return produtoViewModel;
        }

        public ProdutoViewModel BuscarPorId(int idpacto, int idproduto)
        {
            return Mapper.Map<Produto, ProdutoViewModel>(_produtoService.BuscarPorId(idpacto, idproduto));
        }

        public IEnumerable<ProdutoViewModel> ObterTodos(ProdutoViewModel objFiltro)
        {
            var produto = Mapper.Map<ProdutoViewModel, Produto>(objFiltro);
            return Mapper.Map<IEnumerable<Produto>, IEnumerable<ProdutoViewModel>>(_produtoService.ObterTodos(produto));
        }

        public IEnumerable<ProdutoViewModel> ObterTodos(int idpacto)
        {
            return Mapper.Map<IEnumerable<Produto>, IEnumerable<ProdutoViewModel>>(_produtoService.ObterTodos(idpacto));
        }

        public ProdutoViewModel Remover(ProdutoViewModel produtoViewModel)
        {
            var produto = Mapper.Map<ProdutoViewModel, Produto>(produtoViewModel);

            BeginTransaction();

            var produtoReturn = _produtoService.Remover(produto);

            Commit();

            produtoViewModel = Mapper.Map<Produto, ProdutoViewModel>(produtoReturn);
            return produtoViewModel;
        }
    }
}
