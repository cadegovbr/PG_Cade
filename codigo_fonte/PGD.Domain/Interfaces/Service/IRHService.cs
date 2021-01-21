using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using System;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IRHService
    {
        IEnumerable<PGD.Domain.Enums.Perfil> ObterPerfis(Usuario objUsuario);

        #region CSU008_RN050
        IEnumerable<Unidade> ObterUnidades(int idTipoPacto = 0);
        Unidade ObterUnidade(int idUnidade);
        #endregion

        IEnumerable<Feriado> ObterFeriados(DateTime dtAPartirDe);

        bool VerificaFeriado(DateTime dataAVerificar);

        Feriado ObterFeriado(DateTime data);

        IEnumerable<Unidade> ObterUnidadesSubordinadas(int idUnidadePai);

        IEnumerable<Usuario> ObterDirigentesUnidade(int idUnidade);
    }
}
