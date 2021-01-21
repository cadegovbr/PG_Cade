using CGU.Util;
using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class CronogramaTests
    {
        private IFeriadoService _feriadoService;
        private ICronogramaService _cronogramaService;
        private IPactoService _pactoService;
        private IOrdemServicoService _osService;
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

            _feriadoService = kernel.Get<IFeriadoService>();
            _cronogramaService = kernel.Get<ICronogramaService>();
            _pactoService = kernel.Get<IPactoService>();
            _osService = kernel.Get<IOrdemServicoService>();

        }

        int itemId = 0;
        int pactoId = 0;

        [Order(0), TestCase(TestName = "Cronograma Validar")]
        public void TestValidar()
        {
            var dia1 = new Cronograma
            {
                DataCronograma = DateTime.Now.AddDays(-2),
                HorasCronograma = 9,
                DiaUtil = true,
                Feriado = false
            };
            var dia2 = new Cronograma
            {
                DataCronograma = DateTime.Now.AddDays(-1),
                HorasCronograma = 8,
                DiaUtil = false,
                Feriado = true,
                DuracaoFeriado = 4
            };
            var dia3 = new Cronograma
            {
                DataCronograma = DateTime.Now.Date,
                HorasCronograma = 8,
                DiaUtil = true,
                Feriado = false
            };
            var dia4 = new Cronograma
            {
                DataCronograma = DateTime.Now.AddDays(1),
                HorasCronograma = 9,
                DiaUtil = true,
                Feriado = false
            };

            var cronogramaPacto = new CronogramaPacto()
            {
                CPFUsuario = "123123123123",
                DataInicial = DateTime.Now.AddDays(-2),
                DataInicioImpedimento = DateTime.Now.Date,
                HorasDiarias = 8,
                HorasTotais = 20,
                Cronogramas = new List<Cronograma> { dia1, dia2, dia3, dia4 }
            };


            _cronogramaService.ValidarCronograma(cronogramaPacto, new List<Pacto>());

            Assert.IsTrue(cronogramaPacto.ValidationResult.Erros.Any(e => e.Message == "Não é possível salvar o cronograma. Existem horas excedentes."));
            Assert.IsTrue(cronogramaPacto.ValidationResult.Erros.Any(e => e.Message.StartsWith($"{ dia1.DataCronograma.ToString("dd/MM/yyyy")} - Quantidade de horas superior ao máximo de horas permitidas por dia ")));
            Assert.IsTrue(cronogramaPacto.ValidationResult.Erros.Any(e => e.Message == $"{ dia2.DataCronograma.ToString("dd/MM/yyyy")} - Quantidade de horas superior ao máximo de horas permitidas no feriado."));

        }

        [Order(5), TestCase(TestName = "Cronograma Insert")]
        public void TestInsert()
        {
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                Nome = "Cronograma Teste da Silva",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = "11111111555",
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = "11111111111", 
                IdTipoPacto = 1
            };
            _uow.BeginTransaction();
            var resultadoPct = _pactoService.Adicionar(pacto);
            _uow.Commit();

            pactoId = pacto.IdPacto;
            bool diaUtil = false;
            try
            {
                diaUtil = _feriadoService.VerificaFeriado(DateTime.Now);
            }
            catch (Exception ex)
            {

                LogManagerComum.LogarErro(ex, null, $"Erro feriado. URL utilizada: {ConfigurationManager.AppSettings["SIGRHAPI"].ToString()}");
                throw;
            }
            var horasCRn = 0;
            if (diaUtil)
                horasCRn = 8;

            var cronograma = new Cronograma
            {
                IdPacto = pactoId,
                DataCronograma = DateTime.Now,
                HorasCronograma = horasCRn,
                DiaUtil = diaUtil
            };
            _uow.BeginTransaction();
            var resultado = _cronogramaService.Adicionar(cronograma);
            _uow.Commit();
            itemId = cronograma.IdCronograma;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }



        [Order(10), TestCase(TestName = "Cronograma Retrieve")]
        public void TestRetrieveId()
        {
            var crng = _cronogramaService.ObterPorId(itemId);
            Assert.IsNotNull(crng);
            Assert.AreEqual(crng.IdCronograma, itemId);
        }

        [Order(20), TestCase(TestName = "Cronograma Update")]
        public void TestUpdate()
        {
            var crng = _cronogramaService.ObterPorId(itemId);
            crng.HorasCronograma = 5;
            var resultado = _cronogramaService.Atualizar(crng);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(30), TestCase(TestName = "Cronograma Delete")]
        public void TestDelete()
        {
            var crng = _cronogramaService.ObterPorId(itemId);
            var resultado = _cronogramaService.Remover(crng);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }
    }
}
