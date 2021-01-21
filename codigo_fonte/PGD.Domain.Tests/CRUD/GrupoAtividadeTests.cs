using System;
using NUnit.Framework;
using System.Collections.Generic;
using PGD.Domain.Entities;
using PGD.UI.Mvc.App_Start;
using Ninject;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using PGD.Domain.Entities.Usuario;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class GrupoAtividadeTests
    {
        private IGrupoAtividadeService _grupoAtividadeService;
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
            _grupoAtividadeService = kernel.Get<IGrupoAtividadeService>();

        }

        //CRUD
        int itemId = 0;

        [Order(0), TestCase("Grupo Atividade 4", TestName = "GrupoAtividade Insert Grupo Atividade 4")]
        public void TestInsert(string NomeGrupoAtividade)
        {
            var grupo = new GrupoAtividade
            {
                NomGrupoAtividade = NomeGrupoAtividade,
                Atividades = new List<Atividade>()
            };
            grupo.Atividades.Add(_atividadeService.ObterPorId(1));
            _uow.BeginTransaction();
            var resultado = _grupoAtividadeService.Adicionar(grupo);
            _uow.Commit();
            itemId = grupo.IdGrupoAtividade;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "GrupoAtividade Retrieve Grupo 4 By ID")]
        public void TestRetrieveId()
        {
            var grupo = _grupoAtividadeService.ObterPorId(itemId);
            Assert.IsNotNull(grupo);
            Assert.AreEqual(grupo.IdGrupoAtividade, itemId);
        }

        [Order(2), TestCase("NomeNovo", TestName = "GrupoAtividade Update Grupo 4")]
        public void TestUpdate(string nome)
        {
            var grupo = _grupoAtividadeService.ObterPorId(itemId);
            grupo.NomGrupoAtividade = nome;
            var resultado = _grupoAtividadeService.Atualizar(grupo);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "GrupoAtividade Delete Grupo 4")]
        public void TestDelete()
        {
            var grupo = _grupoAtividadeService.ObterPorId(itemId);
            var resultado = _grupoAtividadeService.Remover(grupo);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
