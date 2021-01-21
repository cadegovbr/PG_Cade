using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Enums;
using PGD.Domain.Constantes;
using PGD.Domain.Validations.Pactos;
using PGD.Domain.Entities.Usuario;
using System.ComponentModel.DataAnnotations;

namespace PGD.Domain.Services
{
    public class PactoService : IPactoService
    {
        private readonly IPactoRepository _classRepository;
        private readonly IParametroSistemaService _parametroSistemaService;
        private readonly IHistoricoService _historicoService;

        public PactoService(IPactoRepository classRepository, IParametroSistemaService parametroSistemaService, IHistoricoService historicoService)
        {
            _classRepository = classRepository;
            _parametroSistemaService = parametroSistemaService;
            _historicoService = historicoService;
        }

        public Pacto Adicionar(Pacto obj)
        {
            ValidarInclusaoPacto(obj);

            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_006;

            return _classRepository.Adicionar(obj);
        }

        public Pacto Atualizar(Pacto obj)
        {
            ValidarPacto(obj);
            
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }
            
            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public void ValidarInclusaoPacto(Pacto pacto)
        {
            ValidarPacto(pacto);
            if (pacto.ValidationResult.IsValid && pacto.IdPacto == 0)
            {
                var pactosServidor = _classRepository.Buscar(p => p.CpfUsuario == pacto.CpfUsuario).ToList();
                ValidarPactosSuspensos(pacto, pactosServidor);
                ValidarPactosPendentesAvaliacao(pacto, pactosServidor);
                ValidarPactosPendentesAssinatura(pacto, pactosServidor);
            }
        }

        private void ValidarPactosPendentesAssinatura(Pacto pacto, List<Pacto> pactosServidor)
        {
            var parametroBloquearSeHouverPactoPendenteDeAssinatura = _parametroSistemaService.ObterPorId((int)eParametrosSistema.BloquearSeHouverPactosPendentesDeAssinatura);
            if (parametroBloquearSeHouverPactoPendenteDeAssinatura != null && parametroBloquearSeHouverPactoPendenteDeAssinatura.BoolValue.GetValueOrDefault())
            {

                string urlPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/";
                var pactosSemAssinatura = pactosServidor.Where(p => (p.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura && !p.DataAprovacaoDirigente.HasValue && !p.DataAprovacaoSolicitante.HasValue));
                if (pactosSemAssinatura.Any())
                {
                    string mensagem = $"Usuário impossibilitado de solicitar novo plano de trabalho em razão de uma das pendências a seguir: Existe(m) plano(s) de trabalho(s) do servidor sem assinaturas. ({ string.Join(",", pactosSemAssinatura.Select(p => $"<a href='{ urlPacto }{p.IdPacto}' target='_blank'>{p.IdPacto}</a>")) })";
                    pacto.ValidationResult.Add(new DomainValidation.Validation.ValidationError(mensagem));
                }

                var parametroQuantidadeDiasBloqueioPactoSemAssinatura = _parametroSistemaService.ObterPorId((int)eParametrosSistema.TempoMaximoPendenteAssinatura);
                var dataInicioBloqueio = DateTime.Now.AddDays(parametroQuantidadeDiasBloqueioPactoSemAssinatura.IntValue.GetValueOrDefault() * -1);
                var pactosPendentesAssinatura = pactosServidor.Where(p => p.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura && (!p.DataAprovacaoDirigente.HasValue || !p.DataAprovacaoSolicitante.HasValue)
                    && ((p.DataAprovacaoSolicitante.HasValue && p.DataAprovacaoSolicitante < dataInicioBloqueio)
                         || p.DataAprovacaoDirigente.HasValue && p.DataAprovacaoDirigente < dataInicioBloqueio)).ToList();
                if (pactosPendentesAssinatura.Any())
                {
                    string mensagem = $"Usuário impossibilitado de solicitar novo plano de trabalho em razão de uma das pendências a seguir: Existe(m) plano(s) de trabalho(s) com assinatura pendente há mais de XX dias ({ string.Join(",", pactosPendentesAssinatura.Select(p => $"<a href='{ urlPacto }{p.IdPacto}' target='_blank'>{p.IdPacto}</a>")) })";
                    pacto.ValidationResult.Add(new DomainValidation.Validation.ValidationError(mensagem));
                }
            }
        }

        private void ValidarPactosPendentesAvaliacao(Pacto pacto, List<Pacto> pactosServidor)
        {
            var parametroBloquearSeHouverPactosPendentesDeAvaliacao = _parametroSistemaService.ObterPorId((int)eParametrosSistema.BloquearSeHouverPactosPendentesDeAvaliacao);
            if (parametroBloquearSeHouverPactosPendentesDeAvaliacao != null && parametroBloquearSeHouverPactosPendentesDeAvaliacao.BoolValue.GetValueOrDefault())
            {
                var parametroPrazoBloqueioPactoAposTermino = _parametroSistemaService.ObterPorId((int)eParametrosSistema.PrazoBloqueioPactoAposTermino);
                var dataInicioBloqueio = DateTime.Now.AddDays(parametroPrazoBloqueioPactoAposTermino.IntValue.GetValueOrDefault() * -1);
                var pactosPendentesDeAvaliacao = pactosServidor.Where(p => p.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAvaliacao && p.DataPrevistaTermino < dataInicioBloqueio);
                if (pactosPendentesDeAvaliacao.Any())
                {
                    string urlPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/";
                    string mensagem = $"Usuário impossibilitado de solicitar novo plano de trabalho em razão de uma das pendências a seguir: Existe(m) plano(s) de trabalho(s) do servidor com avaliação final pendente há mais de { parametroPrazoBloqueioPactoAposTermino.IntValue.GetValueOrDefault() } dias do término do plano de trabalho ({ string.Join(",", pactosPendentesDeAvaliacao.Select(p => $"<a href='{ urlPacto }{p.IdPacto}' target='_blank'>{p.IdPacto}</a>")) })";
                    pacto.ValidationResult.Add(new DomainValidation.Validation.ValidationError(mensagem));
                }
            }

            var parametroPrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial = _parametroSistemaService.ObterPorId((int)eParametrosSistema.PrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial);
            if (parametroPrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial != null && parametroPrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial.IntValue.GetValueOrDefault() > 0)
            {
                var dataInicioBloqueio = DateTime.Now.AddDays(parametroPrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial.IntValue.GetValueOrDefault() * -1);
                var pactosEmAndamentoSemAvaliacaoParcial = pactosServidor.Where(p => p.IdSituacaoPacto == (int)eSituacaoPacto.EmAndamento && p.DataUltimaAvaliacaoParcial == null && p.DataPrevistaInicio < dataInicioBloqueio);
                if (pactosEmAndamentoSemAvaliacaoParcial.Any())
                {
                    string urlPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/";
                    string mensagem = $"Usuário impossibilitado de solicitar novo plano de trabalho em razão de uma das pendências a seguir: Existe(m) plano(s) de trabalho(s) do servidor em andamento há mais de { parametroPrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial.IntValue.GetValueOrDefault() } dias do início e sem avaliação parcial ({ string.Join(",", pactosEmAndamentoSemAvaliacaoParcial.Select(p => $"<a href='{ urlPacto }{p.IdPacto}' target='_blank'>{p.IdPacto}</a>")) })";
                    pacto.ValidationResult.Add(new DomainValidation.Validation.ValidationError(mensagem));
                }
            }

            var parametroPrazoBloqueioPactoPendenteAvaliacaoParcial = _parametroSistemaService.ObterPorId((int)eParametrosSistema.PrazoBloqueioAvaliacaoParcial);
            if (parametroPrazoBloqueioPactoPendenteAvaliacaoParcial != null && parametroPrazoBloqueioPactoPendenteAvaliacaoParcial.IntValue.GetValueOrDefault() > 0)
            {
                var dataInicioBloqueio = DateTime.Now.AddDays(parametroPrazoBloqueioPactoPendenteAvaliacaoParcial.IntValue.GetValueOrDefault() * -1);
                var pactosPendentesDeAvaliacaoParcial = pactosServidor.Where(p => p.IdSituacaoPacto == (int)eSituacaoPacto.AvaliadoParcialmente && p.DataUltimaAvaliacaoParcial < dataInicioBloqueio);
                if (pactosPendentesDeAvaliacaoParcial.Any())
                {
                    string urlPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/";
                    string mensagem = $"Usuário impossibilitado de solicitar novo plano de trabalho em razão de uma das pendências a seguir: Existe(m) plano(s) de trabalho(s) do servidor avaliados parcialmente há mais de  { parametroPrazoBloqueioPactoPendenteAvaliacaoParcial.IntValue.GetValueOrDefault() }  dias da última avaliação parcial ({ string.Join(",", pactosPendentesDeAvaliacaoParcial.Select(p => $"<a href='{ urlPacto }{p.IdPacto}' target='_blank'>{p.IdPacto}</a>")) })";
                    pacto.ValidationResult.Add(new DomainValidation.Validation.ValidationError(mensagem));
                }
            }


        }

        private void ValidarPactosSuspensos(Pacto pacto, List<Pacto> pactosServidor)
        {
            var parametroBloquearBloquearSeHouverPactosSuspensos = _parametroSistemaService.ObterPorId((int)eParametrosSistema.BloquearSeHouverPactosSuspensos);
            if (parametroBloquearBloquearSeHouverPactosSuspensos != null && parametroBloquearBloquearSeHouverPactosSuspensos.BoolValue.GetValueOrDefault())
            {
                var pactosSuspensos = pactosServidor.Where(p => p.IdSituacaoPacto == (int)eSituacaoPacto.Suspenso);
                if (pactosSuspensos.Any())
                {
                    string urlPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/";
                    string mensagem = $"Usuário impossibilitado de solicitar novo plano de trabalho em razão de uma das pendências a seguir: Existe(m) plano(s) de trabalho(s) suspenso(s) e não concluídos ({ string.Join(",", pactosSuspensos.Select(p => $"<a href='{ urlPacto }{p.IdPacto}' target='_blank'>{p.IdPacto}</a>")) })";
                    pacto.ValidationResult.Add(new DomainValidation.Validation.ValidationError(mensagem));
                }
            }
        }

        private void ValidarPacto(Pacto pacto)
        {
            pacto.ValidationResult = new PactoValidation().Validate(pacto);
        }

        public List<Pacto> GetPactosConcorrentes(DateTime dataInicio, DateTime dataFinal, string cpfUsuario, int idPacto)
        {
            var situacoesPactoEncerrado = new List<int>{ (int)eSituacaoPacto.Excluido, (int)eSituacaoPacto.Negado,
                (int)eSituacaoPacto.Interrompido };

            var pactosExistentesNoPeriodo = _classRepository.Buscar(x => dataInicio <= x.DataPrevistaTermino
                                         && x.DataPrevistaInicio <= dataFinal
                                         && !situacoesPactoEncerrado.Contains(x.IdSituacaoPacto)
                                         && idPacto != x.IdPacto
                                         && cpfUsuario == x.CpfUsuario).ToList();

            return pactosExistentesNoPeriodo;
        }

        public Pacto ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Pacto> ObterTodos()
        {
  
            return this._classRepository.ObterTodos();
 
        }

        public IEnumerable<Pacto> ConsultarPactos(Pacto objFiltro, bool incluirUnidadesSubordinadas = false)
        {
            return this._classRepository.ConsultarPactos(objFiltro, incluirUnidadesSubordinadas);
        }
 
        public Pacto Remover(Pacto obj)
        {
            var pacto = ObterPorId(obj.IdPacto);
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.Excluido;
            _classRepository.Atualizar(pacto);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }

        public bool PossuiPactoPendencias(Usuario usuario)
        {
            var listaStatusPendencia = new[] { (int)eSituacaoPacto.PendenteDeAssinatura, (int)eSituacaoPacto.AIniciar,
               (int) eSituacaoPacto.EmAndamento, (int)eSituacaoPacto.PendenteDeAvaliacao, (int)eSituacaoPacto.Suspenso };
            var resultado = _classRepository.Buscar(x => listaStatusPendencia.Contains(x.IdSituacaoPacto) && x.CpfUsuario == usuario.Cpf);
            return resultado.Any();
        }

        public Pacto BuscarPorId(int id)
        {
            return _classRepository.BuscarPorId(id);
        }

        public Pacto AtualizarSuspender(Pacto obj, Usuario usuariologado, List<PGD.Domain.Enums.Perfil> Perfis)
        {
            obj.ValidationResult = new SuspenderPactoValidation(usuariologado, Perfis).Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }
            obj.IdSituacaoPacto = (int)eSituacaoPacto.Suspenso;
            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public Pacto RemoverAtrasado(Pacto obj)
        {
            obj.IdSituacaoPacto = (int)eSituacaoPacto.Excluido;
            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public IEnumerable<Pacto> ObterTodos(string include)
        {
            #region Documento CSU008
            return this._classRepository.ObterTodos(include);
            #endregion
        }

        public IEnumerable<Pacto> ObterTodosAtrasados()
        {
            var hoje = DateTime.Now.Date;
            var lista = _classRepository.Buscar(a => a.DataPrevistaInicio < hoje && a.IdSituacaoPacto < (int)eSituacaoPacto.EmAndamento && (a.CpfUsuarioDirigente == null || a.CpfUsuarioSolicitante == null));
            return lista;
        }

        public int BuscaStatusAssinatura(Pacto objFiltro)
        {

            if (objFiltro != null)
                //PLANO DE TRABALHO NÃO FOI ASSINADO POR NINGUEM
                if (string.IsNullOrEmpty(objFiltro.CpfUsuarioDirigente) && string.IsNullOrEmpty(objFiltro.CpfUsuarioSolicitante))
                {
                    return (int)PGD.Domain.Enums.eAssinatura.NaoAssinado;
                }
                //PLANO DE TRABALHO ASSINADO PELO SOLICITANTE
                else if (!string.IsNullOrEmpty(objFiltro.CpfUsuarioSolicitante) && string.IsNullOrEmpty(objFiltro.CpfUsuarioDirigente))
                {
                    return (int)PGD.Domain.Enums.eAssinatura.AssinadoPeloSolicitante;
                }
                //PLANO DE TRABALHO ASSINADO PELO DIRIGENTE E PELO SOLICITANTE
                else if (!string.IsNullOrEmpty(objFiltro.CpfUsuarioSolicitante) && !string.IsNullOrEmpty(objFiltro.CpfUsuarioDirigente))
                {
                    return (int)PGD.Domain.Enums.eAssinatura.AssinadoPorTodos;
                }

            return 0;
        }

        public Pacto AtualizarStatus(Pacto pacto, Usuario usr, eAcaoPacto eAcao, bool isDirigente, bool commit = true)
        {
            bool ignorarValidacoes = false;
             

            ignorarValidacoes = ConfiguraProximoStatus(pacto, usr, eAcao, isDirigente, ignorarValidacoes);

             

            if (!ignorarValidacoes)
            {
                ValidarPacto(pacto);
                if (!pacto.ValidationResult.IsValid)
                {
                    return pacto;
                }

                TratarInclusaoHistoricoAtualizacaoStatus(pacto, eAcao);
            }

            AtualizaEstadoEntidadesRelacionadas(pacto);

            pacto.ValidationResult.Message = Mensagens.MS_004;

            if (commit)
            {
                return _classRepository.Atualizar(pacto, pacto.IdPacto);
            }
            else
            {
                return pacto;
            }
        }

        private void TratarInclusaoHistoricoAtualizacaoStatus(Pacto pacto, eAcaoPacto acao)
        {
            if (acao ==  eAcaoPacto.ReativandoAutomaticamente)
            {
                var historico = new Historico()
                {
                    IdPacto = pacto.IdPacto,
                    Descricao = $"Plano de trabalho reativado automaticamente em { DateTime.Now.ToString("dd/MM/yyyy")}, às { DateTime.Now.ToString("HH:mm:ss")}, conforme programação da reativação."
                };
                _historicoService.Adicionar(historico);
            }
        }

        private bool ConfiguraProximoStatus(Pacto pacto, Usuario usuariologado, eAcaoPacto eAcaoPacto, bool isDirigente, bool ignorarValidacoes)
        {
            if (eAcaoPacto == eAcaoPacto.Assinando)
            {
                ConfiguraProximoStatusAssinando(pacto, usuariologado, isDirigente);
            }
            else if (eAcaoPacto == eAcaoPacto.Negando)
            {
                ConfiguraProximoStatusNegando(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.Interrompendo)
            {
                ConfiguraProximoStatusInterrompendo(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.VoltandoSuspensao)
            {
                ConfiguraProximoStatusVoltandoSuspensao(pacto);
            }
            else if  (eAcaoPacto == eAcaoPacto.ReativandoAutomaticamente)
            {
                ConfiguraProximoPassoReativandoAutomaticamente(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.Suspendendo)
            {
                ConfiguraProximoPassoSuspendendo(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.Avaliando)
            {
                ConfiguraProximoPassoAvaliando(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.AvaliandoParcialmente)
            {
                ConfiguraProximoPassoAvaliandoParcialmente(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.Excluindo)
            {
                ignorarValidacoes = ConfiguraProximoPassoExcluindo(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.CancelandoAvaliacao || eAcaoPacto == eAcaoPacto.CancelandoAvaliacaoParcialmente)
            {
                ignorarValidacoes = ConfiguraProximoPassoCancelandoAvaliacao(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.Iniciando)
            {
                ignorarValidacoes = ConfiguraProximoPassoIniciando(pacto);
            }
            else if (eAcaoPacto == eAcaoPacto.Finalizando)
            {
                ignorarValidacoes = ConfiguraProximoPassoFinalizando(pacto);
            }

            return ignorarValidacoes;
        }

        private static bool ConfiguraProximoPassoFinalizando(Pacto pacto)
        {
            #region Muda a situação para PENDENTE DE AVALIAÇÃO
            bool ignorarValidacoes = true;
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.PendenteDeAvaliacao;
            #endregion
            return ignorarValidacoes;
        }

        private static bool ConfiguraProximoPassoIniciando(Pacto pacto)
        {
            bool ignorarValidacoes = true;
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
            return ignorarValidacoes;
        }

        private static bool ConfiguraProximoPassoCancelandoAvaliacao(Pacto pacto)
        {
            bool ignorarValidacoes = true;
            if (pacto.Produtos.Any(p => p.Avaliacoes.Any()))
            {
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.AvaliadoParcialmente;
            }
            else if (pacto.DataPrevistaTermino.Date >= DateTime.Now.Date)
            {
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
            }
            else
            {
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.PendenteDeAvaliacao;
            }

            return ignorarValidacoes;
        }

        private static bool ConfiguraProximoPassoExcluindo(Pacto pacto)
        {
            #region Muda a situação para EXCLUIDO
            bool ignorarValidacoes = true;
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.Excluido;
            #endregion
            return ignorarValidacoes;
        }

        private static void ConfiguraProximoPassoAvaliandoParcialmente(Pacto pacto)
        {
            #region Muda a situação para AVALIADO PARCIALMENTE;
            //Quando suspenso e for avaliado parcialmente, manter situação como suspenso.
            if (pacto.IdSituacaoPacto != (int)eSituacaoPacto.Suspenso) pacto.IdSituacaoPacto = (int)eSituacaoPacto.AvaliadoParcialmente;
            #endregion
        }

        private static void ConfiguraProximoPassoAvaliando(Pacto pacto)
        {
            #region Muda a situação para AVALIADO;
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.Avaliado;
            #endregion
        }

        private static void ConfiguraProximoPassoSuspendendo(Pacto pacto)
        {
            if (pacto.SuspensoAte.HasValue && pacto.SuspensoAte.Value.Date <= DateTime.Now.Date)
            {
                //para pactos suspensos retroativamente com data retorno menor que hoje.
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
            } else
            {
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.Suspenso;
            }
        }

        private void ConfiguraProximoPassoReativandoAutomaticamente(Pacto pacto)
        {
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
        }

        private void ConfiguraProximoStatusVoltandoSuspensao(Pacto pacto)
        {
            #region Muda a situação para EM ANDAMENTO ou PENDENTE DE AVALIÇÃO
            if (pacto.SuspensoAte.GetValueOrDefault().Date <= DateTime.Now.Date)
            {
                if (pacto.DataPrevistaTermino.Date >= DateTime.Now.Date)
                {
                    pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
                }
                else
                {
                    //Já finalizado
                    var historico = new Historico()
                    {
                        IdPacto = pacto.IdPacto,
                        Descricao = $"Situação do plano de trabalho alterada automaticamente para PENDENTE DE AVALIAÇÃO, pelo término do prazo de sua execução, em { DateTime.Now.ToString("dd/MM/yyyy") }, às { DateTime.Now.ToString("HH:mm") }, conforme horário oficial de Brasília."
                    };
                    _historicoService.Adicionar(historico);

                    pacto.IdSituacaoPacto = (int)eSituacaoPacto.PendenteDeAvaliacao;
                }
            }
            #endregion
        }

        private static void ConfiguraProximoStatusInterrompendo(Pacto pacto)
        {
            #region Muda a situação para INTERROMPIDO
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.Interrompido;
            #endregion
        }

        private static void ConfiguraProximoStatusNegando(Pacto pacto)
        {
            #region Muda a situação para NEGADO
            pacto.IdSituacaoPacto = (int)eSituacaoPacto.Negado;
            #endregion
        }

        private void ConfiguraProximoStatusAssinando(Pacto pacto, Usuario usuariologado, bool isDirigente)
        {
            #region Muda a situação para A INICIAR ou EM ANDAMENTO (caso todos os aprovadores tenham assinado)
            var assinatura = BuscaStatusAssinatura(pacto);

            //A Iniciar
            if (assinatura == (int)eAssinatura.AssinadoPorTodos && pacto.DataPrevistaInicio > DateTime.Now)
            {
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.AIniciar;
            }
            //Em Andamento
            else if (assinatura == (int)eAssinatura.AssinadoPorTodos && pacto.DataPrevistaInicio <= DateTime.Now)
            {
                pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
            }
            if (!isDirigente)
            {
                if (pacto.StatusAprovacaoSolicitante == null && pacto.DataPrevistaInicio > DateTime.Now)
                {
                    pacto.IdSituacaoPacto = (int)eSituacaoPacto.AIniciar;
                    pacto.CpfUsuarioSolicitante = usuariologado.Cpf;
                    pacto.StatusAprovacaoSolicitante = 1;
                    pacto.DataAprovacaoSolicitante = DateTime.Now;
                }
                if (pacto.StatusAprovacaoSolicitante == null && pacto.DataPrevistaInicio <= DateTime.Now)
                {
                    pacto.IdSituacaoPacto = (int)eSituacaoPacto.EmAndamento;
                    pacto.CpfUsuarioSolicitante = usuariologado.Cpf;
                    pacto.StatusAprovacaoSolicitante = 1;
                    pacto.DataAprovacaoSolicitante = DateTime.Now;
                }
            }
            #endregion
        }

        public bool PodeEditar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            #region MESMA UNIDADE

            if (pacto.IdPacto != 0 && !unidadePactoEhSubordinadaUnidadeUsuario) return false;
            if (pacto.IdPacto == 0) return true;
            #endregion

            #region PERMISSÕES DE AÇÕES DO SOLICITANTE
            bool possuiPermissoesSolicitante = PodeEditarPermissoesSolicitante(pacto, usuariologado, isDirigente);
            if (possuiPermissoesSolicitante) return true;
            #endregion

            #region PERMISSÕES DE AÇÕES DO DIRIGENTE
            bool possuiPermissoesDirigente = PodeEditarPermissoesDirigente(pacto, usuariologado, isDirigente);
            if (possuiPermissoesDirigente) return true;
            #endregion

            return false;
        }

        private static bool PodeEditarPermissoesDirigente(Pacto pacto, Usuario usuariologado, bool isDirigente)
        {
            bool possuiPermissoesDirigente = false;
            if (isDirigente && pacto.CpfUsuario != usuariologado.Cpf)
            {
                if (pacto.IdSituacaoPacto == (int)eSituacaoPacto.AIniciar)
                {
                    possuiPermissoesDirigente = true;
                }

                if (pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Suspenso)
                {
                    List<int> situacoesInvalidas = new List<int>() { (int)eSituacaoPacto.Excluido, (int)eSituacaoPacto.Negado, (int)eSituacaoPacto.Interrompido, (int)eSituacaoPacto.Avaliado };
                    if (!situacoesInvalidas.Contains(pacto.IdSituacaoPacto) && pacto.DataPrevistaInicio > DateTime.Now)
                    {
                            possuiPermissoesDirigente = true;
                    }
                }
            }

            return possuiPermissoesDirigente;
        }

        private static bool PodeEditarPermissoesSolicitante(Pacto pacto, Usuario usuariologado, bool isDirigente)
        {
            bool possuiPermissoesSolicitante = false;
            if (!isDirigente && pacto.CpfUsuario == usuariologado.Cpf)
            {

                if (pacto.IdSituacaoPacto ==  (int)eSituacaoPacto.AIniciar || (pacto.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura && pacto.StatusAprovacaoSolicitante.GetValueOrDefault() == 0) || (pacto.IdSituacaoPacto == (int)eSituacaoPacto.EmAndamento))
                {
                    possuiPermissoesSolicitante = true;
                }

                if (pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Suspenso)
                {
                    List<int> situacoesInvalidas = new List<int>() { (int)eSituacaoPacto.Excluido, (int)eSituacaoPacto.Negado, (int)eSituacaoPacto.Interrompido, (int)eSituacaoPacto.Avaliado };

                    if (!situacoesInvalidas.Contains(pacto.IdSituacaoPacto) && pacto.DataPrevistaInicio > DateTime.Now) 
                    {
                        possuiPermissoesSolicitante = true;
                    }
                }

            }

            return possuiPermissoesSolicitante;
        }

        public bool PodeEditarEmAndamento(Pacto pacto, Usuario usr, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            //Dirigente e Servidor pode editar
            if ((isDirigente && unidadePactoEhSubordinadaUnidadeUsuario) || (usr.Cpf.Equals(pacto.CpfUsuario)))
            {
                //Desde que o pacto não esteja suspenso, será exibido para qq Dirigente,
                //mesmo após início do pacto(RN059).

                if (!unidadePactoEhSubordinadaUnidadeUsuario) return false;
                List<int> situacoesInvalidasEditarEmAndamento = new List<int>() { (int)eSituacaoPacto.Excluido, (int)eSituacaoPacto.Negado, (int)eSituacaoPacto.Interrompido, (int)eSituacaoPacto.Avaliado, (int)PGD.Domain.Enums.eSituacaoPacto.Suspenso };
                if (!situacoesInvalidasEditarEmAndamento.Contains(pacto.IdSituacaoPacto) && pacto.DataPrevistaInicio <= DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }

        public bool PodeDeletar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            if (PodeDeletarPermissaoSolicitante(pacto, usuariologado, isDirigente))
                return true;
             
            if (isDirigente && unidadePactoEhSubordinadaUnidadeUsuario &&
                ((pacto.DataPrevistaInicio > DateTime.Now && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Excluido && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Negado)
                    || pacto.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura))
            {
                    return true;
            }
           
 
            return false;
        }

        private static bool PodeDeletarPermissaoSolicitante(Pacto pacto, Usuario usuariologado, bool isDirigente)
        {
            bool usuarioLogadoECriador = (usuariologado.Cpf == pacto.CpfUsuario || (usuariologado.Cpf == pacto.CpfUsuarioCriador));
            bool situacaoElegivel = (pacto.DataPrevistaInicio > DateTime.Now && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Excluido && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Negado)
                    || pacto.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura;

            return !isDirigente && usuarioLogadoECriador && situacaoElegivel;
        }

        public bool PodeAssinar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            #region PERMISSÕES DE AÇÕES DO SOLICITANTE
            if (!isDirigente)
            {
                return PodeAssinarSolicitante(pacto, usuariologado);
            }
            #endregion

            #region PERMISSÕES DE AÇÕES DO DIRIGENTE
            else if (usuariologado.Cpf != pacto.CpfUsuario)
            {
                return  PodeAssinarDirigente(pacto, usuariologado, unidadePactoEhSubordinadaUnidadeUsuario);
            }
            #endregion

            return false;
        }

        private bool PodeAssinarDirigente(Pacto pacto, Usuario usuariologado, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            bool podeAssinarDirigente = false;
            //Opção disponível para os pactos até o dia de início do pacto ou enquanto não tiver sido assinado.
            //Opção só é exibida para o solicitante(ou administrador / solicitante)
            //do plano de trabalho ou o dirigente(ou administrador / dirigente) responsável pelo plano de trabalho e para dirigentes hierarquicamente superiores à unidade.
            var assinatura = BuscaStatusAssinatura(pacto);

            if (pacto.CpfUsuarioDirigente == usuariologado.Cpf || !unidadePactoEhSubordinadaUnidadeUsuario)
            {
                podeAssinarDirigente = false;
            } else
            {
                if ((pacto.DataPrevistaInicio >= DateTime.Today || pacto.StatusAprovacaoDirigente != 1 || pacto.StatusAprovacaoSolicitante != 1) && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Excluido && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Negado && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Suspenso
                 && (assinatura == (int)PGD.Domain.Enums.eAssinatura.NaoAssinado || assinatura == (int)PGD.Domain.Enums.eAssinatura.AssinadoPeloSolicitante))
                {
                    podeAssinarDirigente = true;
                }
            }

            return podeAssinarDirigente;
        }

        private bool PodeAssinarSolicitante(Pacto pacto, Usuario usuariologado)
        {
            bool podeAssinarSolicitante = false;
            if (usuariologado.Cpf == pacto.CpfUsuario || usuariologado.Cpf == pacto.CpfUsuarioCriador)
            {
                //Opção disponível para os pactos até o dia de início do pacto ou enquanto não tiver sido assinado.
                //Opção só é exibida para o solicitante(ou administrador / solicitante) do pacto ou o dirigente(ou administrador / dirigente) responsável pelo pacto e para dirigentes hierarquicamente
                //superiores à unidade.
                var assinatura = BuscaStatusAssinatura(pacto);
                if (pacto.CpfUsuarioSolicitante == usuariologado.Cpf)
                {
                    podeAssinarSolicitante = false;
                }

                //CADE - Editado para permitir assinatura em andamento
                if (assinatura == (int)PGD.Domain.Enums.eAssinatura.NaoAssinado && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Excluido && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Negado && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Suspenso
                    && (pacto.StatusAprovacaoSolicitante != 1)) podeAssinarSolicitante = true;
                /*
                if (pacto.DataPrevistaInicio >= DateTime.Today && assinatura == (int)PGD.Domain.Enums.eAssinatura.NaoAssinado && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Excluido && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Negado && pacto.IdSituacaoPacto != (int)eSituacaoPacto.Suspenso
                    && (pacto.StatusAprovacaoSolicitante != 1)) podeAssinarSolicitante = true;
                */
            }


            else if (pacto.IdPacto == 0)
            {
                podeAssinarSolicitante = true;
            }

            return podeAssinarSolicitante;
        }

        public bool PodeInterromper(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            #region PERMISSÕES DE AÇÕES DO DIRIGENTE
            if (isDirigente && unidadePactoEhSubordinadaUnidadeUsuario && usuariologado.Cpf != pacto.CpfUsuario )
            {
                var assinatura = BuscaStatusAssinatura(pacto);
                if (pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.Interrompido)
                {
                    return false;
                }
                //Disponível para pactos que já foram iniciados e que foram assinados.
                //Caso o pacto seja avaliado ou negado, esta opção não é exibida.
                //Se um pacto for interrompido, nenhuma outra opção de ação é exibida. Opção só é exibida para o dirigente (ou administrador/dirigente) responsável pelo pacto e
                //para dirigentes hierarquicamente superiores à unidade.
                else if (pacto.DataPrevistaInicio <= DateTime.Now && assinatura == (int)PGD.Domain.Enums.eAssinatura.AssinadoPorTodos
                        && (pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Negado &&
                            pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Excluido &&
                            pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.AIniciar))
                {
                    return true;

                }

            }
            #endregion

            return false;
        }

        public bool PodeSuspender(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {

            #region PERMISSÕES DE AÇÕES DO DIRIGENTE
            if ((isDirigente && unidadePactoEhSubordinadaUnidadeUsuario && usuariologado.Cpf != pacto.CpfUsuario) || (usuariologado.Cpf == pacto.CpfUsuario))
            {
                //Disponível para pactos que já foram iniciados e que foram assinados. Caso um pacto seja suspenso, o ícone de "Interromper"
                //continua sendo exibido. Após ser acionado, este ícone não é exibido. Caso o pacto tenha sido avaliado, negado, interrompido ou esteja suspenso, esta opção não é exibida.
                //Opção só é exibida para o dirigente (ou administrador/dirigente) responsável pelo pacto e para dirigentes hierarquicamente superiores à unidade.
                //Regra temporaria -> O pacto só pode ter uma suspensao. Tem de ver com fernanda se vamos mudar essa regra.
                List<int> situacoesPossiveis = new List<int> {
                    (int)PGD.Domain.Enums.eSituacaoPacto.EmAndamento,
                    (int)PGD.Domain.Enums.eSituacaoPacto.PendenteDeAvaliacao,
                    (int)PGD.Domain.Enums.eSituacaoPacto.AvaliadoParcialmente
                };
                if (situacoesPossiveis.Contains(pacto.IdSituacaoPacto))
                {
                    return true;
                }
            }
            #endregion

            return false;
        }

        public bool PodeVoltarSuspensao(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            if (pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.Suspenso)
            {
                #region PERMISSÕES DE AÇÕES DO DIRIGENTE
                if (isDirigente && unidadePactoEhSubordinadaUnidadeUsuario)
                {
                //Disponível para pactos que estão suspensos (após acionar o "Suspender").
                //Caso esta opção seja acionada o pacto retorna a sua vigência. Opção só é exibida para o dirigente (ou administrador/dirigente)
                //responsável pelo pacto e para dirigentes hierarquicamente superiores à unidade.
                        return true;
                }

                if (pacto.CpfUsuario?.Equals(usuariologado.Cpf) ?? false)
                {
                    //disponível para o próprio usuário solicitar o retorno da suspensão
                    return true;
                }
            } 

            #endregion

            return false;
        }

        public bool PodeNegar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            #region PERMISSÕES DE AÇÕES DO DIRIGENTE
            if (isDirigente && unidadePactoEhSubordinadaUnidadeUsuario && usuariologado.Cpf != pacto.CpfUsuario)
            {
                if (pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.Negado || pacto.IdSituacaoPacto == (int)eSituacaoPacto.Excluido)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            #endregion

            return false;

        }
        public bool PodeAvaliar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            #region PERMISSÕES DE AÇÕES DO DIRIGENTE
            if (isDirigente && unidadePactoEhSubordinadaUnidadeUsuario && pacto.CpfUsuario != usuariologado.Cpf)
            {
                if (pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.Avaliado)
                {
                    return false;
                }
                else
                {
                    var assinatura = BuscaStatusAssinatura(pacto);
                    //Disponível para pactos que já foram iniciados e que foram assinados pelo Solicitante e pelo Dirigente.
                    //Se um pacto for avaliado, nenhuma outra opção de ação é exibida.Caso o pacto tenha sido avaliado, negado, interrompido ou esteja suspenso,
                    //esta opção não é exibida.Opção só é exibida para o dirigente(ou administrador / dirigente) responsável pelo pacto e para dirigentes hierarquicamente superiores à unidade.
                    if (pacto.DataPrevistaInicio <= DateTime.Now && assinatura == (int)PGD.Domain.Enums.eAssinatura.AssinadoPorTodos
                       && (pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Avaliado &&
                            pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Negado &&
                            pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Interrompido &&
                            pacto.IdSituacaoPacto != (int)PGD.Domain.Enums.eSituacaoPacto.Excluido))
                    {

                        return true;
                    }
                }
            }
            #endregion

            return false;
        }

        public bool PodeCancelarAvaliacao(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            if (isDirigente && unidadePactoEhSubordinadaUnidadeUsuario && pacto.CpfUsuario != usuariologado.Cpf
             && (pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.Avaliado))
            {
                return true;
            }

            return false;
        }

        public bool PodeVisualizarPactuadoAvaliado(Pacto pacto)
        {

            if (pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.AIniciar ||
                pacto.IdSituacaoPacto == (int)PGD.Domain.Enums.eSituacaoPacto.PendenteDeAssinatura)
            {
                    return false;
            }
            

            return true;
        }

        public bool PodeEditarObservacaoProduto(Pacto pactoVM, Usuario usr, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            if ((isDirigente && unidadePactoEhSubordinadaUnidadeUsuario) || (usr.Cpf.Equals(pactoVM.CpfUsuario))
             && (pactoVM.IdSituacaoPacto == (int)eSituacaoPacto.EmAndamento ||
                    pactoVM.IdSituacaoPacto == (int)eSituacaoPacto.AvaliadoParcialmente ||
                    pactoVM.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAvaliacao ||
                    pactoVM.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura))
            {
                return true;
            }
            return false;
        }

        public bool PodeVisualizarAvaliacaoProduto(Pacto pactoVM, Usuario usr, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario)
        {
            if ((isDirigente && unidadePactoEhSubordinadaUnidadeUsuario) || (usr.Cpf.Equals(pactoVM.CpfUsuario))
             && (
                    pactoVM.IdSituacaoPacto == (int)eSituacaoPacto.AvaliadoParcialmente ||
                    pactoVM.IdSituacaoPacto == (int)eSituacaoPacto.Avaliado
                   ))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Pacto> ObterTodosAIniciarHoje()
        {
            var hoje = DateTime.Today;
            return _classRepository.Buscar(a => a.DataPrevistaInicio <= hoje && a.IdSituacaoPacto == (int)eSituacaoPacto.AIniciar);
        }

        public IEnumerable<Pacto> ObterTodosEmAndamentoComPrazoFinalizado()
        {
            var hoje = DateTime.Today;
            return _classRepository.Buscar(a => a.DataPrevistaTermino < hoje && ( a.IdSituacaoPacto == (int)eSituacaoPacto.EmAndamento || a.IdSituacaoPacto == (int)eSituacaoPacto.AvaliadoParcialmente));
        }

        public IEnumerable<Pacto> ObterTodosSuspensosComPrazoParaRetorno()
        {
            var hoje = DateTime.Today;
            return _classRepository.Buscar(a => a.SuspensoAte <= hoje && (a.IdSituacaoPacto == (int)eSituacaoPacto.Suspenso) && a.DataPrevistaTermino >= DateTime.Now);
        }


        public Pacto IniciarPacto(Pacto obj)
        {
            return AtualizarStatus(obj, null, eAcaoPacto.Iniciando, false);
        }

        public Pacto FinalizarPacto(Pacto obj, bool commit = false)
        {            
            return AtualizarStatus(obj, null, eAcaoPacto.Finalizando, false, commit: commit);
        }

        public Pacto ReiniciarPacto(Pacto pacto, bool commit = false)
        {
            return AtualizarStatus(pacto, null, eAcaoPacto.ReativandoAutomaticamente, false, commit: commit);
        }

        public Pacto Atualizar(Pacto pacto, int idPacto)
        {
            AtualizaEstadoEntidadesRelacionadas(pacto);
            ValidarPacto(pacto);

            if (!pacto.ValidationResult.IsValid)
            {
                return pacto;
            }
            return _classRepository.Atualizar(pacto, idPacto);
        }

        public Pacto AtualizarStatus(Pacto obj, Usuario usuariologado, eAcaoPacto eAcaoPacto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cronograma> ObterTodosCronogramasCpfLogado(string cpf, List<int> idsSituacoes = null,
            DateTime? dataInicial = null, DateTime? dataFinal = null, int? idUnidade = null)
        {
            var resultado = _classRepository.Buscar(x => x.CpfUsuario == cpf);

            if (idsSituacoes != null)
            {
                resultado = resultado.Where(x => idsSituacoes.Contains(x.IdSituacaoPacto));
            }

            if (idUnidade.HasValue && idUnidade > 0)
                resultado = resultado.Where(x => x.UnidadeExercicio == idUnidade);

            return resultado.SelectMany(p => 
                p.Cronogramas.Where(c => (!dataInicial.HasValue || c.DataCronograma >= dataInicial.Value)
                                        && (!dataFinal.HasValue || c.DataCronograma <= dataFinal.Value)

                )).ToList();
        }

        public List<int> ObterSituacoesPactoValido()
        {
            return new List<int>() { (int)eSituacaoPacto.AIniciar, (int)eSituacaoPacto.Avaliado, (int)eSituacaoPacto.EmAndamento, (int)eSituacaoPacto.Interrompido, (int)eSituacaoPacto.PendenteDeAvaliacao, (int)eSituacaoPacto.Suspenso };
        }

        public void AtualizaEstadoEntidadesRelacionadas(Pacto pacto)
        {
            _classRepository.AtualizaEstadoEntidadesRelacionadas(pacto);
        }

        public ValidationResult ValidarDataHoraSuspensaoInterrupcao(Pacto pacto, DateTime dataInicioSuspensao, TimeSpan horasConsideradas, Enums.Operacao operacao)
        {
            if (TimeSpan.FromHours(pacto.CargaHoraria) < horasConsideradas)
            {
                return new ValidationResult("A quantidade de horas deve ser igual ou menor que a carga horaria diária do servidor.");
            }

            DateTime dataMinimoPacto = ObterDataMinimaSuspensaoPacto();
            if (pacto.DataPrevistaInicio > dataMinimoPacto)
                dataMinimoPacto = pacto.DataPrevistaInicio;

            if (dataInicioSuspensao < dataMinimoPacto)
            {
                return new ValidationResult($"A data de {operacao.ToString()} deve ser maior ou igual à { dataMinimoPacto.ToShortDateString() } .");
            }

            DateTime dataMaximaPacto = pacto.Cronogramas.Max(c => c.DataCronograma);
            if (dataInicioSuspensao > dataMaximaPacto)
            {
                return new ValidationResult($"A data de {operacao.ToString()} deve ser menor ou igual à { dataMaximaPacto.ToShortDateString() } .");
            }

            return null;
        }

        public DateTime ObterDataMinimaSuspensaoPacto()
        {
            var parametroDiasRetroagirInterrupcao = _parametroSistemaService.ObterPorId((int)eParametrosSistema.QuantidadesDiaRetroagirInterrupcao);
            int quantidadeDiasRetroativos = parametroDiasRetroagirInterrupcao?.IntValue.GetValueOrDefault() ?? 1;
            DateTime dataMinimoPacto = DateTime.Now.Date.AddDays(quantidadeDiasRetroativos * -1);
            return dataMinimoPacto;
        }

        public ValidationResult ValidarDataConclusaoAntecipada(Pacto pacto, DateTime dataConclusaoAntecipada)
        {
            var parametroDiasConclusaoAntecipada = _parametroSistemaService.ObterPorId((int)eParametrosSistema.QuantidadesDiaConclusaoAntecipada);

            int quantidadeDiasRetroativos = parametroDiasConclusaoAntecipada?.IntValue.GetValueOrDefault() ?? 1;

            DateTime dataMinimoPacto = DateTime.Now.Date.AddDays(quantidadeDiasRetroativos * -1);

            if (pacto.DataPrevistaInicio > dataMinimoPacto)
            {
                dataMinimoPacto = pacto.DataPrevistaInicio;
            }

            if (dataConclusaoAntecipada < dataMinimoPacto)
            {
                return new ValidationResult($"A data de conclusão antecipada deve ser maior ou igual à { dataMinimoPacto.ToShortDateString() } .");
            }

            return null;
        }


        public bool PodeVisualizar(Pacto pacto, Usuario usuario, bool isDirigente,   bool unidadePactoESubordinadaUnidadeUsuario)
        {
            if (pacto.IndVisualizacaoRestrita)
            {
                return ( isDirigente && ( usuario.Unidade == pacto.UnidadeExercicio || unidadePactoESubordinadaUnidadeUsuario )) || 
                       usuario.Administrador || 
                       (pacto.CpfUsuario?.Equals(usuario.Cpf) ?? true);
            } else
            {
                return true;
            }
            
        }
    }
}
