using DomainValidation.Validation;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using PGD.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Controllers
{
    public class CronogramaController : BaseController
    {
        IPactoAppService _pactoService;
        ICronogramaAppService _cronogramaService;

        public CronogramaController(IUsuarioAppService usuarioAppService, IPactoAppService pactoservice, ICronogramaAppService cronogramaService)
            : base(usuarioAppService)
        {
            _pactoService = pactoservice;
            _cronogramaService = cronogramaService;
        }


        // GET: Cronograma
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult ExibirCronograma(CronogramaPactoViewModel modelCronograma)
        {
            AtualizarCronograma(modelCronograma);
            return PartialView("_CronogramasPartial", modelCronograma);
        }


        public JsonResult AtualizarCronograma(CronogramaPactoViewModel modelCronograma)
        {
            var pacto = _pactoService.BuscarPorId(modelCronograma.IdPacto);
            CronogramaPactoViewModel cronogramaExistente;
            if (modelCronograma.CalcularCronogramaAPartirBanco && pacto != null)
            {
                cronogramaExistente = new CronogramaPactoViewModel()
                {
                    DataInicial = pacto.DataPrevistaInicio,
                    Cronogramas = pacto.Cronogramas,
                    HorasTotais = Convert.ToDouble(pacto.CargaHorariaTotal),
                    HorasDiarias = pacto.CargaHorariaDiaria,
                    DataInicioSuspensao = pacto.SuspensoAPartirDe,
                    DataFimSuspensao = DateTime.Today,
                    IdPacto = pacto.IdPacto,
                    CPFUsuario = pacto.CpfUsuario
                };
            }
            else
            {
                cronogramaExistente = (CronogramaPactoViewModel)TempData[PactoController.GetNomeVariavelTempData("Cronogramas", modelCronograma.IdPacto)];
            }

            if (pacto == null || pacto.IdSituacaoPacto != (int)eSituacaoPacto.Interrompido)
            {
                modelCronograma.Cronogramas = _cronogramaService.CalcularCronogramas(
                        modelCronograma.HorasTotais,
                        modelCronograma.HorasDiarias,
                        modelCronograma.DataInicial,
                        modelCronograma.CPFUsuario,
                        getUserLogado(),
                        modelCronograma.IdPacto,
                        modelCronograma.DataInicioSuspensao,
                        modelCronograma.DataFimSuspensao,
                        cronogramaExistente,
                        modelCronograma.QuantidadeHorasDiasSuspensao);
            } else
            {
                modelCronograma.Cronogramas = pacto.Cronogramas;       
            }


            modelCronograma.PodeRemoverDias = PodeRemoverDia(modelCronograma);

            //Só de abrir, já tenho que salvar
            TempData[PactoController.GetNomeVariavelTempData("Cronogramas", modelCronograma.IdPacto)] = modelCronograma;

            return Json(new { Mensagem = "Cronograma atualizado com sucesso", DataTermino = modelCronograma.Cronogramas.OrderBy(c => c.DataCronograma).LastOrDefault()?.DataString ?? "" });
        }



        private bool PodeRemoverDia(CronogramaPactoViewModel modelCronograma)
        {
            DateTime? dataReferenciaUltimoDia = modelCronograma.Cronogramas.LastOrDefault()?.DataCronograma;

            if (dataReferenciaUltimoDia.HasValue)
            {
                bool podeEditarDiasPassados = modelCronograma.IdPacto == 0 || modelCronograma.CPFUsuario == getUserLogado().CPF;
                var pacto = _pactoService.ObterPorId(modelCronograma.IdPacto);

                return podeEditarDiasPassados || !pacto.DataPrevistaTermino.HasValue || dataReferenciaUltimoDia.Value.Date > pacto.DataPrevistaTermino.Value.Date;
            }
            else
                return false;

        }

        [HttpPost]
        public PartialViewResult AdicionarDia(CronogramaPactoViewModel model)
        {

            AdicionarDiaCronograma(model);

            model.PodeRemoverDias = PodeRemoverDia(model);

            return PartialView("_CronogramasPartial", model);
        }

      
        private void AdicionarDiaCronograma(CronogramaPactoViewModel model)
        {
            var listaCronogramas = model.Cronogramas;
            var ultimoDia = listaCronogramas.LastOrDefault();

            var dataDiaCronograma = ultimoDia != null ? ultimoDia.DataCronograma.AddDays(1) : model.DataInicial;

            var pactosConcorrentes = _pactoService.GetPactosConcorrentes(dataDiaCronograma, DateTime.Now.AddYears(1), model.CPFUsuario, model.IdPacto);

            var cronogramVM = _cronogramaService.CriarDiaCronograma(dataDiaCronograma,
                TimeSpan.Zero, TimeSpan.Zero, pactosConcorrentes, cpfSolicitante: model.CPFUsuario, usuarioLogado: getUserLogado());

            listaCronogramas.Add(cronogramVM);
        }

        [HttpPost]
        public PartialViewResult RemoverDia(CronogramaPactoViewModel model)
        {
            RemoverDiaCronograma(model);
            model.PodeRemoverDias = PodeRemoverDia(model);
            return PartialView("_CronogramasPartial", model);
        }

        
        private static void RemoverDiaCronograma(CronogramaPactoViewModel model)
        {
            var listaCronogramas = model.Cronogramas;
            if (listaCronogramas.Count > 0)
            {
                listaCronogramas.RemoveAt(listaCronogramas.Count - 1);
            }
        }

        [HttpPost]
        public PartialViewResult AtualizarVisualizacao(CronogramaPactoViewModel model)
        {
            return PartialView("_CronogramasPartial", model);
        }

        [HttpPost]
        public PartialViewResult FecharCronograma(CronogramaPactoViewModel model)
        {
            ModelState.Clear();
            return PartialView("_CronogramasPartial", model);
        }

        [HttpPost]
        public PartialViewResult SalvarCronograma(CronogramaPactoViewModel model)
        {
            if (ValidarCronograma(model))
            {
                ModelState.Clear();
                _cronogramaService.LimparDiasZerados(model.Cronogramas);
                TempData[PactoController.GetNomeVariavelTempData("Cronogramas", model.IdPacto)] = model;
            }

            return PartialView("_CronogramasPartial", model);
        }

        private bool ValidarCronograma(CronogramaPactoViewModel model)
        {
            _cronogramaService.ValidarCronograma(model);
            setModelError(model.ValidationResult);

            return model.ValidationResult.IsValid;

        }

    }
}