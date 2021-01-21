using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application
{
    public class ArquivoDadoBrutoAppService : ApplicationService, IArquivoDadoBrutoAppService
    {
        private readonly ILogService _logService;
        private readonly IUsuarioService _usuarioService;
        private readonly IArquivoDadoBrutoService _arquivoBrutoService;

        public ArquivoDadoBrutoAppService(IUsuarioService usuarioService, IUnitOfWork uow, ILogService logService, IArquivoDadoBrutoService arquivoBrutoService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _arquivoBrutoService = arquivoBrutoService;
        }

        public IEnumerable<ArquivoDadoBrutoViewModel> ObterTodos()
        {
            IEnumerable<ArquivoDadoBruto> lstArquivo = _arquivoBrutoService.ObterTodos();

            if (lstArquivo == null)
            {
                return null;
            }

            return Mapper.Map<IEnumerable<ArquivoDadoBruto>, IEnumerable<ArquivoDadoBrutoViewModel>>(lstArquivo);
        }

        public IEnumerable<ArquivoDadoBrutoViewModel> ObterPorAno(int ano)
        {
            IEnumerable<ArquivoDadoBruto> lstArquivo = _arquivoBrutoService.ObterPorAno(ano);

            if (lstArquivo == null)
            {
                return null;
            }

            return Mapper.Map<IEnumerable<ArquivoDadoBruto>, IEnumerable<ArquivoDadoBrutoViewModel>>(lstArquivo);
        }

    }
}
