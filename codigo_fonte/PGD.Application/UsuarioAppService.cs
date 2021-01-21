using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.Util;
using PGD.Application.ViewModels;
using PGD.Application.ViewModels.Filtros;
using PGD.Application.ViewModels.Paginacao;
using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PGD.Application.Util;
using PGD.Domain.Paginacao;

namespace PGD.Application
{
    public class UsuarioAppService : ApplicationService, IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogService _logService;
        private readonly IAdministradorService _admService;
        private readonly IPerfilService _perfilService;
        private readonly IUnidadeService _unidadeService;
        private readonly IPermissaoService _permissaoService;
        private readonly IUsuarioPerfilUnidadeService _usuarioPerfilUnidadeService;

        public UsuarioAppService(
            IUsuarioService usuarioService, 
            IUnitOfWork uow, 
            ILogService logService, 
            IPerfilService perfilService, 
            IAdministradorService admService, 
            IUnidadeService unidadeService,
            IPermissaoService permissaoService,
            IUsuarioPerfilUnidadeService usuarioPerfilUnidadeService)
            : base(uow)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _perfilService = perfilService;
            _admService = admService;
            _unidadeService = unidadeService;
            _permissaoService = permissaoService;
            _usuarioPerfilUnidadeService = usuarioPerfilUnidadeService;
        }

        public UsuarioViewModel Adicionar(UsuarioViewModel usuarioViewModel)
        {
            throw new NotImplementedException();
        }

        public UsuarioViewModel Atualizar(UsuarioViewModel usuarioViewModel)
        {
            throw new NotImplementedException();
        }

        public UsuarioViewModel ObterPorNome(string nome)
        {
            return Mapper.Map<Usuario, UsuarioViewModel>(_usuarioService.ObterPorNome(nome));
        }

        public UsuarioViewModel ObterPorEmail(string email)
        {
            return Mapper.Map<Usuario, UsuarioViewModel>(_usuarioService.ObterPorEmail(email));
        }

        public UsuarioViewModel ObterPorCPF(string cpf)
        {
            var eAdm = new Administrador();
            var user = Mapper.Map<Usuario, UsuarioViewModel>(_usuarioService.ObterPorCPF(cpf));
            // user.Administrador = user.PerfisUnidades.Any(x => x.IdPerfil == (int) Domain.Enums.Perfil.Administrador);

            return user;
        }

        public PaginacaoViewModel<UsuarioViewModel> Buscar(UsuarioFiltroViewModel model)
        {
            var retorno = Mapper.Map<PaginacaoViewModel<UsuarioViewModel>>(_usuarioService.Buscar(Mapper.Map<UsuarioFiltro>(model)));
            return retorno;
        }

        public PaginacaoViewModel<UnidadeViewModel> BuscarUnidades(UnidadeFiltroViewModel filtro)
        {
            var retorno = _unidadeService.Buscar(Mapper.Map<UnidadeFiltro>(filtro));
            return new PaginacaoViewModel<UnidadeViewModel>
            {
                QtRegistros = retorno.QtRegistros,
                Lista = retorno.Lista.Select(x => new UnidadeViewModel
                {
                    IdUnidade = x.IdUnidade,
                    Nome = x.Nome,
                    Sigla = x.Sigla,
                    Excluido = x.Excluido,
                    IdUnidadeSuperior = x.IdUnidadeSuperior
                }).ToList()
            };
        }

        public ICollection<PermissaoViewModel> BuscarPermissoes(int? idPerfil)
        {
            if (!idPerfil.HasValue || idPerfil == 0) return new List<PermissaoViewModel>();

            var permissoes = _permissaoService.Buscar(new PermissaoFiltro { IdPerfil = idPerfil });
            return permissoes.Lista.Select(x => new PermissaoViewModel
            {
                Action = x.Action,
                Controller = x.Controller,
                Descricao = x.Descricao,
                IdPermissao = x.IdPermissao,
                IdPerfil = idPerfil.Value
            }).ToList();
        }

        public UsuarioViewModel ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UsuarioViewModel> ObterTodos(int idUnidade, bool incluirSubordinados = false)
        {
            return Mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioViewModel>>(_usuarioService.ObterTodosPorUnidade(idUnidade, incluirSubordinados));
        }
        public IEnumerable<UsuarioViewModel> ObterTodos()
        {
            return Mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioViewModel>>(_usuarioService.ObterTodosDaBase());
        }

        public UsuarioViewModel Remover(UsuarioViewModel usuarioViewModel)
        {
            throw new NotImplementedException();
        }

        public List<Domain.Enums.Perfil> ObterPerfis(UsuarioViewModel usuario)
        {
            return new List<Domain.Enums.Perfil>();
        }

        public UsuarioViewModel TornarRemoverAdministrador(UsuarioViewModel usuario, bool admin)
        {
            // usuario.Administrador = admin;
            if (usuario.ValidationResult.IsValid)
                if (admin)
                {
                    var adm = new Administrador();
                    adm.CPF = usuario.CPF;
                    BeginTransaction();
                    _admService.Adicionar(adm);
                    Commit();
                    usuario.ValidationResult.Message = PGD.Domain.Constantes.Mensagens.MS_001;
                }
                else
                {
                    var obj = new Administrador();
                    obj = _admService.ObterTodosAdm().FirstOrDefault(a => a.CPF.Equals(usuario.CPF));
                    BeginTransaction();
                    _admService.Remover(obj);
                    Commit();
                    usuario.ValidationResult.Message = PGD.Domain.Constantes.Mensagens.MS_002;
                }
            return usuario;
        }

        public IEnumerable<UsuarioViewModel> ObterTodosAdministradores()
        {
            return Mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioViewModel>>(_usuarioService.ObterTodosAdministradores());
        }

        public bool PodeSelecionarPerfil(UsuarioViewModel usuario)
        {
            return usuario.PerfisUnidades.Select(x => x.PerfilEnum).Distinct().ToList().Count > 1;
        }

        public bool PodeSelecionarUnidade(UsuarioViewModel usuario)
        {
            // RNG007 se o perfil for Dirigente ou Administrador e possuir mais de uma unidade, selecionar unidade
            if (usuario.PerfilSelecionado == Domain.Enums.Perfil.Dirigente)
            {
                var idPerfilSelecionado = usuario.IdPerfilSelecionado;
                return usuario.PerfisUnidades.Where(x => x.IdPerfil == idPerfilSelecionado).Select(x => x.IdUnidade).Distinct().Count() > 1;
            }

            return false;
        }

        List<Domain.Enums.Perfil> IUsuarioAppService.ObterPerfis(UsuarioViewModel usuario)
        {
            throw new NotImplementedException();
        }

        public PaginacaoViewModel<UsuarioPerfilUnidadeViewModel> BuscarPerfilUnidade(UsuarioPerfilUnidadeFiltroViewModel filtro)
        {
            var retorno = _usuarioPerfilUnidadeService.Buscar(Mapper.Map<UsuarioPerfilUnidadeFiltro>(filtro));
            return new PaginacaoViewModel<UsuarioPerfilUnidadeViewModel>
            {
                QtRegistros = retorno.QtRegistros,
                Lista = retorno.Lista.Select(x => new UsuarioPerfilUnidadeViewModel
                {
                    Id = x.Id,
                    IdUnidade = x.IdUnidade,
                    CpfUsuario = x.Usuario?.Cpf.MaskCpfCpnj(),
                    IdPerfil = x.IdPerfil,
                    IdUsuario = x.IdUsuario,
                    MatriculaUsuario = x.Usuario?.Matricula,
                    NomePerfil = x.Perfil?.Nome,
                    NomeUnidade = x.Unidade?.Nome,
                    NomeUsuario = x.Usuario?.Nome,
                    SiglaUnidade = x.Unidade?.Sigla
                }).ToList()
            };
        }

        public VincularPerfilUsuarioViewModel VincularUnidadePerfil(VincularPerfilUsuarioViewModel model, string cpfUsuarioLogado)
        {
            var usuarioPerfilUnidade = GetNovoUsuarioPerfilUnidade(model);

            using (var tran = BeginDbTransaction())
            {
                try
                {
                    var retorno = _usuarioPerfilUnidadeService.Adicionar(usuarioPerfilUnidade);
                    model.ValidationResult = retorno.ValidationResult;

                    if (!retorno.ValidationResult.IsValid)
                        return model;

                    _logService.Logar(retorno, cpfUsuarioLogado, Domain.Enums.Operacao.Inclusão.ToString());
                    Commit(tran);
                }
                catch (Exception ex)
                {
                    model.ValidationResult.Add(new DomainValidation.Validation.ValidationError(ex.Message));
                    Rollback(tran);
                }
            }

            return model;
        }

        public VincularPerfilUsuarioViewModel RemoverVinculoUnidadePerfil(long idUsuarioPerfilUnidade, string cpfUsuarioLogado)
        {
            var retorno = new VincularPerfilUsuarioViewModel();
            if (idUsuarioPerfilUnidade == 0)
                return retorno;

            BeginTransaction();
            var retornoUsuarioPerfilUnidade = _usuarioPerfilUnidadeService.Remover(idUsuarioPerfilUnidade);
            if (retorno.ValidationResult.IsValid)
            {
                _logService.Logar(retornoUsuarioPerfilUnidade, cpfUsuarioLogado, Domain.Enums.Operacao.Exclusão.ToString());
                Commit();
            }

            retorno.ValidationResult = retornoUsuarioPerfilUnidade.ValidationResult;
            return retorno;
        }

        private UsuarioPerfilUnidade GetNovoUsuarioPerfilUnidade(VincularPerfilUsuarioViewModel model)
        {
            return new UsuarioPerfilUnidade
            {
                IdPerfil = model.IdPerfil.GetValueOrDefault(0),
                IdUsuario = model.IdUsuario,
                IdUnidade = model.IdUnidade.GetValueOrDefault(0)
            };
        }
    }
}
