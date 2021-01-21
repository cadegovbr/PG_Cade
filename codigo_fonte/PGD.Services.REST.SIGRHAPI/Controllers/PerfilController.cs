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
    /// Fornece dados de perfis do usuário
    /// </summary>
    public class PerfilController : ApiController
    {
        /// <summary>
        /// Obtem os perfis do usuário pelo CPF
        /// </summary>
        /// <param name="cpf">CPF do usuário</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/perfil/{cpf}")]
        public List<Perfil> GetPerfilByCPF(string cpf)
        {
            var lista = new List<Perfil>();
            var sigrh = new SigRHService();

            if (sigrh.EhDirigente(cpf))
                lista.Add(Perfil.Dirigente);

            if (sigrh.EhSolicitante(cpf))
                lista.Add(Perfil.Solicitante);

            //Se não for nem dirigente nem solicitante, retorna como consulta
            if (lista.Count == 0)
                lista.Add(Perfil.Consulta);

            return lista;
        }

    }
}
