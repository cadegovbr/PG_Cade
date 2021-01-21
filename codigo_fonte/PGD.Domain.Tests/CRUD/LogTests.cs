using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities;
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
    public class LogTests
    {
        private IUnitOfWork _uow;
        public static StandardKernel kernel = new StandardKernel();
        private ILogService _logService;
        private IUsuarioService _usuarioService;
        private IOrdemServicoService _osService;

        [OneTimeSetUp]
        public void Initialize()
        {
            kernel = new StandardKernel();
            BootStrapper.RegisterServicesSingleton(kernel);
            _uow = kernel.Get<IUnitOfWork>();
            _logService = kernel.Get<ILogService>();
            _usuarioService = kernel.Get<IUsuarioService>();
            _osService = kernel.Get<IOrdemServicoService>();
        }

        [Order(1), TestCase(TestName = "GerarLog")]
        public void GerarLogAcao()
        {
            var usuario = _usuarioService.ObterPorCPF("02941397450");
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                Nome = "Francir Borges Silvério",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = usuario.CPF,
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = usuario.CPF  
            };
            _uow.BeginTransaction();
            var resultado = _logService.Logar(pacto, usuario.CPF, "Inclusão");
            _uow.Commit();
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(2), TestCase(TestName = "GerarLogException")]
        public void GerarLogAcaoException()
        {
            var usuario = _usuarioService.ObterPorCPF("11391275861");
            var os = _osService.ObterOrdemVigente();

            var pacto = new Pacto
            {
                Nome = "Francir Borges Silva",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = usuario.CPF,
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = usuario.CPF
            };
            _uow.BeginTransaction();
            var resultado = _logService.Logar(null, usuario.CPF, "Edição");
            _uow.Commit();
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }
    }
}
