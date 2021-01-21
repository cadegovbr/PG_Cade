using AutoMapper;
using CGU.Util;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.Util;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGD.Application
{
    public class NotificadorAppService: ApplicationService, INotificadorAppService
    {
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly IUsuarioService _usuarioService;
        private readonly ISituacaoPactoService _situacaoPactoService;

        public NotificadorAppService(IUnitOfWork uow, 
            IUsuarioAppService usuarioAppService,
            IUsuarioService usuarioService,
            ISituacaoPactoService situacaoPactoService) : base(uow)
        {
            _usuarioAppService = usuarioAppService;
            _usuarioService = usuarioService;
            _situacaoPactoService = situacaoPactoService;
        }

  
        public bool TratarNotificacaoPacto(PactoViewModel pactoBuscado, 
                                           UsuarioViewModel usuarioLogado, 
                                           string oper, 
                                           AvaliacaoProdutoViewModel apvm = null)
        {
            try
            {
                if (oper.Equals(Domain.Enums.Operacao.Inclusão.ToString()))
                {
                    MontarEEnviarNotificacaoInclusaoPacto(pactoBuscado);
                }
                else if (oper.Equals(Domain.Enums.Operacao.Inclusão_Pela_Chefia.ToString()))
                {
                    MontarEEnviarNotificacaoInclusaoPelaChefiaPacto(pactoBuscado);
                }
                else if (oper.Equals(Domain.Enums.Operacao.Suspensão.ToString()))
                {
                    MontarEEnviarNotificacaoSuspensaoPacto(pactoBuscado, usuarioLogado);
                }
                else if (oper.Equals(Domain.Enums.Operacao.Interrupção.ToString()))
                {
                    MontarEEnviarNotificacaoInterrupcaoPacto(pactoBuscado, usuarioLogado);
                }
                else if (oper.Equals(Domain.Enums.Operacao.Alteração.ToString()))
                {
                    if (pactoBuscado.IdSituacaoPactoAnteriorAcao == (int)eSituacaoPacto.EmAndamento)
                    {
                        MontarEEnviarNotificacaoAlteracaoEmAndamentoPacto(pactoBuscado);
                    }
                    else
                    {
                        MontarEEnviarNotificacaoAlteracaoPacto(pactoBuscado);
                    }

                }
                else if (oper.Equals(Domain.Enums.Operacao.Assinatura.ToString()))
                {
                    if (pactoBuscado.IdSituacaoPactoAnteriorAcao == (int)eSituacaoPacto.EmAndamento)
                    {
                        MontarEEnviarNotificacaoAlteracaoEmAndamentoPacto(pactoBuscado);
                    }
                    else
                    {
                        MontarEEnviarNotificacaoAssinaturaPactoPelaChefia(pactoBuscado);
                    }
                }
                else if (oper.Equals(Domain.Enums.Operacao.Avaliacao.ToString()))
                {
                    MontarEEnviarNotificacaoAvaliacaoPactoPelaChefia(pactoBuscado, usuarioLogado);
                }
                else if (oper.Equals(Domain.Enums.Operacao.AvaliacaoParcial.ToString()))
                {
                    MontarEEnviarNotificacaoAvaliacaoParcialPactoPelaChefia(pactoBuscado, usuarioLogado, apvm);
                }
                else if (oper.Equals(Domain.Enums.Operacao.VoltandoSuspensão.ToString()))
                {
                    if (pactoBuscado.IdSituacaoPacto == (int)eSituacaoPacto.EmAndamento)
                    {
                        MontarEEnviarNotificacaoVoltandoSuspensao(pactoBuscado, usuarioLogado);
                    }
                    else if (pactoBuscado.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAvaliacao)
                    {
                        EnviarEmailNotificacaoFinalizacaoPacto(pactoBuscado);
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                //LogManagerComum.LogarErro(ex, null, " Erro ao enviar notificação da ação de " + oper + " de pacto aos interessados.");
                return false;
            }
        }

        private void MontarEEnviarNotificacaoInclusaoPacto(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            var dirigentes = _usuarioService.ObterDirigentesUnidade(Convert.ToInt32(p.UnidadeExercicio)).ToList();

            // Destinatários dos emails:
            // 1) Dirigentes 
            // 2) Próprio solicitante

            var lstDestinatarios = dirigentes;
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            lstDestinatarios.ForEach(u =>
            {
                bool montouMensagem = true;

                String nomeServidor = p.Nome;
                String numeroPacto = p.IdPacto.ToString();
                String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
                String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

                try
                {
                    mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_INCLUSAO_PACTO,
                                                                nomeServidor,
                                                                "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                                dataInicioPacto,
                                                                dataTerminoPacto,
                                                                tabelaProdutos,
                                                                "<a href='" + linkPacto + "'> aqui </a>"
                                                              );

                    assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_INCLUSAO_PACTO,
                                                        nomeServidor,
                                                        numeroPacto
                                                        );
                    destinatario = u.Email;
                }
                catch (Exception ex)
                {
                    //LogManagerComum.LogarErro(ex, null, " Erro ao montar email de inclusão do pacto = " + numeroPacto);
                    montouMensagem = false;
                }

                EnviarEmail(assunto, mensagem, destinatario, montouMensagem);
            });

        }

        private void MontarEEnviarNotificacaoInclusaoPelaChefiaPacto(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";


            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            // Destinatários dos emails:
            // 1) Dirigente que fez o pacto 
            // 2) O usuário do pacto

            var lstDestinatarios = new List<Usuario>();

            Usuario chefeQueCriou = Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuarioCriador));
            lstDestinatarios.Add(chefeQueCriou);
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            lstDestinatarios.ForEach(u =>
            {
                bool montouMensagem = true;

                String nomeDirigente = chefeQueCriou.Nome;
                String nomeServidor = p.Nome;
                String numeroPacto = p.IdPacto.ToString();
                String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
                String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

                try
                {
                    mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_INCLUSAO_PACTO_PELA_CHEFIA,
                                                                nomeDirigente,
                                                                "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                                nomeServidor,
                                                                dataInicioPacto,
                                                                dataTerminoPacto,
                                                                tabelaProdutos,
                                                                "<a href='" + linkPacto + "'> aqui </a>"
                                                              );

                    assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_INCLUSAO_PACTO_PELA_CHEFIA,
                                                        nomeDirigente,
                                                        numeroPacto,
                                                        nomeServidor
                                                        );
                    destinatario = u.Email;
                }
                catch (Exception ex)
                {
                    //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de inclusão por chefia do pacto = " + numeroPacto);
                    montouMensagem = false;
                }

                EnviarEmail(assunto, mensagem, destinatario, montouMensagem);
            });

        }

        private void MontarEEnviarNotificacaoAlteracaoPacto(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            var dirigentes = _usuarioService.ObterDirigentesUnidade(Convert.ToInt32(p.UnidadeExercicio)).ToList();

            // Destinatários dos emails:
            // 1) Dirigentes 
            // 2) Próprio solicitante

            var lstDestinatarios = dirigentes;
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            lstDestinatarios.ForEach(u =>
            {
                bool montouMensagem = true;

                String nomeServidor = p.Nome;
                String numeroPacto = p.IdPacto.ToString();
                String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
                String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

                try
                {
                    mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_ALTERACAO_PACTO,
                                                                "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                                nomeServidor,
                                                                dataInicioPacto,
                                                                dataTerminoPacto,
                                                                tabelaProdutos,
                                                                "<a href='" + linkPacto + "'> aqui </a>"
                                                              );

                    assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_ALTERACAO_PACTO,
                                                        numeroPacto,
                                                        nomeServidor
                                                        );
                    destinatario = u.Email;
                }
                catch (Exception ex)
                {
                    //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de alteração do pacto = " + numeroPacto);
                    montouMensagem = false;
                }

                EnviarEmail(assunto, mensagem, destinatario, montouMensagem);
            });

        }

        private void MontarEEnviarNotificacaoAlteracaoEmAndamentoPacto(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            var dirigentes = _usuarioService.ObterDirigentesUnidade(Convert.ToInt32(p.UnidadeExercicio)).ToList();

            // Destinatários dos emails:
            // 1) Dirigentes 
            // 2) Próprio solicitante

            var lstDestinatarios = dirigentes;
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            lstDestinatarios.ForEach(u =>
            {
                bool montouMensagem = true;

                String nomeDirigente = u.Nome;
                String nomeServidor = p.Nome;
                String numeroPacto = p.IdPacto.ToString();
                String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
                String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

                try
                {
                    mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_ALTERACAO_PACTO_EM_ANDAMENTO,
                                                                "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                                nomeServidor,
                                                                dataInicioPacto,
                                                                dataTerminoPacto,
                                                                tabelaProdutos,
                                                                "<a href='" + linkPacto + "'> aqui </a>"
                                                              );

                    assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_ALTERACAO_PACTO,
                                                        numeroPacto,
                                                        nomeServidor
                                                        );
                    destinatario = u.Email;
                }
                catch (Exception ex)
                {
                    //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de alteração do pacto = " + numeroPacto);
                    montouMensagem = false;
                }

                EnviarEmail(assunto, mensagem, destinatario, montouMensagem);
            });

        }

        private void MontarEEnviarNotificacaoSuspensaoPacto(PactoViewModel p, UsuarioViewModel usuarioLogado)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            var dirigentes = _usuarioService.ObterDirigentesUnidade(Convert.ToInt32(p.UnidadeExercicio)).ToList();

            // Destinatários dos emails:
            // 1) Dirigentes 
            // 2) Próprio solicitante

            var lstDestinatarios = dirigentes;
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            lstDestinatarios.ForEach(u =>
            {
                bool montouMensagem = true;

                Usuario chefeQueSuspendeu = Mapper.Map<UsuarioViewModel, Usuario>(usuarioLogado);
                String dataSuspensao = p.SuspensoAPartirDe?.ToString("dd/MM/yyyy");
                String motivo = p.Motivo;
                String nomeServidor = p.Nome;
                String numeroPacto = p.IdPacto.ToString();
                String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
                String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

                try
                {
                    mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_SUSPENSAO_PACTO,
                                                                "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                                nomeServidor,
                                                                chefeQueSuspendeu.Nome,
                                                                dataInicioPacto,
                                                                dataTerminoPacto,
                                                                dataSuspensao,
                                                                motivo,
                                                                tabelaProdutos,
                                                                "<a href='" + linkPacto + "'> aqui </a>"
                                                              );

                    assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_SUSPENSAO_PACTO,
                                                        numeroPacto,
                                                        nomeServidor,
                                                        chefeQueSuspendeu.Nome
                                                        );
                    destinatario = u.Email;
                }
                catch (Exception ex)
                {
                    //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de suspensão do pacto = " + numeroPacto);
                    throw;

                }

                EnviarEmail(assunto, mensagem, destinatario, montouMensagem);

            });

        }

        private void MontarEEnviarNotificacaoAvaliacaoPactoPelaChefia(PactoViewModel p, UsuarioViewModel usuarioLogado)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;


            // Destinatários dos emails:
            //1) Próprio solicitante

            var solicitante = Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario));


            bool montouMensagem = true;

            Usuario chefeQueAvaliou = Mapper.Map<UsuarioViewModel, Usuario>(usuarioLogado);
            String dataAvaliacao = DateTime.Today.ToString("dd/MM/yyyy");
            String nomeServidor = p.Nome;
            String numeroPacto = p.IdPacto.ToString();
            String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
            String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

            try
            {
                mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_AVALIACAO_PACTO_PELA_CHEFIA,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            chefeQueAvaliou.Nome,
                                                            dataInicioPacto,
                                                            dataTerminoPacto,
                                                            dataAvaliacao,
                                                            tabelaProdutos,
                                                            numeroPacto,
                                                            "<a href='" + linkPacto + "'> aqui </a>"
                                                          );

                assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_AVALIACAO_PACTO_PELA_CHEFIA,
                                                    numeroPacto,
                                                    nomeServidor,
                                                    chefeQueAvaliou.Nome
                                                    );
                destinatario = solicitante.Email;
            }
            catch (Exception ex)
            {
                //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de suspensão do pacto = " + numeroPacto);
                throw;

            }

            EnviarEmail(assunto, mensagem, destinatario, montouMensagem);



        }

        private void MontarEEnviarNotificacaoAvaliacaoParcialPactoPelaChefia(PactoViewModel p, UsuarioViewModel usuarioLogado, AvaliacaoProdutoViewModel apvm)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            var solicitante = Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario));

            bool montouMensagem = true;

            Usuario chefeQueAvaliou = Mapper.Map<UsuarioViewModel, Usuario>(usuarioLogado);
            String nomeServidor = p.Nome;
            String numeroPacto = p.IdPacto.ToString();
            String dataAvaliacaoPacto = DateTime.Now.ToString("dd/MM/yyyy");
            String horaAvaliacaoPacto = DateTime.Now.ToString("hh:mm");

            try
            {


                mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_AVALIACAO_PARCIAL_PACTO_PELA_CHEFIA,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            chefeQueAvaliou.Nome, chefeQueAvaliou.NomeUnidade,
                                                            dataAvaliacaoPacto,
                                                            horaAvaliacaoPacto,
                                                            apvm.QuantidadeProdutosAvaliados, apvm.Produto.Atividade.NomAtividade,
                                                            apvm.CargaHorariaAvaliadaFormatada, apvm.Avaliacao,
                                                            "<a href='" + linkPacto + "'> aqui </a>"
                                                          );

                assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_AVALIACAO_PARCIAL_PACTO_PELA_CHEFIA,
                                                    numeroPacto,
                                                    nomeServidor,
                                                    chefeQueAvaliou.Nome
                                                    );
                destinatario = solicitante.Email;
            }
            catch (Exception ex)
            {
                //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de suspensão do pacto = " + numeroPacto);
                throw;

            }

            EnviarEmail(assunto, mensagem, destinatario, montouMensagem);



        }

        private void MontarEEnviarNotificacaoInterrupcaoPacto(PactoViewModel p, UsuarioViewModel usuarioLogado)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;


            // Destinatários dos emails:
            //1) Próprio solicitante

            var solicitante = Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario));


            bool montouMensagem = true;

            Usuario chefeQueInterrompeu = Mapper.Map<UsuarioViewModel, Usuario>(usuarioLogado);
            String dataInterrupcao = p.DataInterrupcao?.ToString("dd/MM/yyyy");
            String motivo = p.Motivo;
            String nomeServidor = p.Nome;
            String numeroPacto = p.IdPacto.ToString();
            String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
            String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

            try
            {
                mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_INTERRUPCAO_PACTO,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            chefeQueInterrompeu.Nome,
                                                            dataInicioPacto,
                                                            dataTerminoPacto,
                                                            dataInterrupcao,
                                                            motivo,
                                                            tabelaProdutos,
                                                            numeroPacto,
                                                            "<a href='" + linkPacto + "'> aqui </a>"
                                                          );

                assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_INTERRUPCAO_PACTO,
                                                    numeroPacto,
                                                    nomeServidor,
                                                    chefeQueInterrompeu.Nome
                                                    );
                destinatario = solicitante.Email;
            }
            catch (Exception ex)
            {
                //LogManagerComum.LogarErro(ex, null, "Email ao montar email de interrupção do pacto = " + numeroPacto);
                throw;

            }

            EnviarEmail(assunto, mensagem, destinatario, montouMensagem);



        }

        private void MontarEEnviarNotificacaoAssinaturaPactoPelaChefia(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;


            // Destinatários dos emails:
            //1) Próprio solicitante

            var solicitante = Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario));


            bool montouMensagem = true;

            String nomeServidor = p.Nome;
            String numeroPacto = p.IdPacto.ToString();
            String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
            String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

            try
            {
                mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_ASSINATURA_PACTO_PELA_CHEFIA,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            dataInicioPacto,
                                                            dataTerminoPacto,
                                                            tabelaProdutos,
                                                            "<a href='" + linkPacto + "'> aqui </a>"
                                                          );

                assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_ASSINATURA_PACTO_PELA_CHEFIA,
                                                    numeroPacto,
                                                    nomeServidor
                                                    );
                destinatario = solicitante.Email;
            }
            catch (Exception ex)
            {
                //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de interrupção do pacto = " + numeroPacto);
                throw;

            }

            EnviarEmail(assunto, mensagem, destinatario, montouMensagem);
        }

        private void MontarEEnviarNotificacaoVoltandoSuspensao(PactoViewModel p, UsuarioViewModel usuarioLogado)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            String tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;

            // Destinatários dos emails:
            //1) Próprio solicitante

            var solicitante = Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario));


            bool montouMensagem = true;

            Usuario chefeQueReiniciou = Mapper.Map<UsuarioViewModel, Usuario>(usuarioLogado);
            String dataSuspensao = p.SuspensoAPartirDe?.ToString("dd/MM/yyyy");
            String nomeServidor = p.Nome;
            String numeroPacto = p.IdPacto.ToString();
            String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
            String dataTerminoPacto = p.DataPrevistaTerminoAntesSuspensao?.ToString("dd/MM/yyyy");
            String dataInicioPactoAposReativacao = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
            String dataTerminoPactoAposReativacao = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

            try
            {
                mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_PACTO_REINICIADO_APOS_SUSPENSAO,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            chefeQueReiniciou.Nome,
                                                            dataInicioPacto,
                                                            dataTerminoPacto,
                                                            dataSuspensao,
                                                            dataInicioPactoAposReativacao,
                                                            dataTerminoPactoAposReativacao,
                                                            tabelaProdutos,
                                                            "<a href='" + linkPacto + "'> aqui </a>",
                                                            DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm:ss"),
                                                            p.SituacaoPactoDescricao
                                                          );

                assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_PACTO_REINICIADO_APOS_SUSPENSAO,
                                                    numeroPacto,
                                                    nomeServidor,
                                                    chefeQueReiniciou.Nome
                                                    );
                destinatario = solicitante.Email;
            }
            catch (Exception ex)
            {
                //LogManagerComum.LogarErro(ex, null, "Erro ao montar email de reinício após suspensão do pacto = " + numeroPacto);
                throw;

            }

            EnviarEmail(assunto, mensagem, destinatario, montouMensagem);
        }


 

        public void EnviarEmailNotificacaoFinalizacaoPacto(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";

            string tabelaProdutos = MontarTextoTabelaProdutos(p);
            string linkPacto = ObterLinkPacto(p);


            List<Usuario> lstDestinatarios = _usuarioService.ObterDirigentesUnidade(Convert.ToInt32(p.UnidadeExercicio)).ToList();
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            bool montouMensagem = true;

            lstDestinatarios.ForEach(u =>
            {
                String nomeDirigente = u.Nome;
                String nomeServidor = p.Nome;
                String numeroPacto = p.IdPacto.ToString();
                String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
                String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");

                try
                {
                    mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_FIM_PACTO,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            dataInicioPacto,
                                                            dataTerminoPacto,
                                                            tabelaProdutos,
                                                            "<a href='" + linkPacto + "'> aqui </a>"
                                                          );

                    assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_FIM_PACTO,
                                                        numeroPacto,
                                                        nomeServidor);
                    destinatario = u.Email;
                }
                catch (Exception ex)
                {
                    // LogManagerComum.LogarErro(ex, null, "Email ao montar email de finalização do pacto = " + numeroPacto);
                    montouMensagem = false;
                }

                try
                {
                    if (montouMensagem)
                    {
                        new EmailCGU().EnviarEmail(mensagem, assunto, destinatario);
                    }
                }
                catch (Exception ex)
                {
                    // LogManagerComum.LogarErro(ex, null, " Email não enviado. Detalhe: Para: " + destinatario + "\n Assunto: " + assunto + "\n Mensagem: " + mensagem);
                }

            });
        }

        public void EnviarEmailNotificacaoReativacaoAutomaticaPacto(PactoViewModel p)
        {
            String assunto = "";
            String mensagem = "";
            String destinatario = "";
            string tabelaProdutos = MontarTextoTabelaProdutos(p);
            String linkPacto = System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() + "Pacto/Solicitar/" + p.IdPacto;
            List<Usuario> lstDestinatarios = _usuarioService.ObterDirigentesUnidade(Convert.ToInt32(p.UnidadeExercicio)).ToList();
            lstDestinatarios.Add(Mapper.Map<UsuarioViewModel, Usuario>(_usuarioAppService.ObterPorCPF(p.CpfUsuario)));

            String dataSuspensao = p.SuspensoAPartirDe?.ToString("dd/MM/yyyy");
            String nomeServidor = p.Nome;
            String numeroPacto = p.IdPacto.ToString();
            String dataInicioPacto = p.DataPrevistaInicio.ToString("dd/MM/yyyy");
            String dataTerminoPacto = p.DataPrevistaTermino?.ToString("dd/MM/yyyy");
            String situacaoPacto = _situacaoPactoService.ObterPorId(p.IdSituacaoPacto)?.DescSituacaoPacto;

            try
            {
                mensagem = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.CORPO_EMAIL_NOTIFICACAO_PACTO_REINICIADO_AUTOMATICAMENTE,
                                                            "<a href='" + linkPacto + "'>" + numeroPacto + "</a>",
                                                            nomeServidor,
                                                            DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm:ss"),
                                                            dataInicioPacto,
                                                            dataTerminoPacto,
                                                            dataSuspensao,
                                                            tabelaProdutos,
                                                            situacaoPacto,
                                                            "<a href='" + linkPacto + "'> aqui </a>");

                assunto = String.Format(PGD.Infra.CrossCutting.Util.Properties.Resources.ASSUNTO_EMAIL_NOTIFICACAO_PACTO_REINICIADO_AUTOMATICAMENTE,
                                                    numeroPacto,
                                                    nomeServidor);

                string destinatarios = string.Join(";", lstDestinatarios.Select(u => u.Email).ToList());
                try
                {
                    new EmailCGU().EnviarEmail(mensagem, assunto, destinatarios);
                }
                catch (Exception ex)
                {
                    // Nada a fazer, apenas loga. Futuramente podem-se armazenar os emails que falharam e tentar reenviar
                    // LogManagerComum.LogarErro(ex, null, " Email não enviado. Detalhe: Para: " + destinatario + "\n Assunto: " + assunto + "\n Mensagem: " + mensagem);
                }
            }
            catch (Exception ex)
            {
                // LogManagerComum.LogarErro(ex, null, " Email ao montar email de reativação automática do pacto = " + numeroPacto);
            }


        }

        private static void EnviarEmail(string assunto, string mensagem, string destinatario, bool montouMensagem)
        {
            try
            {
                if (montouMensagem)
                {
                    new EmailCGU().EnviarEmail(mensagem, assunto, destinatario);
                }
            }
            catch (Exception ex)
            {
                // Nada a fazer, apenas loga. Futuramente podem-se armazenar os emails que falharam e tentar reenviar
                // LogManagerComum.LogarErro(ex, null, " Email não enviado. Detalhe: Para: " + destinatario + "\n Assunto: " + assunto + "\n Mensagem: " + mensagem);
                throw;
            }
        }

 
        private static string ObterLinkPacto(PactoViewModel p)
        {
            return System.Configuration.ConfigurationManager.AppSettings["URL_PGD"].ToString() +
                $"Pacto/AvaliarProduto?idPacto={p.IdPacto}&idOrigemAcao={(int)PGD.Domain.Enums.eOrigem.Listagem}";
        }

        private static string MontarTextoTabelaProdutos(PactoViewModel p)
        {
            List<string> lstDadosProdutos = new List<string>();

            p.Produtos.ToList().ForEach(prod =>
            {

                var tsCarga = TimeSpan.FromHours((double)prod.CargaHorariaProduto * prod.QuantidadeProduto);
                string minutes = tsCarga.Minutes < 10 ? "0" + tsCarga.Minutes : tsCarga.Minutes.ToString();
                string cargaHorariaProdutoFormatada = $"{Math.Floor(tsCarga.TotalHours)}:{minutes}";

                StringBuilder strDadosProdutos = new StringBuilder();
                strDadosProdutos.Append(prod.GrupoAtividade.NomGrupoAtividade)
                                .Append(";")
                                .Append(prod.Atividade.NomAtividade)
                                .Append(";")
                                .Append(prod.TipoAtividade.Faixa)
                                .Append(";")
                                .Append(prod.QuantidadeProduto)
                                .Append(";")
                                .Append(cargaHorariaProdutoFormatada);

                lstDadosProdutos.Add(strDadosProdutos.ToString());

            });

            String tabelaProdutos = MontarTabelaDadosProdutos(lstDadosProdutos);
            return tabelaProdutos;
        }


 

        private static String MontarTabelaDadosProdutos(List<String> lstDadosProdutos, String titulo = null)
        {
            try
            {
                if (lstDadosProdutos.Any())
                {

                    string htmlTitle = String.Empty;
                    if (!String.IsNullOrEmpty(titulo))
                    {
                        htmlTitle = "<h3>" + titulo + "</h3>";
                    }

                    string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                    string htmlTableEnd = "</table>";
                    string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
                    string htmlHeaderRowEnd = "</tr>";
                    string htmlTrStart = "<tr style =\"color:#555555;\">";
                    string htmlTrEnd = "</tr>";
                    string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
                    string htmlTdEnd = "</td>";

                    StringBuilder messageBody = new StringBuilder(htmlTitle + htmlTableStart);

                    if (lstDadosProdutos.Any())
                    {
                        messageBody.Append(htmlHeaderRowStart);
                    }

                    String colunas = "Grupo de atividades;Atividade;Faixa;Qtde de Produtos;Carga Horária";
                    messageBody.Append(htmlTdStart + String.Join(htmlTdEnd + htmlTdStart, colunas.Split(';')) + htmlTdEnd);
                      

                    if (lstDadosProdutos.Any())
                    {
                        messageBody.Append(htmlHeaderRowEnd);
                    }

                    foreach (String strDadosProdutos in lstDadosProdutos)
                    {
                        messageBody.Append(htmlTrStart);

                        messageBody.Append(htmlTdStart + String.Join( htmlTdEnd + htmlTdStart, strDadosProdutos.Split(';'))  + htmlTdEnd);

                        messageBody.Append(htmlTrEnd);
                    }

                    messageBody.Append(htmlTableEnd);


                    return messageBody.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                // LogManagerComum.LogarErro(ex);
                throw new InvalidOperationException("Não foi possível montar a tabela com os produtos"); ;
            }
        }
    }
}
