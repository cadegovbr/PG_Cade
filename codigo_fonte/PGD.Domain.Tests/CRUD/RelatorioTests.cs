sing NUnit.Framework;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{


    [TestFixture]
    public class RelatorioTests
    {
        private IRelatorioAppService _relatorioAppService;

        public IRelatorioAppService RelatorioAppService { get => _relatorioAppService; set => _relatorioAppService = value; }

        [OneTimeSetUp]
        public void Initialize()
        {
        }
        [Order(0), TestCase(TestName = "Relatorio Atividade PGD")]
        public void RetornaRelatorioAtividadePGDPorPeriodo()
        {
            var obj = new RelatorioConsolidadoViewModel
            {
                DataInicial = DateTime.Now,
                DataFinal = DateTime.Now.AddDays(10)

            };
            var resultado = _relatorioAppService.RelatorioAtividadePgdPeriodo(obj);
            Assert.IsTrue(resultado != null);
        }

        [Order(1), TestCase(TestName = "Relatorio Alocação Simultanea")]
        public void RetornaRelatorioAlocacaoSimultanea()
        {
            var obj = new SearchFlPontoViewModel();

            var resultado = _relatorioAppService.RetornaRelatorioAlocacaoSimultanea(obj);
            Assert.IsTrue(resultado != null);
        }
    }
}
