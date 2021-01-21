using AutoMapper;
using CGU.Util;
using DomainValidation.Validation;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Application
{
    public class PactoAppService : ApplicationService, IPactoAppService
    {
        private readonly ILogService _logService;
        private readonly IPactoService _pactoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProdutoService _produtoService;
        private readonly IHistoricoService _historicoService;
        private readonly IFeriadoService _feriadoService;
        private readonly IUnidadeService _unidadeService;
        private readonly INotificadorAppService _notificadorAppService;
        private readonly IAvaliacaoProdutoService _avaliacaoProdutoService;


#pragma warning disable S107 // Methods should not have too many parameters
        public PactoAppService(IUsuarioService usuarioService, IUnitOfWork uow, IPactoService pactoService, ILogService logService, IProdutoService produtoService, IHistoricoService historicoService, IFeriadoService feriadoService, IUnidadeService unidadeService,
            ICronogramaService cronogramaService, ISituacaoPactoService situacaoPactoService, INotificadorAppService notificadorAppService, IAvaliacaoProdutoService avaliacaoProdutoService)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(uow)
        {
            _usuarioService = usuarioService;
            _pactoService = pactoService;
            _logService = logService;
            _produtoService = produtoService;
            _historicoService = historicoService;
            _feriadoService = feriadoService;
            _unidadeService = unidadeService;
            _notificadorAppService = notificadorAppService;
            _avaliacaoProdutoService = avaliacaoProdutoService;
        }

        IEnumerable<PactoViewModel> IPactoAppService.ObterTodos()
        {
            return Mapper.Map<IEnumerable<Pacto>, IEnumerable<PactoViewModel>>(_pactoService.ObterTodos());
        }

        public IEnumerable<PactoViewModel> ObterTodos(PactoViewModel objFiltro, bool incluirUnidadesSubordinadas)
        {
            var pacto = Mapper.Map<PactoViewModel, Pacto>(objFiltro);
            var retorno = _pactoService.ConsultarPactos(pacto, incluirUnidadesSubordinadas);
            var dest = MapeiaSemProdutos(retorno);
            return dest;
        }

        private IEnumerable<PactoViewModel> MapeiaSemProdutos(IEnumerable<Pacto> lstPactos)
        {
            var lst = lstPactos.Select(r => new PactoViewModel
                {
                    IdPacto = r.IdPacto,
                    IdSituacaoPacto = r.IdSituacaoPacto,
                    IdTipoPacto = r.IdTipoPacto,
                    DataPrevistaInicio = r.DataPrevistaInicio,
                    DataPrevistaTermino = r.DataPrevistaTermino,
                    DataInterrupcao = r.DataInterrupcao,
                    DataTerminoReal = r.DataTerminoReal,                    
                    Nome = r.Nome,
                    CpfUsuario = r.CpfUsuario,
                    CpfUsuarioCriador = r.CpfUsuarioCriador,
                    CpfUsuarioDirigente = r.CpfUsuarioDirigente,
                    CpfUsuarioSolicitante  = r.CpfUsuarioSolicitante,
                    UnidadeExercicio = r.UnidadeExercicio,
                    SituacaoPacto = Mapper.Map<SituacaoPactoViewModel>(r.SituacaoPacto),
                    TipoPacto = Mapper.Map<TipoPactoViewModel>(r.TipoPacto),
                    StatusAprovacaoDirigente = r.StatusAprovacaoDirigente,
                    StatusAprovacaoSolicitante = r.StatusAprovacaoSolicitante,
                    IndVisualizacaoRestrita = r.IndVisualizacaoRestrita
                }
            );

            return lst;
        }

        public PactoViewModel ObterPorId(int id)
        {
            return Mapper.Map<Pacto, PactoViewModel>(_pactoService.ObterPorId(id));
        }

        public PactoViewModel Atualizar(PactoViewModel pactoViewModel, UsuarioViewModel usuario, eAcaoPacto eAcao)
        {
            BeginTransaction();

            bool isDirigente = usuario.IsDirigente;

            CriaHistoricoAcaoEmPacto(pactoViewModel, isDirigente, usuario, eAcao);

            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoViewModel);

            TratarSituacaoPactoAtualizacao(pactoViewModel, usuario, eAcao, isDirigente, pacto);

            var pactoReturn = _pactoService.Atualizar(pacto, pacto.IdPacto);

            if (pactoReturn.ValidationResult.IsValid)
            {
                var acao = "";
                if (pactoViewModel.Acao == "Assinando")
                {
                    pactoReturn.ValidationResult.Message = Mensagens.MS_004;
                    acao = Domain.Enums.Operacao.Assinatura.ToString();
                }
                else
                {
                    pactoReturn.ValidationResult.Message = Mensagens.MS_006;
                    acao = Domain.Enums.Operacao.Alteração.ToString();
                }

                _logService.Logar(pacto, usuario.CPF, acao);
                Commit();
            }

            pactoViewModel = Mapper.Map<Pacto, PactoViewModel>(pactoReturn);
            return pactoViewModel;
        }

        private static void TratarSituacaoPactoAtualizacao(PactoViewModel pactoViewModel, UsuarioViewModel usuario, eAcaoPacto eAcao, bool isDirigente, Pacto pacto)
        {

            TratarSituacaoPactoAtualizacaoEditando(eAcao, pacto);
            TratarSituacaoPactoAtualizacaoAssinando(pactoViewModel, usuario, eAcao, isDirigente, pacto);
        }

        private static void TratarSituacaoPactoAtualizacaoAssinando(PactoViewModel pactoViewModel, UsuarioViewModel usuario, eAcaoPacto eAcao, bool isDirigente, Pacto pacto)
        {
          
           

            if (eAcao == eAcaoPacto.Assinando && pactoViewModel.DataPrevistaInicio > DateTime.Today && (!isDirigente && pacto.StatusAprovacaoDirigente == 1 || (isDirigente && pacto.StatusAprovacaoSolicitante == 1)))
            {
               
                    pacto.IdSituacaoPacto = (int)eSituacaoPacto.AIniciar;
                
               
            }

            if (eAcao == eAcaoPacto.Assinando && pactoViewModel.DataPrevistaInicio <= DateTime.Today)
            {
                if (isDirigente && pacto.StatusAprovacaoSolicitante == 1 || !isDirigente && pacto.StatusAprovacaoDirigente == 1)
                {

                    pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
                }

                if (isDirigente && pactoViewModel.StatusAprovacaoDirigente == null)
                {
                    pacto.StatusAprovacaoDirigente = 1;
                    pacto.DataAprovacaoDirigente = DateTime.Today;
                    pacto.CpfUsuarioDirigente = usuario.CPF;
                }
            }
            
 
        }

        private static void TratarSituacaoPactoAtualizacaoEditando(eAcaoPacto eAcao, Pacto pacto)
        {
            if (eAcao == eAcaoPacto.Editando)
            {
                pacto.CpfUsuarioDirigente = null;
                pacto.StatusAprovacaoDirigente = null;
                pacto.DataAprovacaoDirigente = (DateTime?)null;
                //Editado CADE derrubar assinatura servidor
                pacto.CpfUsuarioSolicitante = null;
                pacto.StatusAprovacaoSolicitante = null;
                pacto.DataAprovacaoSolicitante = (DateTime?)null;

                pacto.IdSituacaoPacto = (int)eSituacaoPacto.PendenteDeAssinatura;

            }
        }

        public PactoViewModel Remover(PactoViewModel pactoViewModel, UsuarioViewModel usuariologado)
        {

            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoViewModel);
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.Excluido;

            BeginTransaction();
            var pactoReturn = _pactoService.Atualizar(pacto);
            if (pactoReturn.ValidationResult.IsValid)
            {
                pactoViewModel.ValidationResult.Message = PGD.Domain.Constantes.Mensagens.MS_008;
                _logService.Logar(pacto, usuariologado.CPF, Domain.Enums.Operacao.Alteração.ToString());
                Commit();
            }
            pactoViewModel = Mapper.Map<Pacto, PactoViewModel>(pactoReturn);
            return pactoViewModel;

        }

        public PactoViewModel Adicionar(PactoViewModel pactoViewModel, bool isDirigente, UsuarioViewModel usuarioViewModel)
        {
            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoViewModel);

            BeginTransaction();

            var pactoReturn = _pactoService.Adicionar(pacto);
            if (pactoReturn.ValidationResult.IsValid)
            {
                CriaHistoricoAcaoEmPacto(pactoViewModel, isDirigente, usuarioViewModel, eAcaoPacto.Criando);
                Commit();
                _logService.Logar(pacto, pactoViewModel.CpfUsuarioCriador, Domain.Enums.Operacao.Inclusão.ToString());
                Commit();
            }
            pactoViewModel = Mapper.Map<Pacto, PactoViewModel>(pactoReturn);
            return pactoViewModel;
        }

        public bool PossuiPactoPendencias(UsuarioViewModel usuario)
        {
            var user = Mapper.Map<UsuarioViewModel, Usuario>(usuario);
            return _pactoService.PossuiPactoPendencias(user);
        }

        public PactoViewModel BuscarPorId(int id)
        {
            return Mapper.Map<Pacto, PactoViewModel>(_pactoService.BuscarPorId(id));
        }

        public IEnumerable<PactoViewModel> ObterTodos(string include)
        {
            return Mapper.Map<IEnumerable<Pacto>, IEnumerable<PactoViewModel>>(_pactoService.ObterTodos(include));
        }

        public List<FeriadoViewModel> _Feriados { get; set; }
        private void ObterFeriados(DateTime dtAPartirDe)
        {
            _Feriados = Mapper.Map<IEnumerable<Feriado>, IEnumerable<FeriadoViewModel>>(_feriadoService.ObterFeriados(dtAPartirDe)).ToList();
        }

        public FeriadoViewModel ObterFeriado(DateTime data)
        {
            if (_Feriados == null)
                ObterFeriados(DateTime.Now);

            return _Feriados.FirstOrDefault(a => a.data_feriado == data.Date);
        }
        public IEnumerable<FeriadoViewModel> ObterTodosFeriadosPorData(DateTime dataInicio)
        {
            return Mapper.Map<IEnumerable<Feriado>, IEnumerable<FeriadoViewModel>>(_feriadoService.ObterFeriados(dataInicio)).ToList();
        }

    
        public int BuscaStatusAssinatura(PactoViewModel objFiltro)
        {
            var pacto = Mapper.Map<PactoViewModel, Pacto>(objFiltro);
            return _pactoService.BuscaStatusAssinatura(pacto);
        }

        public void CriaHistoricoAcaoEmPacto(Pacto pacto, eAcaoPacto acao)
        {
            string strHistorico = "";

            if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Finalizando)
            {
                strHistorico = "Situação do pacto alterada automaticamente para PENDENTE DE AVALIAÇÃO, pelo término do prazo de sua execução, em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília.";
            }

            if (strHistorico != "")
            {
                var historico = new HistoricoViewModel() { IdPacto = pacto.IdPacto, Descricao = strHistorico };
                var historicoVM = Mapper.Map<HistoricoViewModel, Historico>(historico);
                _historicoService.Adicionar(historicoVM);
            }
        }
        public PactoViewModel CriaHistoricoAcaoEmPacto(PactoViewModel pacto, bool isDirigente, UsuarioViewModel user, eAcaoPacto acao)
        {
            List<string> lstHistorico = new List<string>();
            string strPerfil = ObterDescricaoPerfilUsuario(isDirigente, user);

            if (VerificarSeDeveCriarHistoricoDeAssinatura(acao))
            {
                TratarHistoricoDeAssinatura(pacto, isDirigente, user, lstHistorico, strPerfil);
            }
            else if ((int)PGD.Domain.Enums.eAcaoPacto.Interrompendo == (int)acao)
            {
                lstHistorico.Add("Plano de Trabalho interrompido por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + ", em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília. Motivo: " + pacto.Motivo);
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Avaliando)
            {
                lstHistorico.Add("Plano de Trabalho avaliado por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília.");
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.AvaliandoParcialmente)
            {
                lstHistorico.Add("Plano de Trabalho avaliado parcialmente por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília.");
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Excluindo)
            {
                lstHistorico.Add("Plano de Trabalho excluido por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília.");
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Negando)
            {
                lstHistorico.Add("Solicitação de Plano de Trabalho negada por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília.");
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Suspendendo)
            {
                string textoReativacao = "";
                if (pacto.SuspensoAte.HasValue) textoReativacao = ", com reativação programada para dia " + pacto.SuspensoAte.Value.ToString("dd/MM/yyyy");
                lstHistorico.Add("Plano de Trabalho suspenso por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília" + textoReativacao + "." + pacto.Motivo);
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.VoltandoSuspensao)
            {

                lstHistorico.Add("Término da suspensão por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília." + pacto.Motivo);
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Editando)
            {
                lstHistorico.Add("Plano de Trabalho alterado por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília." + pacto.Motivo);
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.CancelandoAvaliacao)
            {
                lstHistorico.Add("Avaliação de Plano de Trabalho cancelada por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília." + pacto.Motivo);
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.CancelandoAvaliacaoParcialmente)
            {
                lstHistorico.Add("Avaliação parcial de Plano de Trabalho cancelada por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília." + pacto.Motivo);
            }
            else if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Finalizando)
            {
                lstHistorico.Add("Situação do Plano de Trabalho alterada automaticamente pelo término do prazo de sua execução, em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília.");
            }

            TratarHistoricoRestricaoVisualizacao(pacto, user, acao, lstHistorico, strPerfil);
            AdicionarListaHistoricos(pacto, lstHistorico);
            return pacto;
        }

        private void AdicionarListaHistoricos(PactoViewModel pacto, List<string> lstHistorico)
        {
            if (lstHistorico.Any())
            {
                lstHistorico.ForEach(h =>
                {
                    var historico = new HistoricoViewModel() { IdPacto = pacto.IdPacto, Descricao = h };
                    var historicoVM = Mapper.Map<HistoricoViewModel, Historico>(historico);
                    _historicoService.Adicionar(historicoVM);
                });
            }
        }

        private void TratarHistoricoDeAssinatura(PactoViewModel pacto, bool isDirigente, UsuarioViewModel user, List<string> lstHistorico, string strPerfil)
        {
            var nome_usuario = _usuarioService.ObterPorCPF(user.CpfUsuario).Nome;
            var nome_unidade = _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome;
            var data_atual = DateTime.Now.ToShortDateString();
            var hora_atual = DateTime.Now.ToLongTimeString();
            
            lstHistorico.Add("Plano de Trabalho assinado eletronicamente por " + nome_usuario + "," + strPerfil + ", na unidade " + nome_unidade + ", em " + data_atual + ", às " + hora_atual + ", conforme horário oficial de Brasília.");

            if (isDirigente) 
            {
                pacto.CpfUsuarioDirigente = user.CpfUsuario;
                pacto.DataAprovacaoDirigente = DateTime.Now;
                pacto.StatusAprovacaoDirigente = 1;
            }
            else
            { 
                pacto.CpfUsuarioSolicitante = pacto.CpfUsuario;
                pacto.DataAprovacaoSolicitante = DateTime.Now;
                pacto.StatusAprovacaoSolicitante = 1;
            }
 
        }

        private static bool VerificarSeDeveCriarHistoricoDeAssinatura(eAcaoPacto acao)
        {
            return acao == eAcaoPacto.Assinando;
        }

        private static string ObterDescricaoPerfilUsuario(bool isDirigente, UsuarioViewModel user)
        {
            string strPerfil = "Solicitante";
            user.CpfUsuario = user.CPF;


            if (isDirigente)
            {
                strPerfil = "Dirigente";
            }

            return strPerfil;
        }

        private void TratarHistoricoRestricaoVisualizacao(PactoViewModel pacto, UsuarioViewModel user, eAcaoPacto acao, List<string> lstHistorico, string strPerfil)
        {
            if ((int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Assinando || (int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Editando || (int)acao == (int)PGD.Domain.Enums.eAcaoPacto.Criando)
            {
                bool registrarHistoricoPacto = false;
                if (pacto.IdPacto > 0)
                {
                    var pactoAtual = _pactoService.BuscarPorId(pacto.IdPacto);
                    registrarHistoricoPacto = pactoAtual.IndVisualizacaoRestrita != pacto.IndVisualizacaoRestrita;
                }
                else
                {
                    registrarHistoricoPacto = pacto.IndVisualizacaoRestrita;
                }

                if (registrarHistoricoPacto)
                {
                    if (pacto.IndVisualizacaoRestrita)
                    {
                        lstHistorico.Add("Plano de Trabalho marcado como reservado por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília");
                    }
                    else
                    {
                        lstHistorico.Add("Excluída a marcação de Plano de Trabalho reservado por " + _usuarioService.ObterPorCPF(user.CpfUsuario).Nome + "," + strPerfil + ", na unidade " + _unidadeService.ObterUnidades().FirstOrDefault(x => x.IdUnidade == pacto.UnidadeExercicio).Nome + " em " + DateTime.Now.ToShortDateString() + ", às " + DateTime.Now.ToShortTimeString() + ", conforme horário oficial de Brasília");
                    }
                }
            }
        }

        public PactoViewModel AtualizarStatus(PactoViewModel pactoViewModel, UsuarioViewModel usuario, eAcaoPacto eAcao, bool commit = true)
        {
            BeginTransaction();

            if (!usuario.IsDirigente && pactoViewModel.StatusAprovacaoDirigente != null) 
            {
                pactoViewModel.CpfUsuarioSolicitante = usuario.CPF;
                pactoViewModel.StatusAprovacaoSolicitante = 1;
                pactoViewModel.DataAprovacaoSolicitante = DateTime.Now;
            }

            CriaHistoricoAcaoEmPacto(pactoViewModel, usuario.IsDirigente, usuario, eAcao);

            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoViewModel);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuario);

            if (eAcao == eAcaoPacto.Avaliando)
            {
                if (pactoViewModel.DataTerminoReal != null)
                {
                    pacto.DataTerminoReal = pactoViewModel.DataTerminoReal.Value;
                }

                pacto.EntregueNoPrazo = 1;
            }

            pacto = _pactoService.AtualizarStatus(pacto, usr, eAcao, usuario.IsDirigente, commit);

            if (pacto.ValidationResult.IsValid)
            {
                if (eAcao == eAcaoPacto.Assinando)
                    pacto.ValidationResult.Message = Mensagens.MS_006;

                string oper = Domain.Enums.Operacao.Alteração.ToString();

                switch (eAcao.ToString())
                {
                    case "Suspendendo":
                        oper = Domain.Enums.Operacao.Suspensão.ToString();
                        break;

                    case "Interrompendo":
                        oper = Domain.Enums.Operacao.Interrupção.ToString();
                        break;

                    case "Avaliando":
                        oper = Domain.Enums.Operacao.Avaliacao.ToString();
                        break;
                    case "VoltandoSuspensao":
                        oper = Domain.Enums.Operacao.VoltandoSuspensão.ToString();
                        break;
                }

                _logService.Logar(pacto, usuario.CPF, oper);

                List<ValidationError> lstErros = Commit();

                if (lstErros?.Count > 0)
                {
                    lstErros.ForEach (e =>
                    {
                        // LogManagerComum.LogarErro(null, null, " Erro ao salvar a ação. " + e.Message);
                        pacto.ValidationResult.Add(e);
                    });

                    pacto.ValidationResult.Message = Mensagens.MS_010;
                }

            }

            pactoViewModel = Mapper.Map<Pacto, PactoViewModel>(pacto);
            return pactoViewModel;
        }

        public bool PodeEditar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeEditar(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }
        public bool PodeEditarEmAndamento(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeEditarEmAndamento(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeCancelarAvaliacao(PactoViewModel pactoVM, UsuarioViewModel user, bool isDirigente, bool unidadePactoESubordinadaUnidadeUsuario)
        {
            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoVM);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(user);
            return (!isDirigente || pacto.UnidadeExercicio == user.IdUnidadeSelecionada) && _pactoService.PodeCancelarAvaliacao(pacto, usr, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
        }

        public bool PodeVisualizarPactuadoAvaliado(PactoViewModel pactoVM)
        {
            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoVM);            
            return _pactoService.PodeVisualizarPactuadoAvaliado(pacto);
        }
        

        public bool PodeDeletar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeDeletar(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeAssinar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeAssinar(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeInterromper(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeInterromper(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeSuspender(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeSuspender(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeVoltarSuspensao(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeVoltarSuspensao(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeNegar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeNegar(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeAvaliar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeAvaliar(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeEditarObservacaoProduto(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeEditarObservacaoProduto(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }

        public bool PodeVisualizarAvaliacaoProduto(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            var pactoVM = Mapper.Map<PactoViewModel, Pacto>(pacto);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(usuariologado);
            return (!isDirigente || pacto.UnidadeExercicio == usuariologado.IdUnidadeSelecionada) && _pactoService.PodeVisualizarAvaliacaoProduto(pactoVM, usr, isDirigente, unidadePactoEhSubordinadaUnidadeUsuario);
        }



        public void IniciarAutomaticamente()
        {
            BeginTransaction();
            var pactosQueIniciamHoje = _pactoService.ObterTodosAIniciarHoje();
            pactosQueIniciamHoje.ToList().ForEach(p =>
                                                    {
                                                        _pactoService.IniciarPacto(p);
                                                    });
            Commit();
        }

        public void FinalizarAutomaticamente()
        {
            BeginTransaction();
            var pactosFinalizados = _pactoService.ObterTodosEmAndamentoComPrazoFinalizado();
            pactosFinalizados.ToList().ForEach(p =>
            {
                _pactoService.FinalizarPacto(p, false);
                _logService.Logar(p, "Tarefa Automática", Domain.Enums.Operacao.Alteração.ToString());
                CriaHistoricoAcaoEmPacto(p, eAcaoPacto.Finalizando);

                // O envio de email não precisa dar rollback na transação em caso de erro 
                try
                {
                    _notificadorAppService.EnviarEmailNotificacaoFinalizacaoPacto(Mapper.Map<PactoViewModel>(p));
                }
                catch (Exception ex)
                {
                    // LogManagerComum.LogarErro(ex, null, "  => O Plano de Trabalho " + p.IdPacto + " teve o status alterado para PENDENTE PARA AVALIAÇÃO mas não foi possível notificar os interessados.");
                }
            });
            Commit();
        }

    
        public PactoViewModel OrdemVigenteProdutos(OrdemServicoViewModel ordemVigente, PactoViewModel pacto)
        {
            pacto.OrdemServico = ordemVigente;
                foreach (var produto in pacto.Produtos)
                {
                    produto.GrupoAtividade = ordemVigente.Grupos.FirstOrDefault(x => x.IdGrupoAtividade == produto.IdGrupoAtividade);
                    produto.Atividade = produto.GrupoAtividade.Atividades.FirstOrDefault(x => x.IdAtividade == produto.IdAtividade);
                    produto.TipoAtividade = produto.Atividade.Tipos.FirstOrDefault(x => x.IdTipoAtividade == produto.IdTipoAtividade);

                }
            return pacto;
        }

        public PactoViewModel ZeraRestanteCronograma(PactoViewModel pacto)
        {
            pacto.Cronogramas.ForEach(x =>
            {
                if (x.DataCronograma > pacto.SuspensoAPartirDe)
                {
                    x.HorasCronograma = TimeSpan.Zero;
                }
            });
            return pacto;
        }

        public IEnumerable<CronogramaViewModel> ObterTodosCronogramasCpfLogado(string cpf, List<int> idsSituacoes = null,
            DateTime? dataInicial = null, DateTime? dataFinal = null, int? idUnidade = null)
        {
            return Mapper.Map<IEnumerable<Cronograma>, IEnumerable<CronogramaViewModel>>(_pactoService.ObterTodosCronogramasCpfLogado(cpf, 
                idsSituacoes, dataInicial, dataFinal, idUnidade));
        }

        public List<int> ObterSituacoesPactoValido()
        {
            return _pactoService.ObterSituacoesPactoValido();
        }


        public List<PactoViewModel> GetPactosConcorrentes(DateTime dataInicio, DateTime dataFinal, string cpfUsuario, int idPacto)
        {
            return Mapper.Map<List<Pacto>, List<PactoViewModel>>(_pactoService.GetPactosConcorrentes(dataInicio, dataFinal, cpfUsuario, idPacto));
        }

        public ProdutoViewModel AtualizarObservacaoProduto(ProdutoViewModel produto, UsuarioViewModel usuarioVM)
        {
            var historico = new HistoricoViewModel() { IdPacto = produto.IdPacto, Descricao = $"Campo detalhamento alterado por { usuarioVM.Nome}, { usuarioVM.DescricaoPerfil }, na unidade { usuarioVM.nomeUnidade }, em {DateTime.Now.ToString("dd/MM/yyyy")} às { DateTime.Now.ToString("HH:mm:ss")}, conforme horário oficial de Brasília" };
            var historicoVM = Mapper.Map<HistoricoViewModel, Historico>(historico);
            _historicoService.Adicionar(historicoVM);
            return Mapper.Map<Produto, ProdutoViewModel>(_produtoService.AtualizarObservacaoProduto(idProduto: produto.IdProduto, observacoes: produto.Observacoes)); ;
        }

        public ProdutoViewModel AtualizarSituacaoProduto(ProdutoViewModel produto, UsuarioViewModel usuarioVM)
        {
            var historico = new HistoricoViewModel() { IdPacto = produto.IdPacto, Descricao = $"Campo situacao alterado por { usuarioVM.Nome}, { usuarioVM.DescricaoPerfil }, na unidade { usuarioVM.nomeUnidade }, em {DateTime.Now.ToString("dd/MM/yyyy")} às { DateTime.Now.ToString("HH:mm:ss")}, conforme horário oficial de Brasília" };
            var historicoVM = Mapper.Map<HistoricoViewModel, Historico>(historico);
            _historicoService.Adicionar(historicoVM);
            return Mapper.Map<Produto, ProdutoViewModel>(_produtoService.AtualizarSituacaoProduto(idProduto: produto.IdProduto, idSituacaoProduto: produto.IdSituacaoProduto)); ;
        }

        public PactoViewModel CancelarAvaliacao(PactoViewModel pactoVM, AvaliacaoProdutoViewModel avaliacaoProdutoVM, UsuarioViewModel user, eAcaoPacto acao)
        {
            BeginTransaction();

            CriaHistoricoAcaoEmPacto(pactoVM, user.IsDirigente, user, acao);

            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoVM);
            var usr = Mapper.Map<UsuarioViewModel, Usuario>(user);

            pacto.DataTerminoReal = null;
            pacto.EntregueNoPrazo = null;

            var produto = pacto.Produtos.FirstOrDefault(p => p.IdProduto == avaliacaoProdutoVM.IdProduto);
            var avaliacaoProduto = produto.Avaliacoes.FirstOrDefault(ap => ap.IdAvaliacaoProduto == avaliacaoProdutoVM.IdAvaliacaoProduto);

            _avaliacaoProdutoService.Remover(avaliacaoProduto);

            produto.Avaliacoes.Remove(avaliacaoProduto);

            pacto = _pactoService.AtualizarStatus(pacto, usr, acao, user.IsDirigente);

            if (pacto.ValidationResult.IsValid)
            {
                _logService.Logar(pacto, user.CPF, Domain.Enums.Operacao.Alteração.ToString());
                Commit();
            }

            pactoVM = Mapper.Map<Pacto, PactoViewModel>(pacto);
            return pactoVM;
        }

        public System.ComponentModel.DataAnnotations.ValidationResult ValidarDataHoraSuspensaoInterrupcao(PactoViewModel pactoVM, DateTime dataInicioSuspensao, TimeSpan horasConsideradas, Domain.Enums.Operacao operacao)
        {
            return _pactoService.ValidarDataHoraSuspensaoInterrupcao(Mapper.Map<Pacto>(pactoVM), dataInicioSuspensao, horasConsideradas, operacao);
        }

        public System.ComponentModel.DataAnnotations.ValidationResult ValidarDataConclusaoAntecipada(PactoViewModel pacto, DateTime dataConclusaoAntecipada)
        {
            return _pactoService.ValidarDataConclusaoAntecipada(Mapper.Map<Pacto>(pacto), dataConclusaoAntecipada);
        }

        public void RetornarSuspensaoAutomaticamente()
        {
            BeginTransaction();
            var pactosAReiniciar = _pactoService.ObterTodosSuspensosComPrazoParaRetorno();
            pactosAReiniciar.ToList().ForEach(p =>
            {
                _pactoService.ReiniciarPacto(p, false);

                if (p.ValidationResult.IsValid)
                {
                    _logService.Logar(p, "Tarefa Automática", Domain.Enums.Operacao.Alteração.ToString());

                    // O envio de email não precisa dar rollback na transação em caso de erro 
                    try
                    {
                        _notificadorAppService.EnviarEmailNotificacaoReativacaoAutomaticaPacto(Mapper.Map<PactoViewModel>(p));
                    }
                    catch (Exception ex)
                    {
                        // LogManagerComum.LogarErro(ex, null, "  => O Plano de Trabalho " + p.IdPacto + " teve o status alterado para PENDENTE PARA AVALIAÇÃO mas não foi possível notificar os interessados.");
                    }
                } else {
                    // LogManagerComum.LogarErro(null, null, "  => O Plano de Trabalho " + p.IdPacto + " não teve o status alterado para Em execução. Erro: " + p.ValidationResult.Erros?.FirstOrDefault()?.Message);
                }
            });
            Commit();
        }

        public bool PodeVisualizar(PactoViewModel pactoVM, UsuarioViewModel usuarioVM, bool isDirigente, bool unidadePactoESubordinadaUnidadeUsuario)
        {
            var pacto = Mapper.Map<PactoViewModel, Pacto>(pactoVM);
            var usuario = Mapper.Map<UsuarioViewModel, Usuario>(usuarioVM);
            return _pactoService.PodeVisualizar(pacto, usuario, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
        }
    }
}

