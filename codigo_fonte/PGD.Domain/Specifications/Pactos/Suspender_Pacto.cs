using DomainValidation.Interfaces.Specification;
using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Specifications.Pactos
{
    public class Suspender_Pacto : ISpecification<Pacto>
    {
        #region DESCRIÇÃO DA RN046
        //O usuário não pode assinar no mesmo pacto como solicitante e dirigente, assim, caso o usuário esteja logado como "Solicitante" e 
        //também tenha acesso ao perfil "Dirigente/Substituto", não é permitido que assine seu próprio pacto como "Dirigente" e nem assine 
        //a sua própria avaliação, suspenda seu pacto, retorne da suspensão do seu pacto e interrompa seu pacto.Para esta situação, a 
        //assinatura e a avaliação devem ser realizadas pelo dirigente desta unidade ou chefe da unidade imediatamente superior.
        #endregion
        public Usuario UsuarioLogado { get; set;}
        public List<PGD.Domain.Enums.Perfil> Perfis { get; set; }
        public bool IsSatisfiedBy(Pacto entity)
        {
            
            if(entity.CpfUsuario.ToString() == UsuarioLogado.Cpf)
            {
                foreach (var obj in Perfis)
                {
                    if(PGD.Domain.Enums.Perfil.Dirigente == obj)
                    {
                        return false;
                    }
                }                
            }
            return true;
        }
    }
}
