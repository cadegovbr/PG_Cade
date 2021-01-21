using System;
using NUnit.Framework;
using System.Collections.Generic;
using PGD.Domain.Entities;
using PGD.UI.Mvc.App_Start;
using Ninject;
using PGD.Domain.Interfaces.Service;
using System.Linq;
using PGD.Infra.Data.Interfaces;
using PGD.Domain.Enums;
using PGD.Infra.CrossCutting.IoC;
using System.Diagnostics;
using PGD.Domain.Services;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Application.AutoMapper;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class PactoTests
    {
        private IUsuarioService _usuarioService;
        private IPactoService _pactoService;
        private IOrdemServicoService _osService;
        private int idPacto = 0;
        private IUnitOfWork _uow;
        public static StandardKernel kernel = new StandardKernel();

        public void InitializaNinject()
        {
            kernel = new StandardKernel();
            BootStrapper.RegisterServicesSingleton(kernel);
            _uow = kernel.Get<IUnitOfWork>();
            _usuarioService = kernel.Get<IUsuarioService>();
            _pactoService = kernel.Get<IPactoService>();
            _osService = kernel.Get<IOrdemServicoService>();
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            var dbInicio = new DbTests();
            dbInicio.Limpa_E_Cria_Database();
            Debug.WriteLine("Banco de dados Limpo e atualizado");
            AutoMapperConfig.RegisterMappings();

        }
        [Order(0), TestCase(TestName = "Listar todos - Vazio")]
        public void TestListarTodosVazio()
        {
            InitializaNinject();
            var pacto = _pactoService.ObterTodos().ToList();
            Assert.IsNotNull(pacto);
            Assert.IsTrue(pacto.Count == 0);
        }

        //CRUD
        [Order(10), TestCase(TestName = "Pacto Insert - Campo Obrigatório")]
        public void TestInsertCamposObrigatoriosOK()
        {
            InitializaNinject();
            var usuario = _usuarioService.ObterPorCPF("02941397450");
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                Nome = "Francir Pacto Insert - Campo Obrigatório",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "",
                TelefoneMovelServidor = "",
                PactoExecutadoNoExterior = false,
                ProcessoSEI = "Teste",
                Motivo = "Motivo de Tetste",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHoraria = 10,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = usuario.CPF,
                IdOrdemServico = os.IdOrdemServico,
                CpfUsuarioCriador = usuario.CPF
            };

            var resultado = _pactoService.Adicionar(pacto);
            Assert.IsTrue(resultado.ValidationResult.IsValid);
        }

 

        [Order(20), TestCase(TestName = "Pacto Insert")]
        public void TestInsert()
        {
            InitializaNinject();
            var usuario = _usuarioService.ObterPorCPF("02941397450");
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                Nome = "Francir Pacto Insert",
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
                CpfUsuarioCriador = usuario.CPF, 
                IdTipoPacto = 1,
                IndVisualizacaoRestrita = false
            };

            _uow.BeginTransaction();
            var resultado = _pactoService.Adicionar(pacto);
            _uow.Commit();
            idPacto = pacto.IdPacto;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(25), TestCase(TestName = "Pacto Retrieve")]
        public void TestRetrieveId()
        {
            InitializaNinject();
            var pacto = _pactoService.ObterPorId(idPacto);
            Assert.IsNotNull(pacto);
            Assert.AreEqual(pacto.IdPacto, idPacto);
        }

        [Order(30), TestCase(TestName = "Pacto Update")]
        public void TestUpdate()
        {
            InitializaNinject();
            var usuario = _usuarioService.ObterPorCPF("02941397450");
            var pactoService = kernel.Get<IPactoService>();
            var os = _osService.ObterOrdemVigente();
            var pacto = new Pacto
            {
                IdPacto = idPacto,
                Nome = "Francir Borges Silvério",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 3333-3333",
                TelefoneMovelServidor = "(22) 4444-4444",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now,
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = usuario.CPF,
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = usuario.CPF, 
                IdTipoPacto = 1
            };

            pactoService.Atualizar(pacto);
            _uow.Commit();
            Assert.IsTrue(true);
        }

        [Order(40), TestCase(TestName = "Listar todos")]
        public void TestListarTodos()
        {
            InitializaNinject();
            var pacto = _pactoService.ObterTodos().ToList();
            Assert.IsNotNull(pacto);
            Assert.IsTrue(pacto.Count > 0);
        }

        [Order(44), TestCase(TestName = "Listar todos parametro Id")]
        public void TestListarTodosFiltroParametroId()
        {
            InitializaNinject();
            var pactoBusca = _pactoService.ObterTodos().FirstOrDefault();
            var objFiltro = new Pacto { IdPacto = pactoBusca.IdPacto };
            var pacto = _pactoService.ConsultarPactos(objFiltro).ToList();
            Assert.IsNotNull(pacto);
            Assert.IsTrue(pacto.Count == 1);
            Assert.IsTrue(pacto.Any(p => p.IdPacto == pactoBusca.IdPacto));
        }


        [Order(45), TestCase(TestName = "Listar todos parametro nome")]
        public void TestListarTodosFiltroParametroNome()
        {
            InitializaNinject();
            var objFiltro = new Pacto {Nome = "Francir Borges Silvério"};
            var pacto = _pactoService.ConsultarPactos(objFiltro).ToList();
            Assert.IsNotNull(pacto);
            Assert.IsTrue(pacto.Count > 0);
        }


        [Order(46), TestCase(TestName = "Listar todos 2 parametros")]
        public void TestListarTodosDoisParametros()
        {
            InitializaNinject();
            var objFiltro = new Pacto { Nome = "Francir Borges Silvério", DataPrevistaInicio = DateTime.Today.AddDays(-1)};
            var pacto = _pactoService.ConsultarPactos(objFiltro).ToList();
            Assert.IsNotNull(pacto);
            Assert.IsTrue(pacto.Count > 0);
        }
        [Order(47), TestCase(TestName = "Listar todos nao traz todos")]
        public void TestListarNaoTrazTodos()
        {
            InitializaNinject();
            var usuario = _usuarioService.ObterPorCPF("02941397450");
            var os = _osService.ObterOrdemVigente();
            var pactoJosuel = new Pacto
            {
                Nome = "Josuel",
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
                CpfUsuarioCriador = usuario.CPF, 
                IdTipoPacto = 1
            };

            _uow.BeginTransaction();
            var resultado = _pactoService.Adicionar(pactoJosuel);
            _uow.Commit();

            var objFiltro = new Pacto { Nome = "Josuel", DataPrevistaInicio = DateTime.Today.AddDays(-1) };
            var pacto = _pactoService.ConsultarPactos(objFiltro).ToList();
            Assert.IsNotNull(pacto);
            Assert.IsTrue(pacto.Count >= 1);
        }

        [Order(50), TestCase(TestName = "Pacto Delete")]
        public void TestDelete()
        {
            InitializaNinject();
            var pacto = _pactoService.ObterPorId(idPacto);
            var resultado = _pactoService.Remover(pacto);
            _uow.Commit();
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
            pacto = _pactoService.ObterPorId(idPacto);
            Assert.AreEqual((int)Enums.eSituacaoPacto.Excluido, pacto.IdSituacaoPacto);
        }


        [Order(55), TestCase(TestName = "Suspender Pacto")]
        public void TestSuspender()
        {
            InitializaNinject();
            var pactoAppService = kernel.Get<IPactoAppService>();
            var usuarioAppService = kernel.Get<IUsuarioAppService>();
            var usuario = _usuarioService.ObterPorCPF("02941397450");

            var os = _osService.ObterOrdemVigente();
            var pacto = new PactoViewModel
            {
                Nome = "Francir Borges Silvério",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = DateTime.Now,
                DataPrevistaTermino = DateTime.Now.AddDays(10),
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = usuario.CPF,
                IdOrdemServico = os.IdOrdemServico,
                CargaHorariaDiaria = TimeSpan.FromHours(8),
                CpfUsuarioCriador = usuario.CPF,
                IdTipoPacto = 1 
            };

            var userAssinante = usuarioAppService.ObterPorCPF("11391275861");

            var resultado = pactoAppService.AtualizarStatus(pacto, userAssinante, Enums.eAcaoPacto.Suspendendo,true);
            Assert.AreEqual((int)Enums.eSituacaoPacto.Suspenso, resultado.IdSituacaoPacto);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));

        }

        [Order(60), TestCase(TestName = "Suspender Pacto - Erro")]
        public void TestSuspenderErro()
        {
            InitializaNinject();
            var usuario = _usuarioService.ObterPorCPF("11391275861");
            var pactoService = kernel.Get<IPactoService>();

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
                DataPrevistaTermino = DateTime.Now.AddDays(10),
                CargaHorariaTotal = 190,
                IdSituacaoPacto = 1,
                CpfUsuario = usuario.CPF,
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = usuario.CPF
            };

            var userAssinante = _usuarioService.ObterPorCPF("11391275861");
            List<Enums.Perfil> perfis = new List<Enums.Perfil>();
            perfis.Add(Enums.Perfil.Dirigente);

            var resultado = pactoService.AtualizarSuspender(pacto, userAssinante, perfis);

            Assert.IsTrue(resultado.ValidationResult.Erros.FirstOrDefault().Message.Contains("não pode assinar"));

        }

        [Order(71), TestCase(TestName = "Pode assinar pacto com sucesso")]
        public void TestPodeAssinar()
        {
            InitializaNinject();
            var userLogado = _usuarioService.ObterPorCPF("11391275861");
            var pacto = new Pacto { IdSituacaoPacto = (int)eSituacaoPacto.AIniciar, DataPrevistaInicio = DateTime.Now.AddDays(5), CpfUsuario = "02941397450" };


            Assert.IsTrue(_pactoService.PodeAssinar(pacto, userLogado, true, true));
            Assert.IsFalse(_pactoService.PodeAssinar(pacto, userLogado, true, false));
        }

        [Order(73), TestCase(TestName = "Pacto SoGravar")]
        public void TestSoGravar()
        {
            InitializaNinject();
            var usuarioLogado = AutoMapper.Mapper.Map<UsuarioViewModel>(_usuarioService.ObterPorCPF("11391275861"));
            var pactoAppservice = kernel.Get<IPactoAppService>();
            var historicoService = kernel.Get<IHistoricoService>();

            var pacto = pactoAppservice.ObterPorId(idPacto);
            pacto.StatusAprovacaoDirigente = 0;
            pacto.StatusAprovacaoSolicitante = 0;
            pacto.IdSituacaoPacto = (int)Enums.eSituacaoPacto.PendenteDeAssinatura;
            pactoAppservice.Atualizar(pacto, usuarioLogado, eAcaoPacto.Criando);
            _uow.Commit();

            var hists = historicoService.ObterTodos(pacto.IdPacto);
            Assert.AreEqual(0, pacto.StatusAprovacaoSolicitante);
            Assert.AreEqual(0, pacto.StatusAprovacaoDirigente);
            Assert.IsFalse(hists.Any(h => h.Descricao.Contains("assinado")));
        }

        [Order(75), TestCase(TestName = "Pacto Assinar")]
        public void TestAssinar()
        {
            InitializaNinject();
            var usuarioLogado =  AutoMapper.Mapper.Map<UsuarioViewModel>( _usuarioService.ObterPorCPF("11391275861"));
            var pactoAppservice = kernel.Get<IPactoAppService>();
            var historicoService = kernel.Get<IHistoricoService>();

            var pacto = pactoAppservice.ObterPorId(idPacto);
            pacto.IdSituacaoPacto = (int)Enums.eSituacaoPacto.PendenteDeAssinatura;
            pactoAppservice.Atualizar(pacto, usuarioLogado, eAcaoPacto.Assinando);
            _uow.Commit();
            var hists = historicoService.ObterTodos(pacto.IdPacto);
            Assert.AreEqual(1, pacto.StatusAprovacaoSolicitante);
            Assert.IsTrue(hists.Any(h => h.Descricao.Contains("assinado")));
        }

   
        [Order(80), TestCase(TestName = "Pacto não pode assinar")]
        public void TestNaoPodeAssinar()
        {
            InitializaNinject();
            var userLogado = _usuarioService.ObterPorCPF("02941397450");
            var pacto = new Pacto { IdSituacaoPacto = (int)eSituacaoPacto.Suspenso, DataPrevistaInicio = DateTime.Now.AddDays(-5), CpfUsuario = userLogado.CPF };

            Assert.IsFalse(_pactoService.PodeAssinar(pacto, userLogado, true, true));
            Assert.IsFalse(_pactoService.PodeAssinar(pacto, userLogado, true, false));
        }

        [Order(90), TestCase(TestName = "Pode interromper pacto com sucesso")]
        public void TestPodeInterromper()
        {
            InitializaNinject();
            var userLogado = _usuarioService.ObterPorCPF("11391275861");
            var pacto = new Pacto
            {
                IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento,
                DataPrevistaInicio = DateTime.Now.AddDays(-1),
                CpfUsuario = "02941397450",
                CpfUsuarioDirigente = userLogado.CPF,
                CpfUsuarioSolicitante = userLogado.CPF,
            };


            Assert.IsTrue(_pactoService.PodeInterromper(pacto, userLogado, true, true));
            Assert.IsFalse(_pactoService.PodeInterromper(pacto, userLogado, true, false));
        }

        [Order(100), TestCase(TestName = "Pacto não pode interromper")]
        public void TestNaoPodeInterromper()
        {
            InitializaNinject();
            var userLogado = _usuarioService.ObterPorCPF("11391275861");
            var pacto = new Pacto { IdSituacaoPacto = (int)eSituacaoPacto.Interrompido, DataPrevistaInicio = DateTime.Now.AddDays(5), CpfUsuario = "02941397450" };

            Assert.IsFalse(_pactoService.PodeInterromper(pacto, userLogado, true, true));
            Assert.IsFalse(_pactoService.PodeInterromper(pacto, userLogado, true, false));
        }

        [Order(110), TestCase(TestName = "Pode avaliar pacto com sucesso")]
        public void TestPodeAvaliar()
        {
            InitializaNinject();
            var userLogado = _usuarioService.ObterPorCPF("11391275861");
            var pacto = new Pacto
            {
                IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento,
                DataPrevistaInicio = DateTime.Now.AddDays(-1),
                CpfUsuario = "02941397450",
                CpfUsuarioCriador = userLogado.CPF,
                CpfUsuarioDirigente = userLogado.CPF,
                CpfUsuarioSolicitante = userLogado.CPF,
            };

            Assert.IsTrue(_pactoService.PodeAvaliar(pacto, userLogado, true, true));
            Assert.IsFalse(_pactoService.PodeAvaliar(pacto, userLogado, true, false));
        }

        [Order(120), TestCase(TestName = "Pacto não pode avaliar")]
        public void TestNaoPodeAvaliar()
        {
            InitializaNinject();
            var userLogado = _usuarioService.ObterPorCPF("11391275861");
            var pacto = new Pacto { IdSituacaoPacto = (int)eSituacaoPacto.Interrompido, DataPrevistaInicio = DateTime.Now.AddDays(5), CpfUsuario = userLogado.CPF };

            Assert.IsFalse(_pactoService.PodeAvaliar(pacto, userLogado, false, true));
            Assert.IsFalse(_pactoService.PodeAvaliar(pacto, userLogado, false, false));
        }
         

        [Order(135), TestCase(TestName = "Obter Todos Cronogramas CPF Logado")]
        public void TesteObterTodosCronogramasCpfLogado()
        {
            InitializaNinject();
            var cronogramas = _pactoService.ObterTodosCronogramasCpfLogado("11391275861", _pactoService.ObterSituacoesPactoValido());
            var pactos = new List<Pacto>();
            cronogramas.ToList().ForEach(c => pactos.Add(_pactoService.ObterPorId(c.IdPacto)));
            Assert.IsFalse(pactos.Any(p => p.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura));
            Assert.IsFalse(pactos.Any(p => p.IdSituacaoPacto == (int)eSituacaoPacto.Excluido));
            Assert.IsFalse(pactos.Any(p => p.IdSituacaoPacto == (int)eSituacaoPacto.Negado));
        }



        [Order(140), TestCase(TestName = "Obter Todos A Iniciar Hoje")]
        public void TesteObterTodosAIniciarHoje()
        {
            InitializaNinject();

            var pactoAIniciar1 = MontarPactoParaTeste(DateTime.Today, DateTime.Today.AddDays(10), eSituacaoPacto.AIniciar,"4132213");
            var pactoAIniciar2 = MontarPactoParaTeste(DateTime.Today, DateTime.Today.AddDays(20), eSituacaoPacto.AIniciar, "123451231234");

            _uow.BeginTransaction();
            pactoAIniciar1 = _pactoService.Adicionar(pactoAIniciar1);
            pactoAIniciar2 = _pactoService.Adicionar(pactoAIniciar2);
            _uow.Commit();


            var pactosIniciandoHoje = _pactoService.ObterTodosAIniciarHoje().ToList();
            Assert.IsTrue(pactosIniciandoHoje.Count(p => p.IdPacto == pactoAIniciar1.IdPacto) > 0);
            Assert.IsTrue(pactosIniciandoHoje.Count(p => p.IdPacto == pactoAIniciar2.IdPacto) > 0);

        }

        [Order(141), TestCase(TestName = "Iniciar Pactos")]
        public void TesteIniciarPactos()
        {
            InitializaNinject();

            _uow.BeginTransaction();
            var pactosQueIniciamHoje = _pactoService.ObterTodosAIniciarHoje();
            pactosQueIniciamHoje.ToList().ForEach(p =>
            {
                _pactoService.IniciarPacto(p);
            });
            _uow.Commit();

            Assert.IsTrue(pactosQueIniciamHoje.All(p => p.IdSituacaoPacto == (int)eSituacaoPacto.EmAndamento));
        }

        private Pacto MontarPactoParaTeste(DateTime dataInicio, DateTime dataFim, eSituacaoPacto situacao, string cpf= "02941397450")
        {
            var os = _osService.ObterOrdemVigente();

            var pacto = new Pacto
            {
                Nome = "Teste 123",
                MatriculaSIAPE = "123456",
                UnidadeExercicio = 5,
                TelefoneFixoServidor = "(11) 11111-1111",
                TelefoneMovelServidor = "(22) 22222-2222",
                PossuiCargaHoraria = false,
                DataPrevistaInicio = dataInicio,
                DataPrevistaTermino = dataFim,
                CargaHorariaTotal = 16,
                IdSituacaoPacto = (int)situacao,
                CpfUsuario = cpf,
                IdOrdemServico = os.IdOrdemServico,
                CargaHoraria = 8,
                CpfUsuarioCriador = cpf,
                IdTipoPacto = 1,
            };

            var cronogramai = new Cronograma() { DataCronograma = dataInicio, DiaUtil = false, Feriado = false, HorasCronograma = 8, Suspenso = false };
            var cronogramaf = new Cronograma() { DataCronograma = dataFim, DiaUtil = false, Feriado = false, HorasCronograma = 8, Suspenso = false };

            pacto.Cronogramas = new List<Cronograma> { cronogramai, cronogramaf };

            return pacto;
        }

        [Order(142), TestCase(TestName = "Testar data Inicio suspensao")]
        public void TesteDataInicioSupensao()
        {
            InitializaNinject();

            Pacto pacto = MontarPactoParaTeste(DateTime.Now, DateTime.Now.AddDays(5), eSituacaoPacto.EmAndamento);

            Assert.IsTrue(_pactoService.ValidarDataHoraSuspensaoInterrupcao(pacto, DateTime.MinValue, TimeSpan.FromHours(3), Enums.Operacao.Suspensão) != null);
            Assert.IsTrue(_pactoService.ValidarDataHoraSuspensaoInterrupcao(pacto, DateTime.MinValue, TimeSpan.FromHours(3), Enums.Operacao.Suspensão).ErrorMessage.StartsWith("A data de Suspensão deve ser maior ou igual à") );
            Assert.IsNull(_pactoService.ValidarDataHoraSuspensaoInterrupcao(pacto, DateTime.Now, TimeSpan.FromHours(3), Enums.Operacao.Suspensão) );

        }

        [Order(143), TestCase(TestName = "Testar horas consideradas suspensao")]
        public void TesteHorasConsideradasSupensao()
        {
            InitializaNinject();

            Pacto pacto = MontarPactoParaTeste(DateTime.Now, DateTime.Now.AddDays(5), eSituacaoPacto.EmAndamento);
            pacto.CargaHoraria = 8;

            Assert.IsTrue(_pactoService.ValidarDataHoraSuspensaoInterrupcao(pacto, DateTime.Now, TimeSpan.FromHours(10), Enums.Operacao.Suspensão) != null);
            Assert.IsTrue(_pactoService.ValidarDataHoraSuspensaoInterrupcao(pacto, DateTime.Now, TimeSpan.FromHours(10), Enums.Operacao.Suspensão).ErrorMessage.Equals("A quantidade de horas deve ser igual ou menor que a carga horaria diária do servidor."));
            Assert.IsNull(_pactoService.ValidarDataHoraSuspensaoInterrupcao(pacto, DateTime.Now, TimeSpan.FromHours(8), Enums.Operacao.Suspensão));

        }

        [Order(144), TestCase(TestName = "Testar data de conclusão antecipada")]
        public void TesteDataConclusaoAntecipada()
        {
            InitializaNinject();

            Pacto pacto = MontarPactoParaTeste(DateTime.Now, DateTime.Now.AddDays(5), eSituacaoPacto.EmAndamento);

            Assert.IsTrue(_pactoService.ValidarDataConclusaoAntecipada(pacto, DateTime.MinValue) != null);
            Assert.IsTrue(_pactoService.ValidarDataConclusaoAntecipada(pacto, DateTime.MinValue).ErrorMessage.StartsWith("A data de conclusão antecipada deve ser maior ou igual à"));
            Assert.IsNull(_pactoService.ValidarDataConclusaoAntecipada(pacto, DateTime.Now));

        }
    }
}
