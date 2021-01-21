using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class CriterioAvaliacaoTests
    {
        private ICriterioAvaliacaoService _criterioAvaliacaoService;
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
            _criterioAvaliacaoService = kernel.Get<ICriterioAvaliacaoService>();

        }
    
        //CRUD

        int itemId = 0;

        [Order(0), TestCase("Critério de Avaliação 4", TestName = "Critério de Avaliação Insert Critério de Avaliação 4")]
        public void TestInsert(string DescCriterioAvaliacao)
        {
            NotaAvaliacao nota = new NotaAvaliacao { IdNotaAvaliacao = 3, DescNotaAvaliacao = "Nota 3", LimiteInferiorFaixa = 7, LimiteSuperiorFaixa = 7.9M };

            var criterioAvaliacao = new CriterioAvaliacao
            {
                DescCriterioAvaliacao = DescCriterioAvaliacao,
                StrTextoExplicativo = DescCriterioAvaliacao,
                ItensAvaliacao = new List<ItemAvaliacao> { new ItemAvaliacao { ImpactoNota = -0.1M, DescItemAvaliacao = "Desc1", NotaMaxima = nota } }
            };
            _uow.BeginTransaction();
            var resultado = _criterioAvaliacaoService.Adicionar(criterioAvaliacao);
            _uow.Commit();
            itemId = criterioAvaliacao.IdCriterioAvaliacao;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "Critério de Avaliação Retrieve Critério de Avaliação 4")]
        public void TestRetrieveId()
        {
            var criterioAvaliacao = _criterioAvaliacaoService.ObterPorId(itemId);
            Assert.IsNotNull(criterioAvaliacao);
            Assert.AreEqual(criterioAvaliacao.IdCriterioAvaliacao, itemId);
        }

        [Order(2), TestCase("Nome Editado", TestName = "Critério de Avaliação Update Critério de Avaliação 4")]
        public void TestUpdate(string nome)
        {
            var criterioAvaliacao = _criterioAvaliacaoService.ObterPorId(itemId);
            criterioAvaliacao.DescCriterioAvaliacao = nome;
            var resultado = _criterioAvaliacaoService.Atualizar(criterioAvaliacao);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "Critério de Avaliação Delete Critério de Avaliação 4")]
        public void TestDelete()
        {
            var atividade = _criterioAvaliacaoService.ObterPorId(itemId);
            var resultado = _criterioAvaliacaoService.Remover(atividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
