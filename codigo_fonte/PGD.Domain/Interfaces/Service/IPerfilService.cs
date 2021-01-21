using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IPerfilService : IService<Perfil>
    {
        IEnumerable<Perfil> ObterPerfis(Usuario objUsuario);
    }
}
