using DomainValidation.Validation;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities.RH;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PGD.Application.ViewModels.Filtros;
using WebGrease.Css.Extensions;

namespace PGD.UI.Mvc.Controllers
{
    public class RelatorioApoioFolhaPontoController :  BaseController
    {

        IUsuarioAppService _usuarioservice;
        IPactoAppService _pactoservice;
        private readonly IUnidadeAppService _unidadeAppService;


        public RelatorioApoioFolhaPontoController(IUsuarioAppService usuarioAppService, IUnidadeService unidadeService, IPactoAppService pactoservice, IUnidadeAppService unidadeAppService) : base(usuarioAppService)
        {
            _usuarioservice = usuarioAppService;
            _unidadeService = unidadeService;
            _pactoservice = pactoservice;
            _unidadeAppService = unidadeAppService;
        }


        // GET: RelatorioApoioFolhaPonto
        public ActionResult Index()
        {
            var relatorioSearch = new RelatorioFolhaPontoSearchViewModel();
            var user = getUserLogado();

            relatorioSearch.NomeUnidade = user?.nomeUnidade;
            relatorioSearch.IdUnidade = user.Unidade;
            relatorioSearch.DataInicial = null;
            relatorioSearch.DataFinal = null;
            relatorioSearch.IsDirigente = user.IsDirigente;
            CarregarCombosFiltrosRelatorio(relatorioSearch, user);
            return View(relatorioSearch);
        }

        private void CarregarCombosFiltrosRelatorio(RelatorioFolhaPontoSearchViewModel relatorioSearch, UsuarioViewModel user)
        {
            if (!relatorioSearch.IsDirigente)
            {
                relatorioSearch.NomeServidor = user.Nome.TrimEnd();
                relatorioSearch.CpfServidor = user.CPF;
            }
            else
            {
                TempData["NomesSubordinados"] = ListarNomesPorUnidade();
            }
            TempData["Unidades"] = ListarUnidades();
        }

        private List<Unidade> ListarUnidades()
        {
            return _unidadeService.ObterUnidades().ToList();
        }

        private List<UsuarioViewModel> ListarNomesPorUnidade()
        {
            var user = getUserLogado();
            return _usuarioAppService.ObterTodos().ToList();
        }

        
        [HttpPost]
        public ActionResult Index(RelatorioFolhaPontoSearchViewModel dadosSearch)
        {
            ModelState.Clear();
            var usuarioLogado = getUserLogado();
            dadosSearch.IsDirigente = usuarioLogado.IsDirigente;
            if (ValidaFormularioSearch(dadosSearch))
            {
                var listaUsuarios = GetCpfServidoresEmissao(dadosSearch);
                var folhas = GetFolhasPonto(listaUsuarios, dadosSearch);
                return View("EmiteRelatorio", folhas);
            }
            else
            {
                CarregarCombosFiltrosRelatorio(dadosSearch, usuarioLogado);
                return View(dadosSearch);
            }
        }

        private IEnumerable<RelatorioFolhaPontoResultsViewModel> GetFolhasPonto(List<string> listaUsuarios, RelatorioFolhaPontoSearchViewModel dadosSearch)
        {
            var folhas = new List<RelatorioFolhaPontoResultsViewModel>();
            foreach(string cpf in listaUsuarios)
            {
                var cronogramas = GetCronogramas(cpf, dadosSearch);


                var idsPactos = RetornaIdsPactos(cronogramas.Select(x => x.IdsPacto).ToList());

                var nomeUnidades = RetornaNomesUnidades(_unidadeAppService.Buscar(new UnidadeFiltroViewModel
                {
                    IdsPactos = idsPactos
                }).Lista);

                if (cronogramas.Any())
                {
                    folhas.Add(new RelatorioFolhaPontoResultsViewModel()
                    {
                        
                        Servidor = GetDadosServidor(cpf, nomeUnidades),
                        ListCronogramas = cronogramas
                    });
                }
            }
            return folhas;
        }

        private List<int> RetornaIdsPactos(ICollection<string> ids)
        {
            var retorno = new List<int>();

            foreach (var id in ids)
                retorno.AddRange(id.Replace(" ", "").Split(',').Select(int.Parse));

            return retorno.Distinct().ToList();
        }

        private string RetornaNomesUnidades(ICollection<UnidadeViewModel> unidades)
        {
            var nomesUnidades = unidades.Select(x => x.Nome).Distinct().ToList();
            return string.Join("<br />", nomesUnidades);
        }

        private IEnumerable<DiaCronogramaConsolidadoViewModel> GetCronogramas(string cpf, RelatorioFolhaPontoSearchViewModel dadosSearch)
        {
            var cronogramas = _pactoservice.ObterTodosCronogramasCpfLogado(cpf, _pactoservice.ObterSituacoesPactoValido(), 
                dadosSearch.DataInicial, dadosSearch.DataFinal, dadosSearch.IdUnidade);

            return cronogramas.GroupBy(c => c.DataCronograma)
                    .Select(cd => new DiaCronogramaConsolidadoViewModel
                    {
                        DataCronograma = cd.First().DataCronograma,
                        HorasCronograma = TimeSpan.FromHours(cd.Sum(cc => cc.HorasCronograma.TotalHours)),
                        IdsPacto = string.Join(", ", cd.Select(cc => cc.IdPacto))

                    }).ToList();
            
        }

        private RelatorioFolhaPontoSearchViewModel GetDadosServidor(string cpf, string nomesUnidades)
        {
            var usuario = _usuarioservice.ObterPorCPF(cpf);
            return new RelatorioFolhaPontoSearchViewModel
            {
                CpfServidor = cpf,
                NomeServidor = usuario.Nome,
                NomeUnidade = nomesUnidades
            };
        }

        private List<string> GetCpfServidoresEmissao(RelatorioFolhaPontoSearchViewModel dadosSearch)
        {
            if (dadosSearch.CpfServidor != null)
            {
                return new List<string> { dadosSearch.CpfServidor };
            }
            else if (dadosSearch.IdUnidade > 0)
            {
                return _usuarioservice.ObterTodos(dadosSearch.IdUnidade).Select(c => c.CPF).ToList();
            }
            else
                return new List<string>();
        }

        private bool ValidaFormularioSearch(RelatorioFolhaPontoSearchViewModel dadosSearch)
        {
            if (dadosSearch.IsDirigente)
            {
                if (String.IsNullOrEmpty(dadosSearch.CpfServidor) && dadosSearch.IdUnidade <= 0)
                {
                    SetaErro("O nome do servidor ou a unidade devem ser preenchidos.");
                    return false;
                }
            }
            else
            {
                if ( String.IsNullOrEmpty(dadosSearch.CpfServidor) )
                {
                    SetaErro("O nome do servidor deve ser preenchido.");
                    return false;
                }
                if (dadosSearch.IdUnidade <= 0)
                {
                    SetaErro("A unidade deve ser preenchida.");
                    return false;
                }
            }
            if (dadosSearch.DataInicial != null && dadosSearch.DataFinal != null)
            {
                if (dadosSearch.DataInicial >= dadosSearch.DataFinal)
                {
                    SetaErro("A data inicial deve ser posterior à data final");
                    return false;
                }
            }

            return true;
        }

        private void SetaErro(string msgErro)
        {
            var lstErros = new List<ValidationError> { new ValidationError(msgErro) };
            setModelErrorList(lstErros);
        }
    }
}