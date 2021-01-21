using System;
using NUnit.Framework;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using PGD.Domain.Entities;
using Ninject;
using PGD.Domain.Tests.CRUD;

namespace PGD.Domain.Tests.Service
{
    [TestFixture]
    class UsuarioServiceTests
    {
        private IUsuarioService _usuarioService;
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

            _usuarioService = kernel.Get<IUsuarioService>();
        }

        [TestCase("0007387475300", TestName = "Retorna Usuário - com muitos zeros")]
        [TestCase("07387475300", TestName = "Retorna Usuário - com zeros")]
        [TestCase("7387475300", TestName = "Retorna Usuário - Sem zeros")]
        public void RetornaUsuario(string cpf)
        {
            Assert.IsNotNull(_usuarioService.ObterPorCPF(cpf));
        }
    }
}
