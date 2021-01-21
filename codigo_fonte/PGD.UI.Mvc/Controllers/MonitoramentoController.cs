using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using PGD.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Controllers
{    
    public class MonitoramentoController : BaseController
    {

        IUsuarioAppService _Usuarioservice;

        public MonitoramentoController(IUsuarioAppService usuarioAppService) : base(usuarioAppService)
        {
            _Usuarioservice = usuarioAppService;
        }

        [HttpGet]
        public JsonResult Status()
        {
            Result situacao = new Result
            {
                Status = "ok"
            };

            try
            {
                // teste do sigrhApi
                var userTeste = _usuarioAppService.ObterPorCPF("11111122233");

            }
            catch(Exception e)
            {
                situacao.Status = "Erro: " + e.GetBaseException();
            }
            
            return Json (situacao, JsonRequestBehavior.AllowGet);
        }
    }

    /// <summary>
    /// Model para resultado do teste de conexão com o banco
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Status da conexão
        /// </summary>
        public string Status { get; set; }
    }
}