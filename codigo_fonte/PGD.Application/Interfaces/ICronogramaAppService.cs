using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface ICronogramaAppService
    {
        List<CronogramaViewModel> CalcularCronogramas(double horasTotais, TimeSpan horasDiarias,
            DateTime dataInicial, string CPFUsuario, UsuarioViewModel usuarioLogado, int idPacto = 0,
            DateTime? dataInicioSuspensao = null, DateTime? dataFimSuspensao = null,
            CronogramaPactoViewModel cronogramaExistente = null, TimeSpan? quantidadeHorasDiasSuspensao = null);
        CronogramaViewModel CriarDiaCronograma(DateTime dataCronograma,
            TimeSpan cargaHorariaRestantePacto, TimeSpan cargaHorariaDiaria,
            List<PactoViewModel> pactosConcorrentes,
            DateTime? dataInicioSuspensao = null, DateTime? dataFimSuspensao = null,
            UsuarioViewModel usuarioLogado = null, string cpfSolicitante = null);
        List<CronogramaViewModel> CalcularAumentoCargaHoraria(CronogramaPactoViewModel cronogramaPactoReferencia, double novaCargaHoraria,
            List<Pacto> listaPactosConcorrentes, string cpfSolicitante, UsuarioViewModel usuarioLogado);
        List<CronogramaViewModel> CalcularDiminuicaoCargaHoraria(CronogramaPactoViewModel cronogramaPactoReferencia, double novaCargaHoraria,
            List<Pacto> listaPactosConcorrentes, string cpfSolicitante, UsuarioViewModel usuarioLogado, bool podeEditarDiasPassados = false);
        List<CronogramaViewModel> CalcularInclusaoSuspensao(CronogramaPactoViewModel cronogramaPactoReferencia, DateTime dataInicioSuspensao, string cpfSolicitante, UsuarioViewModel usuarioLogado,
            DateTime? dataFimSuspensao = null, TimeSpan? horasConsideradasNaDataSuspensao = null);
        void ValidarCronograma(CronogramaPactoViewModel cronogramaPactoVM);
        bool PodeEditarDiaCronograma(string cpfSolicitantePacto, UsuarioViewModel usuarioLogado, DateTime dataCronograma);
        double GetQuantidadeHorasNoDia(DateTime dataCronograma, List<PactoViewModel> pactosConcorrentes);

        void LimparDiasZerados(List<CronogramaViewModel> cronogramas);
    }
}
