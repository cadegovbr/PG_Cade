using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Tests.CRUD;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Linq;

namespace PGD.Domain.Tests.Service
{
    [TestFixture]
    public class RHTests
    {
        private IFeriadoService _feriadoService;
        private IUnidadeService _unidadeService;
        private IPerfilService _perfilService;
        private IUsuarioService _usuarioService;
        private Usuario usuario;
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
            _perfilService = kernel.Get<IPerfilService>();
            _unidadeService = kernel.Get<IUnidadeService>();
            _feriadoService = kernel.Get<IFeriadoService>();
            usuario = _usuarioService.ObterPorCPF("11391275861");
        }

        [Order(1), TestCase("2017-12-25", TestName = "Verifica Feriado"), Category("Integration")]
        public void VerificaFeriado(string dataVerificar)
        {
            var nvData = DateTime.Parse(dataVerificar);
            Assert.IsTrue(_feriadoService.VerificaFeriado(nvData));
        }
        [Order(2), TestCase(TestName = "Retorna Perfis"), Category("Integration")]
        public void RetornaPerfis()
        {
            Assert.IsTrue(_perfilService.ObterPerfis(usuario).ToList().Count() > 0);
        }
        [Order(3), TestCase(TestName = "Retorna Unidades"), Category("Integration")]
        public void RetornaUnidades()
        {
            Assert.IsTrue(_unidadeService.ObterUnidades().ToList().Count() > 0);
        }
        [Order(4), TestCase(TestName = "Retorna Feriados"), Category("Integration")]
        public void RetornaFeriados()
        {
            Assert.IsTrue(_feriadoService.ObterFeriados(DateTime.Now.AddDays(-150)).ToList().Count() > 0);
        }
    }
}
