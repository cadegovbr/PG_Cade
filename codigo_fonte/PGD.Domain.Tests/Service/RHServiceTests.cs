using Ninject;
using NUnit.Framework;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.Service
{

    public class RHServiceTests
    {
        private IUsuarioService _usuarioService;
        private IRHService _rhService;
        private IUnitOfWork _uow;
        public static StandardKernel kernel = new StandardKernel();

        [OneTimeSetUp]
        public void Initialize()
        {

            BootStrapper.RegisterServicesSingleton(kernel);
            _uow = kernel.Get<IUnitOfWork>();

            _usuarioService = kernel.Get<IUsuarioService>();
            _rhService = kernel.Get<IRHService>();
        }
        [Order(1), TestCase(TestName = "ObterPerfis")]
        public void ObterPerfis()
        {
            var usuario = _usuarioService.ObterPorCPF("191");
            var perfis = _rhService.ObterPerfis(usuario);
            Assert.IsTrue(perfis.Count() > 0);
        }

        [Order(2), TestCase(TestName = "ObterUnidades")]
        public void ObterUnidades()
        {
            var unidades = _rhService.ObterUnidades();
            Assert.IsTrue(unidades.Count() > 0);
        }

        [Order(3), TestCase(TestName = "ObterFeriados")]
        public void ObterFeriados()
        {
            var feriados = _rhService.ObterFeriados(DateTime.Today);
            Assert.IsTrue(feriados.Count() > 0);
        }

        [Order(4), TestCase(TestName = "VerificaFeriado")]
        public void VerificaFeriado()
        {
            var feriado = DateTime.Parse("25/12/2017");
            var feriadoRe = _rhService.VerificaFeriado(feriado);
            Assert.IsTrue(feriadoRe);
        }
    }
}
