using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace PGD.Application
{
    public class OS_ItemAvaliacaoAppService : ApplicationService, IOS_ItemAvaliacaoAppService
    {
        private readonly IOS_ItemAvaliacaoService _OS_ItemAvaliacaoService;
        private readonly ILogService _logService;
        private readonly IUsuarioAppService _usuarioAppService;
        
        public OS_ItemAvaliacaoAppService(IUnitOfWork uow, IOS_ItemAvaliacaoService oS_ItemAvaliacaoService, ILogService logService,
            IUsuarioAppService usuarioAppService)
            : base(uow)
        {
            _usuarioAppService = usuarioAppService;
            _logService = logService;
            _OS_ItemAvaliacaoService = oS_ItemAvaliacaoService;
        }

        public OS_ItemAvaliacaoViewModel Adicionar(OS_ItemAvaliacaoViewModel oS_ItemAvaliacaoViewModel, UsuarioViewModel user)
        {
            throw new NotImplementedException();
        }

        public OS_ItemAvaliacaoViewModel Atualizar(OS_ItemAvaliacaoViewModel oS_ItemAvaliacaoViewModel, UsuarioViewModel user)
        {
            throw new NotImplementedException();
        }

        public OS_ItemAvaliacaoViewModel BuscarPorId(int idItemAvaliacao)
        {
            return Mapper.Map<OS_ItemAvaliacao, OS_ItemAvaliacaoViewModel>(_OS_ItemAvaliacaoService.ObterPorId(idItemAvaliacao));
        }

        public IEnumerable<OS_ItemAvaliacaoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<OS_ItemAvaliacao>, IEnumerable<OS_ItemAvaliacaoViewModel>>(_OS_ItemAvaliacaoService.ObterTodos());
        }

        public void Remover(int id, UsuarioViewModel user)
        {
            throw new NotImplementedException();
        }
    }
}
