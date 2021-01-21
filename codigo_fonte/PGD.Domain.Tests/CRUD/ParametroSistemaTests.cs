using Ninject;
using NUnit.Framework;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{
    public class ParametroSistemaTests
    {
        private IParametroSistemaService _parametroSistemaService;
        public static StandardKernel kernel = new StandardKernel();

        public void InitializaNinject()
        {
            kernel = new StandardKernel();
            BootStrapper.RegisterServicesSingleton(kernel);
            _parametroSistemaService = kernel.Get<IParametroSistemaService>();
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            var dbInicio = new DbTests();
            dbInicio.Limpa_E_Cria_Database();
            Debug.WriteLine("Banco de dados Limpo e atualizado");
        }

        [Order(0), TestCase(TestName = "Listar Por Id - Vazio")]
        public void TestListarTodosVazio()
        {
            InitializaNinject();
            var parametroSistema = _parametroSistemaService.ObterPorId(-1);
            Assert.Null(parametroSistema);
        }

        [Order(1), TestCase(TestName = "Listar Por Id Ok")]
        public void TestListarTodosOk()
        {
            InitializaNinject();
            var parametroSistema = _parametroSistemaService.ObterPorId(1);
            Assert.IsNotNull(parametroSistema);
        }


    }
}
