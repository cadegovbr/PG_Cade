using PGD.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using AutoMapper;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;

namespace PGD.Application
{
    public class JustificativaAppService : ApplicationService, IJustificativaAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;
        private readonly IJustificativaService _justificativaService;

        public JustificativaAppService(IUsuarioService usuarioService, ILogService logService, IUnitOfWork uow, IJustificativaService justificativaService )
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _justificativaService = justificativaService;
        }

        public JustificativaViewModel Adicionar(JustificativaViewModel justificativaViewModel)
        {
            throw new NotImplementedException();
        }

        public JustificativaViewModel Atualizar(JustificativaViewModel justificativaViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JustificativaViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<Justificativa>, IEnumerable<JustificativaViewModel>>(_justificativaService.ObterTodos());
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
