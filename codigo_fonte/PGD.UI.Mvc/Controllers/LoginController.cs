using PGD.Application.Interfaces;
using PGD.Application.Util;
using PGD.Application.ViewModels;
using PGD.Application.ViewModels.Filtros;
using PGD.Domain.Entities.RH;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.DirectoryServices;

namespace PGD.UI.Mvc.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(IUsuarioAppService usuarioAppService,
            IUnidadeService unidadeService)
            : base(usuarioAppService)
        {
            _unidadeService = unidadeService;
        }

        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", loginViewModel);

            var usuario = Login(loginViewModel);

            if (usuario == null)
                return View(loginViewModel);

            if (!usuario.PerfilSelecionado.HasValue)
                return RedirectToAction("SelecionarPerfil", "Login");
            else
            {
                var deveSelecionarUnidade = _usuarioAppService.PodeSelecionarUnidade(usuario);

                if (!deveSelecionarUnidade)
                {
                    usuario.SelecionarUnidadePerfil();
                    setUserLogado(usuario);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("SelecionarUnidade", "Login");
            }
        }

        private UsuarioViewModel Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ConfigurationManager.AppSettings["ambiente"] != "Desenvolvimento")
                {
                    AutenticarLDAP(loginViewModel);
                }

                var usuario = BuscarUsuario(loginViewModel);

                if (usuario == null)
                {
                    ModelState.AddModelError("", "Usuário ou senha incorretos");
                    return null;
                }

                var deveSelecionarPerfil = _usuarioAppService.PodeSelecionarPerfil(usuario);

                if (!deveSelecionarPerfil)
                {
                    var perfil = usuario.PerfisUnidades.Select(x => x.PerfilEnum).FirstOrDefault(); 
                    usuario.AlterarPerfilSelecionado(perfil);
                } 

                setUserLogado(usuario);
                return usuario;
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", $@"Usuário ou senha incorretos "+e.Message);
                return null;
            }
        }

        private void AutenticarLDAP(LoginViewModel loginViewModel)
        {
            /*
            var ip = ConfigurationManager.AppSettings["IPLDAP"].ToString();
            var porta = int.Parse(ConfigurationManager.AppSettings["PortaLDAP"].ToString());
            var networkCredential = ConfigurationManager.AppSettings["NetworkCredentialLDAP"].ToString();

            var ldi = new LdapDirectoryIdentifier(ip, porta);
            var ldapConnection = new LdapConnection(ldi)
            {
                AuthType = AuthType.Basic
            };

            ldapConnection.SessionOptions.ProtocolVersion = 3;
            NetworkCredential nc = new NetworkCredential(string.Format(networkCredential, loginViewModel.Cpf.RemoverMaskCpfCnpj()), loginViewModel.Senha);
            ldapConnection.Bind(nc);
            ldapConnection.Dispose();
            */
            var ip = ConfigurationManager.AppSettings["IPLDAP"].ToString();
            var porta = int.Parse(ConfigurationManager.AppSettings["PortaLDAP"].ToString());
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + ip + ":" + porta, loginViewModel.Sigla, loginViewModel.Senha);
            DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
            directorySearcher.Filter = "(SAMAccountName=" + loginViewModel.Sigla + ")";
            SearchResult searchResult = directorySearcher.FindOne();

        }

        public ActionResult SelecionarPerfil()
        {
            PrepararTempDataPerfis();
            return View();
        }

        [HttpPost]
        public ActionResult SelecionarPerfil(Perfil? perfil)
        {
            if (perfil == null)
            {
                PrepararTempDataPerfis();
                return View();
            }

            var usuario = getUserLogado();
            usuario.AlterarPerfilSelecionado(perfil.Value);

            // usuario.AlterarListaPermissoes(_usuarioAppService.BuscarPermissoes(usuario.IdPerfilSelecionado));


            setUserLogado(usuario);

            var possuiUnidades = _usuarioAppService.PodeSelecionarUnidade(usuario);

            if (possuiUnidades)
                return RedirectToAction("SelecionarUnidade", "Login");
            else
            {
                usuario.SelecionarUnidadePerfil();
                setUserLogado(usuario);
                return RedirectToAction("Index", "Home");
            }
                
        }

        public ActionResult AlterarPerfil()
        {
            var usuario = getUserLogado();
            usuario.LimparPerfil();
            usuario.LimparUnidade();
            setUserLogado(usuario);
            return Json(new {ok = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelecionarUnidade()
        {
            PrepararTempDataUnidade();
            return View(new SelecionarUnidadeViewModel());
        }

        [HttpPost]
        public ActionResult SelecionarUnidade(SelecionarUnidadeViewModel selecionarUnidadeViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepararTempDataUnidade();
                return View(selecionarUnidadeViewModel);
            }

            var usuario = getUserLogado();
            usuario.AlterarUnidadeSelecionada(selecionarUnidadeViewModel.IdUnidade.Value);

            setUserLogado(usuario);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            LimparSessionUsuario();
            return Json(new {ok = true}, JsonRequestBehavior.AllowGet);
        }

        private void PrepararTempDataUnidade()
        {
            TempData["lstUnidade"] = GetUnidadesUsuario();
        }

        private void PrepararTempDataPerfis()
        {
            TempData["lstPerfil"] = GetPerfisUsuario();
        }

        private List<Unidade> GetUnidadesUsuario()
        {
            var retorno = new List<Unidade>();
            var usuario = getUserLogado();
            if (usuario == null || !usuario.IdPerfilSelecionado.HasValue)
                return retorno;

            return usuario.PerfisUnidades.Where(x => x.IdPerfil == usuario.IdPerfilSelecionado).Select(x => new Unidade
            {
                IdUnidade = x.IdUnidade,
                Nome = x.NomeUnidade,
                Sigla = x.SiglaUnidade
            }).Distinct().ToList();
        }

        private List<Perfil> GetPerfisUsuario()
        {
            var retorno = new List<Perfil>();
            var usuario = getUserLogado();
            if (usuario == null)
                return retorno;

            return usuario.PerfisUnidades.Select(p => p.PerfilEnum).Distinct().ToList();
        }

        private void LimparSessionUsuario()
        {
            Session["UserLogado"] = null;
            Session.Abandon();
        }

        private UsuarioViewModel BuscarUsuario(LoginViewModel loginViewModel)
        {
            //var retorno = _usuarioAppService.Buscar(new UsuarioFiltroViewModel{ Cpf = loginViewModel.Cpf.RemoverMaskCpfCnpj(), IncludeUnidadesPerfis = true});
           
            var retorno = _usuarioAppService.Buscar(new UsuarioFiltroViewModel { Sigla = loginViewModel.Sigla, IncludeUnidadesPerfis = true });
            return retorno.Lista.FirstOrDefault();
        }
    }
}