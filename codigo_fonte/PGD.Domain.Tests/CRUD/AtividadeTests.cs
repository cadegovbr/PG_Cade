using System;
using NUnit.Framework;
using System.Collections.Generic;
using PGD.Domain.Entities;
using PGD.UI.Mvc.App_Start;
using Ninject;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Context;
using PGD.Infra.Data.Interfaces;
using PGD.Infra.CrossCutting.IoC;
using PGD.Domain.Entities.Usuario;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class AtividadeTests
    {
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

        }
    
        //CRUD

        int itemId = 0;

        [Order(0), TestCase("Atividade 4", TestName = "Atividade Insert Atividade 4")]
        public void TestInsert(string NomeAtividade)
        {
            var atividade = new Atividade
            {
                NomAtividade = NomeAtividade,
                PctMinimoReducao = 10,
                Link = "http://linkDeTeste.com",
                Tipos = new List<TipoAtividade> { new TipoAtividade { DuracaoFaixa = 15, DuracaoFaixaPresencial = 35, Faixa = "Faixa Teste 1" } }
            };
            _uow.BeginTransaction();
            var resultado = _atividadeService.Adicionar(atividade);
            _uow.Commit();
            itemId = atividade.IdAtividade;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "Atividade Retrieve Atividade 4")]
        public void TestRetrieveId()
        {
            var atividade = _atividadeService.ObterPorId(itemId);
            Assert.IsNotNull(atividade);
            Assert.AreEqual(atividade.IdAtividade, itemId);
        }

        [Order(2), TestCase("NomeNovo", TestName = "Atividade Update Atividade 4")]
        public void TestUpdate(string nome)
        {
            var atividade = _atividadeService.ObterPorId(itemId);
            atividade.NomAtividade = nome;
            var resultado = _atividadeService.Atualizar(atividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "Atividade Delete Atividade 4")]
        public void TestDelete()
        {
            var atividade = _atividadeService.ObterPorId(itemId);
            var resultado = _atividadeService.Remover(atividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
