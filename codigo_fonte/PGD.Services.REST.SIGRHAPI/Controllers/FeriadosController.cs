using PGD.Application.ViewModels;
using PGD.Domain.Entities.RH;
using PGD.Services.REST.SIGRHAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGD.Services.REST.SIGRHAPI.Controllers
{
    /// <summary>
    /// Controller para fornecer informações sobre feriados nacionais.
    /// </summary>
    public class FeriadosController : ApiController
    {
        private const string DataInicioSistema = "20170101";

        /// <summary>
        /// Retorna todos os feriados desde o início do sistema.
        /// </summary>
        /// <returns>lista de feriados</returns>
        [HttpGet]
        [Route("api/feriados")]
        public List<Feriado> Index()
        {
            return new SistemasComumService().ObterFeriados(DataInicioSistema);
        }


        /// <summary>
        /// Obtem os feriados cadastrados no sistema a partir de uma data
        /// </summary>
        /// <param name="dataInicio">Data no formato AAAAMMDD, traz apenas os feriados a partir desta data.</param>
        /// <returns>Lista de feriados</returns>

        [HttpGet]
        [Route("api/feriados/{dataInicio}")]
        public List<Feriado> GetFeriadoByDataInicio(string dataInicio)
        {
            return new SistemasComumService().ObterFeriados(dataInicio);
        }


        /// <summary>
        /// Retorna se uma data é feriado ou não
        /// </summary>
        /// <param name="data">Data no formato AAAAMMDD</param>
        /// <returns>true se é feriado</returns>
        [HttpGet]
        [Route("api/feriados/ehferiado/{data}")]
        public bool GetEhFeriado(string data)
        { 
            return new SistemasComumService().VefificaSeFeriado(data);
        }
    }
}
