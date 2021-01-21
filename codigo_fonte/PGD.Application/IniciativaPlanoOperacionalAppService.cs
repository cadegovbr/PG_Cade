using AutoMapper;
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
    public class IniciativaPlanoOperacionalAppService : ApplicationService, IIniciativaPlanoOperacionalAppService
    {
        private readonly IIniciativaPlanoOperacionalService _iniciativaPOService;

        public IniciativaPlanoOperacionalAppService(IUnitOfWork uow, IIniciativaPlanoOperacionalService iniciativaPOService) : base(uow)
        {
            _iniciativaPOService = iniciativaPOService;
        }

        public List<IniciativaPlanoOperacionalViewModel> ObterTodos()
        {
            return Mapper.Map<List<IniciativaPlanoOperacionalViewModel>>(_iniciativaPOService.ObterTodos());
        }
    }
}
