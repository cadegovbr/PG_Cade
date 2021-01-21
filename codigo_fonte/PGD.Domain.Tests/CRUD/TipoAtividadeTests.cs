using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities;
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
    public class TipoAtividadeTests
    {
        private ITipoAtividadeService _tipoAtividadeService;
        private IAtividadeService _atividadeService;
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

            _atividadeService = kernel.Get<IAtividadeService>();
            _tipoAtividadeService = kernel.Get<ITipoAtividadeService>();

        }
        //CRUD

        int itemId = 0;
        int idAtividadeLc = 0;

        [Order(0), TestCase(TestName = "Tipo Atividade Insert")]
        public void TestInsert()
        {
            var atividade = new Atividade
            {
                NomAtividade = "Atividade Teste Insert Tipo Atividade",
                PctMinimoReducao = 10,
                Tipos = new List<TipoAtividade> { new TipoAtividade { DuracaoFaixa = 15, DuracaoFaixaPresencial = 35, Faixa = "Faixa Teste 1" } }
            };
            _uow.BeginTransaction();
            var resultadoAtv = _atividadeService.Adicionar(atividade);
            _uow.Commit();
            idAtividadeLc = resultadoAtv.IdAtividade;

            var tipoAtividade = new TipoAtividade
            {
                IdAtividade = idAtividadeLc,
                DuracaoFaixa = 15,
                DuracaoFaixaPresencial = 35,
                Faixa = "Faixa Teste Insert"
            };
            _uow.BeginTransaction();
            var resultado = _tipoAtividadeService.Adicionar(tipoAtividade);
            _uow.Commit();
            itemId = tipoAtividade.IdTipoAtividade;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "Tipo Atividade Retrieve")]
        public void TestRetrieveId()
        {
            var tipoAtividade = _tipoAtividadeService.ObterPorId(itemId);
            Assert.IsNotNull(tipoAtividade);
            Assert.AreEqual(tipoAtividade.IdTipoAtividade, itemId);
        }

        [Order(2), TestCase(TestName = "Tipo Atividade Update")]
        public void TestUpdate()
        {
            var tipoAtividade = _tipoAtividadeService.ObterPorId(itemId);
            tipoAtividade.DuracaoFaixa = 50;
            var resultado = _tipoAtividadeService.Atualizar(tipoAtividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "Tipo Atividade Delete")]
        public void TestDelete()
        {
            var tipoAtividade = _tipoAtividadeService.ObterPorId(itemId);
            var resultado = _tipoAtividadeService.Remover(tipoAtividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }
    }
}
