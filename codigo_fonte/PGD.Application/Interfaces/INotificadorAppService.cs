using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface INotificadorAppService
    {
        bool TratarNotificacaoPacto(PactoViewModel pactoBuscado, UsuarioViewModel usuarioLogado, string oper, AvaliacaoProdutoViewModel apvm = null);
        void EnviarEmailNotificacaoFinalizacaoPacto(PactoViewModel p);
        void EnviarEmailNotificacaoReativacaoAutomaticaPacto(PactoViewModel p);
    }
}
