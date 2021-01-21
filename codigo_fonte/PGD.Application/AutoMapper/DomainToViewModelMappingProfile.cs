using AutoMapper;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using PGD.Application.ViewModels.Filtros;
using PGD.Application.ViewModels.Filtros.Base;
using PGD.Application.ViewModels.Paginacao;
using PGD.Domain.Filtros;
using PGD.Domain.Filtros.Base;
using PGD.Domain.Paginacao;

namespace PGD.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Atividade, AtividadeViewModel>();
            CreateMap<OS_Atividade, AtividadeViewModel>();            
            CreateMap<TipoAtividade, TipoAtividadeViewModel>();
            CreateMap<OS_TipoAtividade, TipoAtividadeViewModel>();
            CreateMap<GrupoAtividade, GrupoAtividadeViewModel>();
            CreateMap<OS_GrupoAtividade, GrupoAtividadeViewModel>();
            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(x => x.PerfisUnidades, opt => opt.MapFrom(src =>

                    src.UsuariosPerfisUnidades == null
                        ? new List<UsuarioPerfilUnidadeViewModel>()
                        : src.UsuariosPerfisUnidades.Where(x => !x.Excluido).Select(x => new UsuarioPerfilUnidadeViewModel
                        {
                            Id = x.Id,
                            IdPerfil = x.IdPerfil,
                            IdUnidade = x.IdUnidade,
                            NomePerfil = x.Perfil.Nome,
                            NomeUnidade = x.Unidade.Nome,
                            SiglaUnidade = x.Unidade.Sigla,
                            IdUsuario = x.IdUsuario
                        }).ToList()
                ));
            CreateMap<OrdemServico, OrdemServicoViewModel>();
            CreateMap<Pacto, PactoViewModel>()
                .ForMember(dest => dest.CargaHorariaDiaria, opt => opt.MapFrom(src => TimeSpan.FromHours(src.CargaHoraria)))
                .ForMember(p => p.Avaliacoes, opt => opt.Ignore());
            CreateMap<Feriado, FeriadoViewModel>();
            CreateMap<Produto, ProdutoViewModel>();
            CreateMap<Justificativa, JustificativaViewModel>();
            CreateMap<SituacaoPacto, SituacaoPactoViewModel>();
            CreateMap<TipoPacto, TipoPactoViewModel>();
            CreateMap<Cronograma, CronogramaViewModel>()
                .ForMember(dest => dest.HorasCronograma, opt => opt.MapFrom(src => TimeSpan.FromHours( Convert.ToDouble(src.HorasCronograma))))
                .ForMember(dest => dest.DuracaoFeriado, opt => opt.MapFrom(src => TimeSpan.FromHours(src.DuracaoFeriado.HasValue? Convert.ToDouble(src.DuracaoFeriado.Value) : 0)));
            CreateMap<Historico, HistoricoViewModel>();
            CreateMap<ArquivoDadoBruto, ArquivoDadoBrutoViewModel>();
            CreateMap<CronogramaPacto, CronogramaPactoViewModel>()
                .ForMember(dest => dest.HorasDiarias, opt => opt.MapFrom(src => TimeSpan.FromHours(src.HorasDiarias)));
            CreateMap<IniciativaPlanoOperacional, IniciativaPlanoOperacionalViewModel>();
            CreateMap<IniciativaPlanoOperacionalProduto, IniciativaPlanoOperacionalProdutoViewModel>();
            CreateMap<AvaliacaoProduto, AvaliacaoProdutoViewModel>();
            CreateMap<Unidade_TipoPacto, Unidade_TipoPactoViewModel>()
                .ForMember(x => x.NomeUnidade, opt => opt.MapFrom(src => src.Unidade == null ? null : src.Unidade.Nome))
                .ForMember(x => x.DescTipoPacto, opt => opt.MapFrom(src => src.TipoPacto == null ? null : src.TipoPacto.DescTipoPacto));

            CreateMap<NotaAvaliacao, NotaAvaliacaoViewModel>();
            
            CreateMap<CriterioAvaliacao, CriterioAvaliacaoViewModel>();
            CreateMap<OS_CriterioAvaliacao, CriterioAvaliacaoViewModel>();
            CreateMap<OS_CriterioAvaliacao, OS_CriterioAvaliacaoViewModel>();            
            CreateMap<ItemAvaliacao, ItemAvaliacaoViewModel>();
            CreateMap<OS_ItemAvaliacao, ItemAvaliacaoViewModel>();
            CreateMap<NivelAvaliacao, NivelAvaliacaoViewModel>();
            CreateMap<AvaliacaoDetalhadaProduto, AvaliacaoDetalhadaProdutoViewModel>();
            CreateMap<OS_ItemAvaliacao, OS_ItemAvaliacaoViewModel>();

            // Paginacao
            CreateMap(typeof(Paginacao<>), typeof(PaginacaoViewModel<>));
        }
    }
}
