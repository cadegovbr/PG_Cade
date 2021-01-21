using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
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
    /// Retorna dados sobre unidades internas, dirigentes destas e hierarquias.
    /// </summary>
    public class UnidadesController : ApiController
    {
        /// <summary>
        /// Retorna todas as unidades internas do órgão.
        /// </summary>
        /// <returns>Lista das unidades</returns>
        [HttpGet]
        [Route("api/unidades")]
        public List<Unidade> GetUnidades()
        {
            var sigrh = new SigRHService();
            return sigrh.ObterTodasUnidades();
        }

        /// <summary>
        /// Retorna detalhes de uma unidade
        /// </summary>
        /// <param name="id">Identificador da unidade</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/unidades/{id}")]
        public Unidade GetUnidadeById(int id)
        {
            var sigrh = new SigRHService();
            return sigrh.ObterUnidadeById(id);
        }


        /// <summary>
        /// Retorna lista de unidades subordinadas à unidade, em todos os níveis.
        /// </summary>
        /// <param name="idUnidadeSuperior">Identificador da unidade</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/unidades/subordinadas/{idUnidadeSuperior}")]
        public List<Unidade> GetUnidadesSubordinadas(int idUnidadeSuperior)
        {
            var sigrh = new SigRHService();
            return sigrh.ObterUnidadesSubordinadas(idUnidadeSuperior);
        }

        /// <summary>
        /// Retorna os dirigentes da unidade
        /// </summary>
        /// <param name="idUnidade">Identificador da unidade</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/unidades/dirigentes/{idUnidade}")]
        public List<Usuario> GetDirigentesUnidade(int idUnidade)
        {
            var sigrh = new SigRHService();
            return sigrh.ObterDirigentesUnidade(idUnidade);
        }


    }
}
