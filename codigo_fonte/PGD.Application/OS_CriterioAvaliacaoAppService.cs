using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Application
{
    public class OS_CriterioAvaliacaoAppService : ApplicationService, IOS_CriterioAvaliacaoAppService
    {
        private readonly IOS_CriterioAvaliacaoService _OS_CriterioAvaliacaoService;
        private readonly ILogService _logService;
        private readonly IUsuarioAppService _usuarioAppService;
        
        public OS_CriterioAvaliacaoAppService(IUnitOfWork uow, IOS_CriterioAvaliacaoService oS_CriterioAvaliacaoService, ILogService logService,
            IUsuarioAppService usuarioAppService)
            : base(uow)
        {
            _usuarioAppService = usuarioAppService;
            _logService = logService;
            _OS_CriterioAvaliacaoService = oS_CriterioAvaliacaoService;
        }

        public OS_CriterioAvaliacaoViewModel Adicionar(OS_CriterioAvaliacaoViewModel oS_CriterioAvaliacaoViewModel, UsuarioViewModel user)
        {
            throw new NotImplementedException();
        }

        public OS_CriterioAvaliacaoViewModel Atualizar(OS_CriterioAvaliacaoViewModel oS_CriterioAvaliacaoViewModel, UsuarioViewModel user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OS_CriterioAvaliacaoViewModel> BuscarPorIdOS(int idOS)
        {
            var retorno = _OS_CriterioAvaliacaoService.BuscarPorIdOS(idOS).ToList();
            return Mapper.Map<IEnumerable<OS_CriterioAvaliacao>, IEnumerable<OS_CriterioAvaliacaoViewModel>>(retorno);
        }

        public IEnumerable<OS_CriterioAvaliacaoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<OS_CriterioAvaliacao>, IEnumerable<OS_CriterioAvaliacaoViewModel>>(_OS_CriterioAvaliacaoService.ObterTodos());
        }

        public void Remover(int id, UsuarioViewModel user)
        {
            throw new NotImplementedException();
        }
    }
}
