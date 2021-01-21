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
    public class HistoricoTests
    {
        private IUnitOfWork _uow;
        private IHistoricoService _historicoService;
        private IPactoService _pactoService;
        private IOrdemServicoService _osService;
        public static StandardKernel kernel = new StandardKernel();

        int itemId = 0;
        int pactoId = 0;
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

            _historicoService = kernel.Get<IHistoricoService>();
            _pactoService = kernel.Get<IPactoService>();
            _osService = kernel.Get<IOrdemServicoService>();
        }
        [Order(0), TestCase(TestName = "Historico Insert")]
        public void TestInsert()
        {
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                Nome = "Historico Teste da Silva",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = "11111111111",
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = "11111111111", 
                IdTipoPacto = 1
                
            };
            _uow.BeginTransaction();
            var resultadoPct = _pactoService.Adicionar(pacto);
            _uow.Commit();

            pactoId = pacto.IdPacto;

            var historico = new Historico
            {
                IdPacto = pactoId,
                Descricao = "teste inserção de histórico"

            };
            _uow.BeginTransaction();
            var resultado = _historicoService.Adicionar(historico);
            _uow.Commit();
            itemId = historico.IdHistorico;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "Historico Retrieve")]
        public void TestRetrieveId()
        {
            var hist = _historicoService.BuscarPorId(pactoId, itemId);
            Assert.IsNotNull(hist);
            Assert.AreEqual(hist.IdHistorico, itemId);
        }
        [Order(2), TestCase(TestName = "Historico Update")]
        public void TestUpdate()
        {
            var hist = _historicoService.BuscarPorId(pactoId, itemId);
            hist.Descricao = "teste atualização historico";
            _uow.BeginTransaction();
            var resultado = _historicoService.AtualizarIdPacto(hist, hist.IdHistorico);
            _uow.Commit();
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
