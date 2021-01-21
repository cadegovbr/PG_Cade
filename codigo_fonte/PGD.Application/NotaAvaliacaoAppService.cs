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
    public class NotaAvaliacaoAppService : ApplicationService, INotaAvaliacaoAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;
        private readonly INotaAvaliacaoService _notaAvaliacaoService;

        public NotaAvaliacaoAppService(IUsuarioService usuarioService, ILogService logService, IUnitOfWork uow, INotaAvaliacaoService notaAvaliacaoService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _notaAvaliacaoService = notaAvaliacaoService;
        }

        public NotaAvaliacaoViewModel Adicionar(NotaAvaliacaoViewModel notaAvaliacaoViewModel)
        {
            throw new NotImplementedException();
        }

        public NotaAvaliacaoViewModel Atualizar(NotaAvaliacaoViewModel notaAvaliacaoViewModel)
        {
            throw new NotImplementedException();
        }

        public NotaAvaliacaoViewModel ObterPorId(int id)
        {
            return Mapper.Map<NotaAvaliacao, NotaAvaliacaoViewModel>(_notaAvaliacaoService.ObterPorId(id));
        }

        public IEnumerable<NotaAvaliacaoViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<NotaAvaliacao>, IEnumerable<NotaAvaliacaoViewModel>>(_notaAvaliacaoService.ObterTodos());
        }

        public IEnumerable<NotaAvaliacaoViewModel> ObterTodosPorNivelAvaliacao(int idNivelAvaliacao)
        {
            return Mapper.Map<IEnumerable<NotaAvaliacao>, IEnumerable<NotaAvaliacaoViewModel>>(_notaAvaliacaoService.ObterTodosPorNivelAvaliacao(idNivelAvaliacao));
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
