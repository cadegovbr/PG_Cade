using PGD.Domain.Entities.Usuario;
using PGD.Services.REST.SIGRHAPI.Service;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;


namespace PGD.Services.REST.SIGRHAPI.Controllers
{

    /// <summary>
    /// Retorna dados de usuários do sistema
    /// </summary>
    public class UsuariosController : ApiController
    {
        /// <summary>
        /// Retorna todos os usuarios da base
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet]
        [Route("api/usuarios")]
        public List<Usuario> GetAllUsuarios()
        {
            return new SigRHService().TodosUsuariosDaBase();
        }

        /// <summary>
        /// Retorna usuario por cpf  
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Usuario))]
        [Route("api/usuarios/porCpf/{cpf}")]
        public IHttpActionResult GetUsuarioPorCpf(string cpf)
        {
            if (!String.IsNullOrEmpty(cpf))
            {
                return Ok(new SigRHService().ObterUsuarioPorParametro(cpf));
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Retorna usuario por nome  
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Usuario))]
        [Route("api/usuarios/porNome/{nome}")]
        public IHttpActionResult GetUsuarioPorNome(string nome)
        {
            if (!String.IsNullOrEmpty(nome))
            {
                return Ok(new SigRHService().ObterUsuarioPorNome(nome));
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Retorna lista de usuarios da unidade
        /// </summary>
        /// <param name="id">id da unidade</param>
        /// <param name="incluirSubordinados">true returna usuarios das unidades inferiores</param>
        /// <returns>lista de unidades</returns>
        [HttpGet]
        [ResponseType(typeof(Usuario))]
        [Route("api/usuarios/porUnidade/{id}")]
        public List<Usuario> GetListaPorUnidade(string id, [FromUri]bool incluirSubordinados=false)
        {
            return new SigRHService().TodosUsuariosDaUnidade(id, incluirSubordinados);
        }


    }
}
