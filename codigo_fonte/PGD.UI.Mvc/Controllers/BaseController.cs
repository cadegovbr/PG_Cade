using DomainValidation.Validation;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PGD.UI.Mvc.Controllers
{

    public class BaseController : Controller
    {
        protected readonly IUsuarioAppService _usuarioAppService;
        public IUnidadeService _unidadeService;
        public IPerfilService _perfilService;
        public IFeriadoService _feriadoService;

        public BaseController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //Localization in Base controller:
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");
            return base.BeginExecuteCore(callback, state);
        }

        public void setUserLogado(UsuarioViewModel userLogado)
        {
            Session["UserLogado"] = userLogado;
        }

        public UsuarioViewModel getUserLogado()
        {
            return (UsuarioViewModel)Session["UserLogado"];
        }

        [NonAction]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = getUserLogado();
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;


            if (!ActionsNaoValidarRota().Contains(action))
            {
                if (controller != "Login" || (action != "LogOut" && action != "AlterarPerfil"))
                {
                    if (user == null && (controller != "Login" || action != "Index"))
                        if (Request.IsAjaxRequest())
                        {
                            filterContext.Result = new ContentResult();
                            filterContext.HttpContext.Response.StatusCode = 401;
                        }
                        else
                            filterContext.Result = RedirectToRoute(new RouteValueDictionary(new { controller = "Login", action = "Index" }));

                    if (user != null)
                    {
                        if (!user.PerfilSelecionado.HasValue && (controller != "Login" || action != "SelecionarPerfil"))
                            filterContext.Result = RedirectToRoute(new RouteValueDictionary(new { controller = "Login", action = "SelecionarPerfil" }));

                        if (user.PerfilSelecionado.HasValue && !user.IdUnidadeSelecionada.HasValue && (controller != "Login" || action != "SelecionarUnidade"))
                            filterContext.Result = RedirectToRoute(new RouteValueDictionary(new { controller = "Login", action = "SelecionarUnidade" }));

                        if (user.PerfilSelecionado.HasValue && user.IdUnidadeSelecionada.HasValue && controller == "Login")
                            filterContext.Result = RedirectToRoute(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    }
                }
            }
            

            ValidarExigeAdmin(filterContext);

            ValidarPermissoes(filterContext);

            base.OnActionExecuting(filterContext);

            getMessages();
        }

        // TODO: Verificar se vai ter validação de permissões e aplicar validação por controller e action
        private void ValidarPermissoes(ActionExecutingContext filterContext)
        {
            var user = getUserLogado();
            if (user == null) return;
            var perfilAtual = user.IdPerfilSelecionado;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao executar a operação");
            //base.OnException(filterContext);
        }

        public string GetVersaoSistema()
        {
            // string versao =  ConfigurationManager.AppSettings["VersaoSistema"].ToString();
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return assemblyVersion;
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            using (var sw = new StringWriter())
            {

                // Create an MVC Controller Context
                var wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);

                RouteData routeData = new RouteData();

                routeData.Values.Add("controller", this.GetType().Name
                    .ToLower()
                    .Replace("controller", ""));

                this.ControllerContext = new ControllerContext(wrapper, routeData, this);

                this.ViewData.Model = model;

                var viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);

                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }

        public new RedirectToRouteResult RedirectToAction(string action, string controller, object routeValues)
        {
            if (routeValues == null)
                return base.RedirectToAction(action, controller);
            else
                return base.RedirectToAction(action, controller, routeValues);
        }

        #region Alertas
        public ActionResult Alerta(string tipo, string mensagem)
        {
            MessageViewModel message = new MessageViewModel { Tipo = (TipoMessage)int.Parse(tipo), Mensagem = mensagem };
            return PartialView("_MessagePartial", message);
        }

        public void getMessages()
        {
            ViewData["Message"] = "";

            if (TempData["Result"] != null)
            {
                ValidationResult result = (ValidationResult)TempData["Result"];

                if (result.IsValid)
                {
                    ViewData["MessageType"] = TipoMessage.success;
                    if (result.Message != null)
                        ViewData["Message"] = result.Message;
                }
                else
                {
                    ViewData["MessageType"] = TipoMessage.danger;
                    string message = "";
                    foreach (ValidationError mensagem in result.Erros)
                        message += mensagem.Message;
                    ViewData["Message"] = message.Replace("\r\n", "");
                }

                TempData["Result"] = null;
            }
        }

        public void setMessage(ValidationResult result)
        {
            TempData["Result"] = result;
        }

        public void setMessage(string message, bool erro = false, string campo = "")
        {
            ValidationResult resultado = new ValidationResult();

            Mensagem msg = new Mensagem();
            msg.Descricao = message;
            msg.Campo = campo;
            resultado.Message = msg.Descricao;

            if (erro)
                setModelError(resultado);
            else
                setMessage(resultado);
        }

        public void setModelError(ValidationResult resultado)
        {
            foreach (ValidationError mensagem in resultado.Erros)
                ModelState.AddModelError(mensagem.Message, mensagem.Message);
        }

        public void setModelErrorList(IEnumerable<ValidationError> erros)
        {
            foreach (var erro in erros)
                ModelState.AddModelError(string.Empty, erro.Message);
        }

        public void setModelError(ValidationError erro)
        {
            ModelState.AddModelError(string.Empty, erro.Message);
        }

        public ActionResult setMessageAndRedirect(string message, string action, object routes = null)
        {
            setMessage(message);
            return RedirectToAction(action, routes);
        }

        public ActionResult setMessageAndRedirect(string message, string action, RouteValueDictionary routeValues)
        {
            setMessage(message);
            return RedirectToAction(action, routeValues);
        }

        public ActionResult setMessageAndRedirect(string action, RouteValueDictionary routeValues)
        {
            return RedirectToAction(action, routeValues);
        }

        public ActionResult setMessageAndRedirect(ValidationResult result, string action, object routes = null)
        {
            setMessage(result);
            return RedirectToAction(action, routes);
        }

        public ActionResult setMessageAndRedirect(IEnumerable<ValidationError> erros, string action, object routes = null)
        {
            setModelErrorList(erros);
            return RedirectToAction(action, routes);
        }

        public ActionResult RetornarErrosModelState(string joinSeparator = "|||")
        {
            var mensagem = string.Join(joinSeparator, ModelState.Values
                               .SelectMany(v => v.Errors)
                               .Select(e => e.ErrorMessage));
            return RetornarErro(mensagem);
        }


        public JsonResult RetornarErro(string mensagem)
        {
            Response.StatusCode = 400;
            return Json(new { MsgErro = mensagem });
        }

        public JsonResult RetornarMensagem(string mensagem)
        {
            return Json(new { Mensagem = mensagem });
        }


        #endregion

        public bool ToExcel(HttpResponseBase Response, DataTable listTable, string nomeArquivo = "ConsultaPedidos.xls", int columnsMerged = 0)
        {
            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = listTable;


            grid.DataBind();

            if (columnsMerged > 0)
            {
                MergeRows(grid, columnsMerged);
            }


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + nomeArquivo);
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

            return true;


        }

        public void MergeRows(GridView gridView, int columnsMerged)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < columnsMerged; i++)
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }

        public IEnumerable<string> RetornaErrosModelState()
        {
            return ModelState.SelectMany(keyValuePair => keyValuePair.Value.Errors).Select(modelError => modelError.ErrorMessage).ToList();
        }

        private void ValidarExigeAdmin(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var userLogado = getUserLogado();
            if (controller == "Usuario" && (userLogado == null || !userLogado.IsAdmin))
                filterContext.Result = RedirectToRoute(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }

        private string[] ActionsNaoValidarRota() {
            return new string[] { "ErroGenerico" };
        }

        public ActionResult ErroGenerico()
        {
            return View("~/Views/Erro/Error.cshtml");
        }
    }
}