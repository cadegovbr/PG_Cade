using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using PGD.Domain.Specifications.Pactos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Validations.Pactos
{
    public class SuspenderPactoValidation:Validator<Pacto>
    {
        public SuspenderPactoValidation(Usuario usuariologado, List<PGD.Domain.Enums.Perfil> Perfis)
        {
            var ruleSuspenderData = new Suspender_Pacto_Data();
            var ruleSuspenderProprioPacto = new Suspender_Pacto();
            ruleSuspenderProprioPacto.UsuarioLogado = usuariologado;
            ruleSuspenderProprioPacto.Perfis = Perfis;

            base.Add("Suspender_Pacto_Data", new Rule<Pacto>(ruleSuspenderData, "Plano de Trabalho já suspenso ou fora do período de vigência."));
            base.Add("ruleSuspenderProprioPacto", new Rule<Pacto>(ruleSuspenderProprioPacto, "Usuário não pode assinar seu próprio plano de trabalho."));
        }
    }
}
