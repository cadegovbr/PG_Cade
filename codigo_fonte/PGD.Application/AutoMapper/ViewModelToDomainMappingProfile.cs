using AutoMapper;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using System;
using PGD.Application.ViewModels.Filtros;
using PGD.Application.ViewModels.Filtros.Base;
using PGD.Domain.Filtros;
using PGD.Domain.Filtros.Base;

namespace PGD.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            // trim todas strings
            CreateMap<string, string>()
                .ConvertUsing(x => (x ?? "").Trim());

            CreateMap<AtividadeViewModel, Atividade>();
            CreateMap<AtividadeViewModel, OS_Atividade>();            
            CreateMap<TipoAtividadeViewModel, TipoAtividade>();
            CreateMap<TipoAtividadeViewModel, OS_TipoAtividade>();
            CreateMap<GrupoAtividadeViewModel, GrupoAtividade>();
            CreateMap<GrupoAtividadeViewModel, OS_GrupoAtividade>();
            CreateMap<UsuarioViewModel, Usuario>();
            CreateMap<OrdemServicoViewModel, OrdemServico>();
            CreateMap<PactoViewModel,Pacto >()
                .ForMember(dest => dest.CargaHoraria, opt => opt.MapFrom(src => src.CargaHorariaDiaria.TotalHours))
                .ForMember(p => p.ValidationResult, opt => opt.MapFrom(src => new DomainValidation.Validation.ValidationResult()));
            CreateMap<ProdutoViewModel, Produto>()
                .ForMember(p => p.Atividade, opt => opt.Ignore())
                .ForMember(p => p.GrupoAtividade, opt => opt.Ignore())
                .ForMember(p => p.TipoAtividade, opt => opt.Ignore())
                .ForMember(p => p.Justificativa, opt => opt.Ignore());
            CreateMap<JustificativaViewModel, Justificativa>();
            CreateMap<SituacaoPactoViewModel, SituacaoPacto>();
            CreateMap<TipoPactoViewModel, TipoPacto>();
            CreateMap<HistoricoViewModel, Historico>();
            CreateMap<CronogramaViewModel, Cronograma>()
                .ForMember(dest => dest.HorasCronograma, opt => opt.MapFrom(src => src.HorasCronograma.TotalHours))
                .ForMember(dest => dest.DuracaoFeriado, opt => opt.MapFrom(src => src.DuracaoFeriado.TotalHours));
            CreateMap<FeriadoViewModel, Feriado>();
            CreateMap<ArquivoDadoBrutoViewModel, ArquivoDadoBruto>();
            CreateMap<CronogramaPactoViewModel, CronogramaPacto>()
                .ForMember(dest => dest.HorasDiarias, opt => opt.MapFrom(src => src.HorasDiarias.TotalHours));
            CreateMap<IniciativaPlanoOperacionalViewModel, IniciativaPlanoOperacional>();
            CreateMap<IniciativaPlanoOperacionalProdutoViewModel, IniciativaPlanoOperacionalProduto>();
            CreateMap<AvaliacaoProdutoViewModel, AvaliacaoProduto>()
                .ForMember(dest => dest.Produto, opt => opt.Ignore());
            CreateMap<Unidade_TipoPactoViewModel, Unidade_TipoPacto>();
            CreateMap<NotaAvaliacaoViewModel, NotaAvaliacao>();            
            CreateMap<CriterioAvaliacaoViewModel, CriterioAvaliacao>();
            CreateMap<OS_CriterioAvaliacaoViewModel, OS_CriterioAvaliacao>();
            CreateMap<CriterioAvaliacaoViewModel, OS_CriterioAvaliacao>();
            CreateMap<ItemAvaliacaoViewModel, ItemAvaliacao>();
            CreateMap<ItemAvaliacaoViewModel, OS_ItemAvaliacao>();
            CreateMap<NivelAvaliacaoViewModel, NivelAvaliacao>();
            CreateMap<AvaliacaoDetalhadaProdutoViewModel, AvaliacaoDetalhadaProduto>();
            CreateMap<OS_ItemAvaliacaoViewModel, OS_ItemAvaliacao>();

            // Filtros
            CreateMap<UsuarioFiltroViewModel, UsuarioFiltro>();
            CreateMap<UnidadeFiltroViewModel, UnidadeFiltro>();
            CreateMap<UsuarioPerfilUnidadeFiltroViewModel, UsuarioPerfilUnidadeFiltro>();
            CreateMap<UnidadeTipoPactoFiltroViewModel, UnidadeTipoPactoFiltro>();
        }


    }
}
