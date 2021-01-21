using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class ProdutoTests
    {
        private IProdutoService _produtoService;
        private IPactoService _pactoService;
        private IOrdemServicoService _osService;
        private IUnitOfWork _uow;
        public static StandardKernel kernel = new StandardKernel();

        [OneTimeSetUp]
        public void Initialize()
        {
            var dbInicio = new DbTests();
            try
            {
                dbInicio.Limpa_E_Cria_Database();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Banco de dados Limpo e atualizado");

            BootStrapper.RegisterServicesSingleton(kernel);
            _uow = kernel.Get<IUnitOfWork>();
            _pactoService = kernel.Get<IPactoService>();
            _osService = kernel.Get<IOrdemServicoService>();
            _produtoService = kernel.Get<IProdutoService>();

        }

        int itemId = 0;
        int pactoId = 0;

        [Order(0), TestCase(TestName = "Produto Insert")]
        public void TestInsert()
        {
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                Nome = "Produto Teste da Silva",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = "11111111111",
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = "11111111888", 
                IdTipoPacto = 1
            };
            _uow.BeginTransaction();
            var resultadoPct = _pactoService.Adicionar(pacto);
            _uow.Commit();

            pactoId = resultadoPct.IdPacto;

            var produto = new Produto
            {
                IdAtividade = 1,
                IdGrupoAtividade = 1,
                IdPacto = pactoId,
                IdTipoAtividade = 1,
                CargaHoraria = 8,
                QuantidadeProduto = 3,
                CargaHorariaProduto = 30,
                Avaliacao = 0
            };
            _uow.BeginTransaction();
            var resultado = _produtoService.Adicionar(produto);
            _uow.Commit();
            itemId = produto.IdProduto;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "Produto Retrieve")]
        public void TestRetrieveId()
        {
            var produto = _produtoService.ObterPorId(itemId);
            Assert.IsNotNull(produto);
            Assert.AreEqual(produto.IdProduto, itemId);
        }

        [Order(2), TestCase(TestName = "Produto Update")]
        public void TestUpdate()
        {
            var produto = _produtoService.ObterPorId(itemId);
            produto.Avaliacao = 1;
            produto.ObservacoesAdicionais = "teste update produto";
            var resultado = _produtoService.AtualizarIdProduto(produto, produto.IdProduto);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "Produto Delete")]
        public void TestDelete()
        {
            var produto = _produtoService.ObterPorId(itemId);
            var resultado = _produtoService.Remover(produto);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
