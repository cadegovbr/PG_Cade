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
    public class NivelAvaliacaoAppService : ApplicationService, INivelAvaliacaoAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;
        private readonly INivelAvaliacaoService _nivelAvaliacaoService;

        public NivelAvaliacaoAppService(IUsuarioService usuarioService, ILogService logService, IUnitOfWork uow, INivelAvaliacaoService nivelAvaliacaoService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _nivelAvaliacaoService = nivelAvaliacaoService;
        }

        public NivelAvaliacaoViewModel Adicionar(NivelAvaliacaoViewModel nivelAvaliacaoViewModel)
        {
            throw new NotImplementedException();
        }

        public NivelAvaliacaoViewModel Atualizar(NivelAvaliacaoViewModel nivelAvaliacaoViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NivelAvaliacaoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<NivelAvaliacao>, IEnumerable<NivelAvaliacaoViewModel>>(_nivelAvaliacaoService.ObterTodos());
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
