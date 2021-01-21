using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using PGD.Domain.Entities.RH;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PGD.Domain.Entities.Usuario;
using System.Net;

namespace PGD.Domain.Services
{
    public class RHService : IRHService
    {
        private readonly ILogService _logService;
        private readonly IUnidade_TipoPactoService _unidade_TipoPactoService;
        public RHService(ILogService logService, IUnidade_TipoPactoService unidade_TipoPactoService)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _logService = logService;
            _unidade_TipoPactoService = unidade_TipoPactoService;
        }

        public IEnumerable<PGD.Domain.Enums.Perfil> ObterPerfis(Usuario objUsuario)
        {
            var lista = new List<PGD.Domain.Enums.Perfil>();

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SIGRHAPI"].ToString()))
                return null;
            var api = ConfigurationManager.AppSettings["SIGRHAPI"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));//"application/json"
                client.Timeout = new TimeSpan(1, 0, 0);
                var retorno = client.GetAsync(string.Format("api/Perfil/{0}", objUsuario.Cpf)).Result;

                try
                {
                    lista = JsonConvert.DeserializeObject<List<PGD.Domain.Enums.Perfil>>(retorno.Content.ReadAsStringAsync().Result);
                }
                catch
                {
                    string errMsg = $"erro no obterperfis, cpf: {objUsuario.Cpf} \n Retorno: {retorno.Content.ReadAsStringAsync().Result}";
                    throw new Exception(errMsg);
                }
            }


            return lista;
        }

        public IEnumerable<Unidade> ObterUnidades(int idTipoPacto = 0)
        {
            var listaTodasUnidades = new List<Unidade>();

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SIGRHAPI"].ToString()))
                return null;
            var api = ConfigurationManager.AppSettings["SIGRHAPI"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));//"application/json"
                client.Timeout = new TimeSpan(1, 0, 0);
                var retorno = client.GetAsync("api/unidades").Result;

                listaTodasUnidades = JsonConvert.DeserializeObject<List<Unidade>>(retorno.Content.ReadAsStringAsync().Result);
            }

            if (idTipoPacto > 0)
            {
                var listaUnidadeHabilitadasParaTipoPacto = _unidade_TipoPactoService.ObterTodosPorTipoPacto(idTipoPacto).ToList();

                return listaTodasUnidades.Where(l => listaUnidadeHabilitadasParaTipoPacto.Select(h => h.IdUnidade).Contains(l.IdUnidade)).ToList();
            }
            else
            {
                return listaTodasUnidades;
            }
        }

        public IEnumerable<Unidade> ObterUnidadesSubordinadas(int idUnidadePai)
        {
            var lista = new List<Unidade>();

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SIGRHAPI"].ToString()))
                return null;
            var api = ConfigurationManager.AppSettings["SIGRHAPI"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));//"application/json"
                client.Timeout = new TimeSpan(1, 0, 0);
                var retorno = client.GetAsync($"api/unidades/subordinadas/{idUnidadePai}").Result;

                lista = JsonConvert.DeserializeObject<List<Unidade>>(retorno.Content.ReadAsStringAsync().Result);
            }

            return lista;
        }

        public IEnumerable<Feriado> ObterFeriados(DateTime dtAPartirDe)
        {
            var lista = new List<Feriado>();

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SIGRHAPI"].ToString()))
                return null;
            var api = ConfigurationManager.AppSettings["SIGRHAPI"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));//"application/json"
                client.Timeout = new TimeSpan(1, 0, 0);
                var retorno = client.GetAsync(string.Format("api/Feriados/{0}", dtAPartirDe.ToString("yyyyMMdd"))).Result;

                lista = JsonConvert.DeserializeObject<List<Feriado>>(retorno.Content.ReadAsStringAsync().Result);
            }


            return lista;
        }

        public Feriado ObterFeriado(DateTime data)
        {
            return ObterFeriados(data).FirstOrDefault(a => a.data_feriado == data.Date);
        }
        public bool VerificaFeriado(DateTime dtAVerificar)
        {
            var ehFeriado = false;

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SIGRHAPI"].ToString()))
                return false;
            var api = ConfigurationManager.AppSettings["SIGRHAPI"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));//"application/json"
                client.Timeout = new TimeSpan(1, 0, 0);
                var retorno = client.GetAsync(string.Format("api/feriados/EhFeriado/{0}", dtAVerificar.ToString("yyyyMMdd"))).Result;

                ehFeriado = bool.Parse(retorno.Content.ReadAsStringAsync().Result);
            }

            return ehFeriado;
        }

        public IEnumerable<Usuario> ObterDirigentesUnidade(int idUnidadePai)
        {
            var lista = new List<Usuario>();

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SIGRHAPI"].ToString()))
                return null;
            var api = ConfigurationManager.AppSettings["SIGRHAPI"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                client.Timeout = new TimeSpan(1, 0, 0);
                var retorno = client.GetAsync($"api/unidades/dirigentes/{idUnidadePai}").Result;

                lista = JsonConvert.DeserializeObject<List<Usuario>>(retorno.Content.ReadAsStringAsync().Result);
            }

            return lista;
        }

        public Unidade ObterUnidade(int idUnidade)
        {
            var listaTodasUnidades = ObterUnidades();
            return listaTodasUnidades.SingleOrDefault(i => i.IdUnidade == idUnidade);
        }
    }
}
