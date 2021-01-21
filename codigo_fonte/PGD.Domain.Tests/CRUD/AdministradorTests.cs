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
    public class AdministradorTests
    {

        private IAdministradorService _administradorService;
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

            _administradorService = kernel.Get<IAdministradorService>();

        }
        //CRUD
        int itemId = 0;

        [Order(0), TestCase(TestName = "Administrador Insert")]
        public void TestInsert()
        {

            var administrador = new Administrador
            {
                CPF = "000000000191"
            };
            _uow.BeginTransaction();
            var resultado = _administradorService.Adicionar(administrador);
            _uow.Commit();
            itemId = resultado.IdAdministrador;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "Administrador Retrieve")]
        public void TestRetrieveId()
        {
            var adm = _administradorService.ObterPorId(itemId);
            Assert.IsNotNull(adm);
            Assert.AreEqual(adm.IdAdministrador, itemId);
        }

        [Order(2), TestCase(TestName = "Administrador Update")]
        public void TestUpdate()
        {
            var adm = _administradorService.ObterPorId(itemId);
            adm.CPF = "02350277151";
            var resultado = _administradorService.Atualizar(adm);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "Administrador Delete")]
        public void TestDelete()
        {
            var adm = _administradorService.ObterPorId(itemId);
            var resultado = _administradorService.Remover(adm);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
