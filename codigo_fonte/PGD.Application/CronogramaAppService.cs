using AutoMapper;
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
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application
{
    public class CronogramaAppService : ApplicationService, ICronogramaAppService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IFeriadoService _feriadoService;
        private readonly ICronogramaService _cronogramaService;
        private readonly IPactoService _pactoService;

        public CronogramaAppService(IUsuarioService usuarioService, IUnitOfWork uow, IFeriadoService feriadoService,
            ICronogramaService cronogramaService, IPactoService pactoService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _feriadoService = feriadoService;
            _cronogramaService = cronogramaService;
            _pactoService = pactoService;
        }

        public List<CronogramaViewModel> CalcularCronogramas(double horasTotais, TimeSpan horasDiarias,
            DateTime dataInicial, string CPFUsuario, UsuarioViewModel usuarioLogado, int idPacto = 0, 
            DateTime? dataInicioSuspensao = null, DateTime? dataFimSuspensao = null,
            CronogramaPactoViewModel cronogramaExistente = null, TimeSpan? quantidadeHorasDiasSuspensao = null)
        {
            List<CronogramaViewModel> cronogramas;

            var listaPactosConcorrentes = _pactoService.GetPactosConcorrentes(dataInicial, DateTime.Now.AddDays(365), CPFUsuario, idPacto);

            if (cronogramaExistente != null && cronogramaExistente.Cronogramas.Count > 0
                                && cronogramaExistente.DataInicial.Date == dataInicial.Date
                                && horasDiarias == cronogramaExistente.HorasDiarias
                                && cronogramaExistente.QuantidadeHorasDiasSuspensao == quantidadeHorasDiasSuspensao
                                && cronogramaExistente.DataFimSuspensao.GetValueOrDefault().ToShortDateString() == dataFimSuspensao.GetValueOrDefault().ToShortDateString()
                                && cronogramaExistente.DataInicioSuspensao.GetValueOrDefault().ToShortDateString() == dataInicioSuspensao.GetValueOrDefault().ToShortDateString())
                                
            {
                //Se cronograma existente e usuario tentando alterar carga horaria.
                if (horasTotais != cronogramaExistente.HorasTotais)
                {
                    //Se é preciso acrescentar carga horária
                    if (horasTotais > cronogramaExistente.HorasTotais)
                    {
                        cronogramas = CalcularAumentoCargaHoraria(cronogramaExistente, horasTotais, listaPactosConcorrentes: listaPactosConcorrentes, cpfSolicitante: CPFUsuario, usuarioLogado: usuarioLogado);

                    }
                    //Se é preciso retirar horas da carga anterior
                    else
                    {
                        cronogramas = CalcularDiminuicaoCargaHoraria(cronogramaExistente, horasTotais, listaPactosConcorrentes: listaPactosConcorrentes, cpfSolicitante: CPFUsuario, usuarioLogado: usuarioLogado);
                    }

                }
                else 
                {
                    //Mantem o que está no tempdata sem recalcular
                    cronogramas = cronogramaExistente.Cronogramas;
                }
            }
            else
            {
                //Calcula do zero
                if (dataInicioSuspensao.HasValue)
                {
                    cronogramas = CalcularInclusaoSuspensao(cronogramaExistente, dataInicioSuspensao: dataInicioSuspensao.GetValueOrDefault(), dataFimSuspensao: dataFimSuspensao.GetValueOrDefault(), horasConsideradasNaDataSuspensao: quantidadeHorasDiasSuspensao, cpfSolicitante: CPFUsuario, usuarioLogado: usuarioLogado);

                }
                else
                {
                    cronogramas = GerarCronogramas(dataInicio: dataInicial,
                                                                cargaHorariaTotalPacto: TimeSpan.FromHours(horasTotais),
                                                                cargaHorariaDiaria: horasDiarias,
                                                                dataInicioSuspensao: dataInicioSuspensao,
                                                                dataFimSuspensao: dataFimSuspensao,
                                                                quantidadeHorasDiasSuspensao: quantidadeHorasDiasSuspensao,
                                                                listaPactosConcorrentes: listaPactosConcorrentes,
                                                                cpfSolicitante: CPFUsuario, usuarioLogado: usuarioLogado, idPacto: idPacto);
                }
            }

            return cronogramas;
        }

        public void LimparDiasZerados(List<CronogramaViewModel> cronogramas)
        {
            var ultimoDiaComHoras = cronogramas.OrderByDescending(c => c.DataCronograma).First(c => c.HorasCronograma.Hours > 0).DataCronograma;
            var cronogramasARemover = cronogramas.Where(c => c.HorasCronograma.Hours == 0 && c.DataCronograma > ultimoDiaComHoras);
            cronogramasARemover.ToList().ForEach(c => cronogramas.Remove(c));
        }

        private List<CronogramaViewModel> GerarCronogramas(DateTime dataInicio, 
            TimeSpan cargaHorariaTotalPacto, TimeSpan cargaHorariaDiaria,
            List<Pacto> listaPactosConcorrentes, string cpfSolicitante, UsuarioViewModel usuarioLogado,
            DateTime? dataInicioSuspensao = null, DateTime? dataFimSuspensao = null, 
            TimeSpan? quantidadeHorasDiasSuspensao = null, int idPacto = 0)
        {
            var resultado = new List<CronogramaViewModel>();

            DateTime tmpData = dataInicio.AddDays(-1);
            
            
            while (cargaHorariaTotalPacto > TimeSpan.Zero)
            {
                tmpData = tmpData.AddDays(1);
                CronogramaViewModel diaCronograma = CriarDiaCronograma(dataCronograma: tmpData,
                                                                       cargaHorariaRestantePacto: cargaHorariaTotalPacto,
                                                                       cargaHorariaDiaria: cargaHorariaDiaria,
                                                                       dataInicioSuspensao: dataInicioSuspensao,
                                                                       dataFimSuspensao: dataFimSuspensao,
                                                                       pactosConcorrentes: listaPactosConcorrentes,
                                                                       quantidadeHorasDiasSuspensao: quantidadeHorasDiasSuspensao,
                                                                       cpfSolicitante: cpfSolicitante, usuarioLogado: usuarioLogado);
                diaCronograma.IdPacto = idPacto;


                resultado.Add(diaCronograma);

                cargaHorariaTotalPacto -= diaCronograma.HorasCronograma;
            }

            resultado.ForEach(c => c.PodeEditar = PodeEditarDiaCronograma(cpfSolicitante, usuarioLogado, c.DataCronograma));

            return resultado;
        }

        public CronogramaViewModel CriarDiaCronograma(DateTime dataCronograma,
            TimeSpan cargaHorariaRestantePacto, TimeSpan cargaHorariaDiaria,
            List<PactoViewModel> pactosConcorrentes,
            DateTime? dataInicioSuspensao = null, DateTime? dataFimSuspensao = null,
            UsuarioViewModel usuarioLogado = null, string cpfSolicitante = null)
        {
            var pactos = Mapper.Map<List<PactoViewModel>, List<Pacto>>(pactosConcorrentes);

            return CriarDiaCronograma(dataCronograma, cargaHorariaRestantePacto, cargaHorariaDiaria,
                                      pactos, cpfSolicitante, usuarioLogado, dataInicioSuspensao, dataFimSuspensao);
        }

        private CronogramaViewModel CriarDiaCronograma(DateTime dataCronograma, 
            TimeSpan cargaHorariaRestantePacto, TimeSpan cargaHorariaDiaria,
            List<Pacto> pactosConcorrentes, string cpfSolicitante, UsuarioViewModel usuarioLogado,
            DateTime? dataInicioSuspensao = null, DateTime? dataFimSuspensao = null, 
            TimeSpan? quantidadeHorasDiasSuspensao = null)
        {
            var diaFeriado = _feriadoService.ObterFeriado(dataCronograma.Date);
            TimeSpan duracaoFeriado = TimeSpan.Zero;
            TimeSpan horasDiaCronograma = cargaHorariaDiaria;
            TimeSpan horasUsadasPorOutroPacto = TimeSpan.Zero;

            bool fimDeSemana = dataCronograma.DayOfWeek == DayOfWeek.Saturday || dataCronograma.DayOfWeek == DayOfWeek.Sunday;
            bool diaSuspenso = IsDiaCronogramaSuspenso(dataCronograma, dataInicioSuspensao, dataFimSuspensao);
            double qtdHorasOcupadasOutroPacto = GetQuantidadeHorasNoDia(dataCronograma, pactosConcorrentes);

            bool podeEditar = PodeEditarDiaCronograma(cpfSolicitante, usuarioLogado, dataCronograma);

            if (fimDeSemana)
            {
                horasDiaCronograma = TimeSpan.Zero;
            }
            else
            {
                AvaliarDiaCronograma(dataCronograma, cargaHorariaDiaria, dataInicioSuspensao, dataFimSuspensao, quantidadeHorasDiasSuspensao, diaFeriado, ref duracaoFeriado, ref horasDiaCronograma, ref horasUsadasPorOutroPacto, diaSuspenso, qtdHorasOcupadasOutroPacto);
            }

            return PopularDiaCronograma(dataCronograma, cargaHorariaRestantePacto, podeEditar, diaFeriado, duracaoFeriado, horasDiaCronograma, horasUsadasPorOutroPacto, fimDeSemana, diaSuspenso);
        }

        private static void AvaliarDiaCronograma(DateTime dataCronograma, TimeSpan cargaHorariaDiaria, DateTime? dataInicioSuspensao, DateTime? dataFimSuspensao, TimeSpan? quantidadeHorasDiasSuspensao, Feriado diaFeriado, ref TimeSpan duracaoFeriado, ref TimeSpan horasDiaCronograma, ref TimeSpan horasUsadasPorOutroPacto, bool diaSuspenso, double qtdHorasOcupadasOutroPacto)
        {
            AvaliarDiaCronogramaFeriado(cargaHorariaDiaria, diaFeriado, ref duracaoFeriado, ref horasDiaCronograma);

            if (qtdHorasOcupadasOutroPacto > 0)
            {
                horasUsadasPorOutroPacto = TimeSpan.FromHours(Convert.ToDouble(qtdHorasOcupadasOutroPacto));
                horasDiaCronograma = (horasDiaCronograma - horasUsadasPorOutroPacto) > TimeSpan.Zero ? horasDiaCronograma - horasUsadasPorOutroPacto : TimeSpan.Zero;
            }

            horasDiaCronograma = AvaliarDiaCronogramaSuspenso(dataCronograma, dataInicioSuspensao, dataFimSuspensao, quantidadeHorasDiasSuspensao, horasDiaCronograma, diaSuspenso);
        }

        private static TimeSpan AvaliarDiaCronogramaSuspenso(DateTime dataCronograma, DateTime? dataInicioSuspensao, DateTime? dataFimSuspensao, TimeSpan? quantidadeHorasDiasSuspensao, TimeSpan horasDiaCronograma, bool diaSuspenso)
        {
            if (diaSuspenso)
            {
                if (dataCronograma.Date == dataInicioSuspensao.Value.Date)
                {
                    horasDiaCronograma = quantidadeHorasDiasSuspensao.GetValueOrDefault();
                }
                else if (dataCronograma.Date < dataFimSuspensao?.Date)
                {
                    horasDiaCronograma = TimeSpan.Zero;
                }

            }

            return horasDiaCronograma;
        }

        private static void AvaliarDiaCronogramaFeriado(TimeSpan cargaHorariaDiaria, Feriado diaFeriado, ref TimeSpan duracaoFeriado, ref TimeSpan horasDiaCronograma)
        {
            if (diaFeriado != null)
            {
                duracaoFeriado = string.IsNullOrWhiteSpace(diaFeriado.duracao) ? cargaHorariaDiaria : TimeSpan.Parse(diaFeriado.duracao);
                horasDiaCronograma = cargaHorariaDiaria > TimeSpan.Zero ? cargaHorariaDiaria - duracaoFeriado : TimeSpan.Zero;
                if (horasDiaCronograma < TimeSpan.Zero)
                    horasDiaCronograma = TimeSpan.Zero;
                horasDiaCronograma = (cargaHorariaDiaria - duracaoFeriado) > TimeSpan.Zero ? cargaHorariaDiaria - duracaoFeriado : TimeSpan.Zero;
            }
        }

        private CronogramaViewModel PopularDiaCronograma(DateTime dataCronograma, TimeSpan cargaHorariaRestantePacto, bool podeEditar, Feriado diaFeriado, TimeSpan duracaoFeriado, TimeSpan horasDiaCronograma, TimeSpan horasUsadasPorOutroPacto, bool fimDeSemana, bool diaSuspenso)
        {
            return new CronogramaViewModel
            {
                DataCronograma = dataCronograma,
                HorasCronograma = cargaHorariaRestantePacto < horasDiaCronograma ? cargaHorariaRestantePacto : horasDiaCronograma,
                DiaUtil = !fimDeSemana && diaFeriado == null,
                Feriado = (diaFeriado != null),
                DuracaoFeriado = duracaoFeriado,
                Suspenso = diaSuspenso,
                PodeEditar = podeEditar,
                HorasUsadasPorOutroPacto = horasUsadasPorOutroPacto
            };
        }

        public double GetQuantidadeHorasNoDia(DateTime dataCronograma, List<PactoViewModel> pactosConcorrentes)
        {
            var pactos = Mapper.Map<List<PactoViewModel>, List<Pacto>>(pactosConcorrentes);
            return GetQuantidadeHorasNoDia(dataCronograma, pactos);
        }

        private double GetQuantidadeHorasNoDia(DateTime dataCronograma, List<Pacto> pactosConcorrentes)
        {
            return pactosConcorrentes.Select(p => p.Cronogramas.Where(c => dataCronograma.Date == c.DataCronograma.Date).FirstOrDefault()).Sum(c => c == null ? 0 : c.HorasCronograma);
        }

        public bool PodeEditarDiaCronograma (string cpfSolicitantePacto, UsuarioViewModel usuarioLogado, DateTime dataCronograma)
        {
            DateTime dataMinimaSuspensaoPacto = _pactoService.ObterDataMinimaSuspensaoPacto();
            DateTime dataReferencia = dataCronograma;
            return dataReferencia >= dataMinimaSuspensaoPacto && (cpfSolicitantePacto == usuarioLogado.CPF || usuarioLogado.IsDirigente);
        }


        private bool IsDiaCronogramaSuspenso(DateTime dataCronograma,
            DateTime? dataInicioSuspensao, DateTime? dataFimSuspensao)
        {
            bool isSuspenso = false;

            isSuspenso = dataInicioSuspensao.HasValue && dataCronograma.Date >= dataInicioSuspensao.Value.Date;
            isSuspenso = dataFimSuspensao.HasValue ? isSuspenso && dataCronograma.Date <= dataFimSuspensao.Value.Date : isSuspenso;

            return isSuspenso;
        }

        public List<CronogramaViewModel> CalcularAumentoCargaHoraria(CronogramaPactoViewModel cronogramaPactoReferencia,
            double novaCargaHoraria,
            List<Pacto> listaPactosConcorrentes, string cpfSolicitante, UsuarioViewModel usuarioLogado)
        {
            List<CronogramaViewModel> lista = new List<CronogramaViewModel>();

            //Descontando o ultimo dia pra preencher de novo com horas full.
            DateTime ultimoDiaPreenchido = cronogramaPactoReferencia.Cronogramas.LastOrDefault().DataCronograma;
            lista.AddRange(cronogramaPactoReferencia.Cronogramas.Take(cronogramaPactoReferencia.Cronogramas.Count - 1));

            double horasAAcrescentar = novaCargaHoraria - lista.Sum(c => c.HorasCronograma.TotalHours);

            lista.AddRange(GerarCronogramas(dataInicio: ultimoDiaPreenchido,
                cargaHorariaTotalPacto: TimeSpan.FromHours(horasAAcrescentar),
                cargaHorariaDiaria: cronogramaPactoReferencia.HorasDiarias,
                cpfSolicitante: cpfSolicitante, usuarioLogado: usuarioLogado,
                dataInicioSuspensao: cronogramaPactoReferencia.DataInicioSuspensao,
                dataFimSuspensao: cronogramaPactoReferencia.DataFimSuspensao, 
                listaPactosConcorrentes: listaPactosConcorrentes, idPacto: cronogramaPactoReferencia.IdPacto));
            return lista;
        }

        public List<CronogramaViewModel> CalcularDiminuicaoCargaHoraria(CronogramaPactoViewModel cronogramaPactoReferencia, 
            double novaCargaHoraria,
            List<Pacto> listaPactosConcorrentes, string cpfSolicitante, UsuarioViewModel usuarioLogado,
            bool podeEditarDiasPassados = false)
        {
            List<CronogramaViewModel> lista = new List<CronogramaViewModel>();

            double totalHorasTemp = 0;
            double horasRestantes = 0;

            lista = cronogramaPactoReferencia.Cronogramas.TakeWhile(c => (totalHorasTemp += c.HorasCronograma.TotalHours) <= novaCargaHoraria).ToList();

            while (lista.Sum(c => c.HorasCronograma.TotalHours) < novaCargaHoraria
                && (!cronogramaPactoReferencia.DataInicioSuspensao.HasValue || cronogramaPactoReferencia.DataFimSuspensao.HasValue)) //testando se o cronograma nao esta suspenso.
            {
                horasRestantes = novaCargaHoraria - lista.Sum(c => c.HorasCronograma.TotalHours);

                var cronogramVM = CriarDiaCronograma(
                    dataCronograma: lista.Last().DataCronograma.AddDays(1),
                    cargaHorariaRestantePacto: TimeSpan.FromHours(horasRestantes),
                    cargaHorariaDiaria: cronogramaPactoReferencia.HorasDiarias,
                    dataInicioSuspensao: cronogramaPactoReferencia.DataInicioSuspensao,
                    dataFimSuspensao: cronogramaPactoReferencia.DataFimSuspensao, 
                    pactosConcorrentes: listaPactosConcorrentes,
                    usuarioLogado: usuarioLogado, cpfSolicitante: cpfSolicitante);

                lista.Add(cronogramVM);
            }
            return lista;
        }


        public List<CronogramaViewModel> CalcularInclusaoSuspensao(CronogramaPactoViewModel cronogramaPactoReferencia, DateTime dataInicioSuspensao, 
            string cpfSolicitante, UsuarioViewModel usuarioLogado, DateTime? dataFimSuspensao = null, TimeSpan? horasConsideradasNaDataSuspensao = null)
        {
            List<CronogramaViewModel> diasAjustados = cronogramaPactoReferencia.Cronogramas.TakeWhile(c => c.DataCronograma.Date < dataInicioSuspensao.Date).ToList();

            if (dataFimSuspensao.HasValue && dataFimSuspensao.Value > DateTime.MinValue)
            {
                var horasRestantes = cronogramaPactoReferencia.HorasTotais - diasAjustados.Sum(c => c.HorasCronograma.TotalHours);
                var listaPactosConcorrentes = _pactoService.GetPactosConcorrentes(dataInicioSuspensao, DateTime.Now.AddDays(365), 
                    cronogramaPactoReferencia.CPFUsuario,
                    cronogramaPactoReferencia.IdPacto);


                diasAjustados.AddRange(GerarCronogramas(dataInicio: dataInicioSuspensao,
                                                            cargaHorariaTotalPacto: TimeSpan.FromHours(horasRestantes),
                                                            cargaHorariaDiaria: cronogramaPactoReferencia.HorasDiarias,
                                                            cpfSolicitante: cpfSolicitante,
                                                            usuarioLogado: usuarioLogado,
                                                            dataInicioSuspensao: dataInicioSuspensao,
                                                            dataFimSuspensao: dataFimSuspensao, 
                                                            listaPactosConcorrentes: listaPactosConcorrentes, quantidadeHorasDiasSuspensao: horasConsideradasNaDataSuspensao, idPacto: cronogramaPactoReferencia.IdPacto));
            }
            else
            {
                var diasSuspensos = cronogramaPactoReferencia.Cronogramas.Where(c => c.DataCronograma.Date >= dataInicioSuspensao.Date).ToList();
                diasSuspensos.ForEach(c => { c.Suspenso = true; c.HorasCronograma = c.DataCronograma.Date == dataInicioSuspensao.Date && horasConsideradasNaDataSuspensao.HasValue ? horasConsideradasNaDataSuspensao.Value : TimeSpan.Zero; });
                diasAjustados.AddRange(diasSuspensos);
            }

            return diasAjustados;
        }
        public void ValidarCronograma(CronogramaPactoViewModel cronogramaPactoVM)
        {
            var cronogramaPacto = Mapper.Map<CronogramaPactoViewModel, CronogramaPacto>(cronogramaPactoVM);

            var pactosConcorrentes = _pactoService.GetPactosConcorrentes(cronogramaPacto.DataInicial, DateTime.Now.AddYears(1), cronogramaPacto.CPFUsuario, cronogramaPacto.IdPacto);

            _cronogramaService.ValidarCronograma(cronogramaPacto, pactosConcorrentes, cronogramaPactoVM.ValidarHorasADistribuir);
            cronogramaPactoVM.ValidationResult = cronogramaPacto.ValidationResult;
        }
    }
}
