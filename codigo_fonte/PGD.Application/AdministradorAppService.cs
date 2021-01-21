using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application
{
    public class AdministradorAppService : ApplicationService, IAdministradorAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;

        public AdministradorAppService(IUsuarioService usuarioService, IUnitOfWork uow, ILogService logService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
        }

        public AdministradorViewModel Adicionar(AdministradorViewModel administradorViewModel)
        {
            throw new NotImplementedException();
        }

        public AdministradorViewModel Atualizar(AdministradorViewModel administradorViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdministradorViewModel> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }
        public AdministradorViewModel Remover(AdministradorViewModel administradorViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
