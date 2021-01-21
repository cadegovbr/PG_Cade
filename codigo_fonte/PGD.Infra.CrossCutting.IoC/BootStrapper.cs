using Ninject;
using Ninject.Web.Common;
using PGD.Application;
using PGD.Application.Interfaces;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Services;
using PGD.Infra.Data.Context;
using PGD.Infra.Data.Interfaces;
using PGD.Infra.Data.Repository;
using PGD.Infra.Data.UoW;

namespace PGD.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(IKernel kernel)
        {
            // Lifestyle.Transient ---> Uma instancia para cada solicitacao
            // Lifestyle.Singleton ---> Uma instancia unica para a classe
            // Lifestyle.Scoped ---> Uma instancia unica para o request

            // App
            kernel.Bind(typeof(IAtividadeAppService)).To(typeof(AtividadeAppService)).InRequestScope();
            kernel.Bind(typeof(IGrupoAtividadeAppService)).To(typeof(GrupoAtividadeAppService)).InRequestScope();
            kernel.Bind(typeof(IOrdemServicoAppService)).To(typeof(OrdemServicoAppService)).InRequestScope();
            kernel.Bind(typeof(ITipoAtividadeAppService)).To(typeof(TipoAtividadeAppService)).InRequestScope();
            kernel.Bind(typeof(IUsuarioAppService)).To(typeof(UsuarioAppService)).InRequestScope();
            kernel.Bind(typeof(IPactoAppService)).To(typeof(PactoAppService)).InRequestScope();
            kernel.Bind(typeof(IProdutoAppService)).To(typeof(ProdutoAppService)).InRequestScope();
            kernel.Bind(typeof(IHistoricoAppService)).To(typeof(HistoricoAppService)).InRequestScope();
            kernel.Bind(typeof(IAdministradorAppService)).To(typeof(AdministradorAppService)).InRequestScope();
            kernel.Bind(typeof(IRelatorioAppService)).To(typeof(RelatoriosAppService)).InRequestScope();
            kernel.Bind(typeof(ICronogramaAppService)).To(typeof(CronogramaAppService)).InRequestScope();
            kernel.Bind(typeof(IArquivoDadoBrutoAppService)).To(typeof(ArquivoDadoBrutoAppService)).InRequestScope();
            kernel.Bind(typeof(IJustificativaAppService)).To(typeof(JustificativaAppService)).InRequestScope();
            kernel.Bind(typeof(ISituacaoPactoAppService)).To(typeof(SituacaoPactoAppService)).InRequestScope();
            kernel.Bind(typeof(ISituacaoProdutoAppService)).To(typeof(SituacaoProdutoAppService)).InRequestScope();
            kernel.Bind(typeof(ITipoPactoAppService)).To(typeof(TipoPactoAppService)).InRequestScope();
            kernel.Bind(typeof(IIniciativaPlanoOperacionalAppService)).To(typeof(IniciativaPlanoOperacionalAppService)).InRequestScope();
            kernel.Bind(typeof(IAvaliacaoProdutoAppService)).To(typeof(AvaliacaoProdutoAppService)).InRequestScope();
            kernel.Bind(typeof(IUnidade_TipoPactoAppService)).To(typeof(Unidade_TipoPactoAppService)).InRequestScope();
            kernel.Bind(typeof(INotificadorAppService)).To(typeof(NotificadorAppService)).InRequestScope();
            kernel.Bind(typeof(ICriterioAvaliacaoAppService)).To(typeof(CriterioAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(IItemAvaliacaoAppService)).To(typeof(ItemAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(INotaAvaliacaoAppService)).To(typeof(NotaAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(INivelAvaliacaoAppService)).To(typeof(NivelAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(IOS_CriterioAvaliacaoAppService)).To(typeof(OS_CriterioAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(IOS_ItemAvaliacaoAppService)).To(typeof(OS_ItemAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(IUnidadeAppService)).To(typeof(UnidadeAppService)).InRequestScope();
            kernel.Bind(typeof(IFeriadoAppService)).To(typeof(FeriadoAppService)).InRequestScope();
            kernel.Bind(typeof(IPerfilAppService)).To(typeof(PerfilAppService)).InRequestScope();

            // Domain
            kernel.Bind(typeof(IAtividadeService)).To(typeof(AtividadeService)).InRequestScope();
            kernel.Bind(typeof(IGrupoAtividadeService)).To(typeof(GrupoAtividadeService)).InRequestScope();
            kernel.Bind(typeof(ILogService)).To(typeof(LogService)).InRequestScope();
            kernel.Bind(typeof(IOrdemServicoService)).To(typeof(OrdemServicoService)).InRequestScope();
            kernel.Bind(typeof(ITipoAtividadeService)).To(typeof(TipoAtividadeService)).InRequestScope();
            kernel.Bind(typeof(IUsuarioService)).To(typeof(UsuarioService)).InRequestScope();
            kernel.Bind(typeof(IOS_GrupoAtividadeService)).To(typeof(OS_GrupoAtividadeService)).InRequestScope();
            kernel.Bind(typeof(IPactoService)).To(typeof(PactoService)).InRequestScope();
            kernel.Bind(typeof(IRHService)).To(typeof(RHService)).InRequestScope();
            kernel.Bind(typeof(IProdutoService)).To(typeof(ProdutoService)).InRequestScope();
            kernel.Bind(typeof(IAnexoProdutoService)).To(typeof(AnexoProdutoService)).InRequestScope();
            kernel.Bind(typeof(ISituacaoProdutoService)).To(typeof(SituacaoProdutoService)).InRequestScope();
            kernel.Bind(typeof(IHistoricoService)).To(typeof(HistoricoService)).InRequestScope();
            kernel.Bind(typeof(ICronogramaService)).To(typeof(CronogramaService)).InRequestScope();
            kernel.Bind(typeof(IAdministradorService)).To(typeof(AdministradorService)).InRequestScope();
            kernel.Bind(typeof(IArquivoDadoBrutoService)).To(typeof(ArquivoDadoBrutoService)).InRequestScope();
            kernel.Bind(typeof(IJustificativaService)).To(typeof(JustificativaService)).InRequestScope();
            kernel.Bind(typeof(ISituacaoPactoService)).To(typeof(SituacaoPactoService)).InRequestScope();
            kernel.Bind(typeof(ITipoPactoService)).To(typeof(TipoPactoService)).InRequestScope();
            kernel.Bind(typeof(IIniciativaPlanoOperacionalService)).To(typeof(IniciativaPlanoOperacionalService)).InRequestScope();
            kernel.Bind(typeof(IParametroSistemaService)).To(typeof(ParametroSistemaService)).InRequestScope();
            kernel.Bind(typeof(IAvaliacaoProdutoService)).To(typeof(AvaliacaoProdutoService)).InRequestScope();
            kernel.Bind(typeof(IUnidade_TipoPactoService)).To(typeof(Unidade_TipoPactoService)).InRequestScope();
            kernel.Bind(typeof(ICriterioAvaliacaoService)).To(typeof(CriterioAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IItemAvaliacaoService)).To(typeof(ItemAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(INotaAvaliacaoService)).To(typeof(NotaAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(INivelAvaliacaoService)).To(typeof(NivelAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IOS_CriterioAvaliacaoService)).To(typeof(OS_CriterioAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IOS_ItemAvaliacaoService)).To(typeof(OS_ItemAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IUnidadeService)).To(typeof(UnidadeService)).InRequestScope();
            kernel.Bind(typeof(IFeriadoService)).To(typeof(FeriadoService)).InRequestScope();
            kernel.Bind(typeof(IPerfilService)).To(typeof(PerfilService)).InRequestScope();
            kernel.Bind(typeof(IPermissaoService)).To(typeof(PermissaoService)).InRequestScope();
            kernel.Bind(typeof(IUsuarioPerfilUnidadeService)).To(typeof(UsuarioPerfilUnidadeService)).InRequestScope();

            // Infra Dados
            kernel.Bind(typeof(IAtividadeRepository)).To(typeof(AtividadeRepository)).InRequestScope();
            kernel.Bind(typeof(IGrupoAtividadeRepository)).To(typeof(GrupoAtividadeRepository)).InRequestScope();
            kernel.Bind(typeof(ILogRepository)).To(typeof(LogRepository)).InRequestScope();
            kernel.Bind(typeof(IOrdemServicoRepository)).To(typeof(OrdemServicoRepository)).InRequestScope();
            kernel.Bind(typeof(IUsuarioRepository)).To(typeof(UsuarioRepository)).InRequestScope();
            kernel.Bind(typeof(ITipoAtividadeRepository)).To(typeof(TipoAtividadeRepository)).InRequestScope();
            kernel.Bind(typeof(IOS_GrupoAtividadeRepository)).To(typeof(OS_GrupoAtividadeRepository)).InRequestScope();
            kernel.Bind(typeof(IPactoRepository)).To(typeof(PactoRepository)).InRequestScope();
            kernel.Bind(typeof(IProdutoRepository)).To(typeof(ProdutoRepository)).InRequestScope();
            kernel.Bind(typeof(IAnexoProdutoRepository)).To(typeof(IAnexoProdutoRepository)).InRequestScope();
            kernel.Bind(typeof(ICronogramaRepository)).To(typeof(CronogramaRepository)).InRequestScope();
            kernel.Bind(typeof(IHistoricoRepository)).To(typeof(HistoricoRepository)).InRequestScope();
            kernel.Bind(typeof(IAdministradorRepository)).To(typeof(AdministradorRepository)).InRequestScope();
            kernel.Bind(typeof(IArquivoDadoBrutoRepository)).To(typeof(ArquivoDadoBrutoRepository)).InRequestScope();
            kernel.Bind(typeof(IJustificativaRepository)).To(typeof(JustificativaRepository)).InRequestScope();
            kernel.Bind(typeof(ISituacaoPactoRepository)).To(typeof(SituacaoPactoRepository)).InRequestScope();
            kernel.Bind(typeof(ITipoPactoRepository)).To(typeof(TipoPactoRepository)).InRequestScope();
            kernel.Bind(typeof(IIniciativaPlanoOperacionalRepository)).To(typeof(IniciativaPlanoOperacionalRepository)).InRequestScope();
            kernel.Bind(typeof(IParametroSistemaRepository)).To(typeof(ParametroSistemaRepository)).InRequestScope();
            kernel.Bind(typeof(IAvaliacaoProdutoRepository)).To(typeof(AvaliacaoProdutoRepository)).InRequestScope();
            kernel.Bind(typeof(IAvaliacaoDetalhadaProdutoRepository)).To(typeof(AvaliacaoDetalhadaProdutoRepository)).InRequestScope();
            kernel.Bind(typeof(IUnidade_TipoPactoRepository)).To(typeof(Unidade_TipoPactoRepository)).InRequestScope();
            kernel.Bind(typeof(ICriterioAvaliacaoRepository)).To(typeof(CriterioAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IItemAvaliacaoRepository)).To(typeof(ItemAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(INotaAvaliacaoRepository)).To(typeof(NotaAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(INivelAvaliacaoRepository)).To(typeof(NivelAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IOS_CriterioAvaliacaoRepository)).To(typeof(OS_CriterioAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IOS_ItemAvaliacaoRepository)).To(typeof(OS_ItemAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IUnidadeRepository)).To(typeof(UnidadeRepository)).InRequestScope();
            kernel.Bind(typeof(IFeriadoRepository)).To(typeof(FeriadoRepository)).InRequestScope();
            kernel.Bind(typeof(IPerfilRepository)).To(typeof(PerfilRepository)).InRequestScope();
            kernel.Bind(typeof(IPermissaoRepository)).To(typeof(PermissaoRepository)).InRequestScope();
            kernel.Bind(typeof(IUsuarioPerfilUnidadeRepository)).To(typeof(UsuarioPerfilUnidadeRepository)).InRequestScope();
            kernel.Bind(typeof(ISituacaoProdutoRepository)).To(typeof(SituacaoProdutoRepository)).InRequestScope();

            kernel.Bind(typeof(IUnitOfWork)).To(typeof(UnitOfWork)).InRequestScope();
            kernel.Bind(typeof(PGDDbContext)).ToSelf().InRequestScope();

        }

        public static void RegisterServicesSingleton(IKernel kernel)
        {
            // App
            kernel.Bind(typeof(IAtividadeAppService)).To(typeof(AtividadeAppService)).InSingletonScope();
            kernel.Bind(typeof(IGrupoAtividadeAppService)).To(typeof(GrupoAtividadeAppService)).InSingletonScope();
            kernel.Bind(typeof(IOrdemServicoAppService)).To(typeof(OrdemServicoAppService)).InSingletonScope();
            kernel.Bind(typeof(ITipoAtividadeAppService)).To(typeof(TipoAtividadeAppService)).InSingletonScope();
            kernel.Bind(typeof(IUsuarioAppService)).To(typeof(UsuarioAppService)).InSingletonScope();
            kernel.Bind(typeof(IPactoAppService)).To(typeof(PactoAppService)).InSingletonScope();
            kernel.Bind(typeof(IProdutoAppService)).To(typeof(ProdutoAppService)).InSingletonScope();
            kernel.Bind(typeof(IHistoricoAppService)).To(typeof(HistoricoAppService)).InSingletonScope();
            kernel.Bind(typeof(IAdministradorAppService)).To(typeof(AdministradorAppService)).InSingletonScope();
            kernel.Bind(typeof(IRelatorioAppService)).To(typeof(RelatoriosAppService)).InSingletonScope();
            kernel.Bind(typeof(ICronogramaAppService)).To(typeof(CronogramaAppService)).InSingletonScope();
            kernel.Bind(typeof(IArquivoDadoBrutoAppService)).To(typeof(ArquivoDadoBrutoAppService)).InSingletonScope();
            kernel.Bind(typeof(IJustificativaAppService)).To(typeof(JustificativaAppService)).InSingletonScope();
            kernel.Bind(typeof(ISituacaoPactoAppService)).To(typeof(SituacaoPactoAppService)).InSingletonScope();
            kernel.Bind(typeof(ITipoPactoAppService)).To(typeof(TipoPactoAppService)).InSingletonScope();
            kernel.Bind(typeof(IIniciativaPlanoOperacionalAppService)).To(typeof(IniciativaPlanoOperacionalAppService)).InSingletonScope();
            kernel.Bind(typeof(IAvaliacaoProdutoAppService)).To(typeof(AvaliacaoProdutoAppService)).InSingletonScope();
            kernel.Bind(typeof(IUnidade_TipoPactoAppService)).To(typeof(Unidade_TipoPactoAppService)).InRequestScope();
            kernel.Bind(typeof(INotificadorAppService)).To(typeof(NotificadorAppService)).InRequestScope();
            kernel.Bind(typeof(ICriterioAvaliacaoAppService)).To(typeof(CriterioAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(IItemAvaliacaoAppService)).To(typeof(ItemAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(INotaAvaliacaoAppService)).To(typeof(NotaAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(INivelAvaliacaoAppService)).To(typeof(NivelAvaliacaoAppService)).InRequestScope();
            kernel.Bind(typeof(IUnidadeAppService)).To(typeof(UnidadeAppService)).InRequestScope();
            kernel.Bind(typeof(IFeriadoAppService)).To(typeof(FeriadoAppService)).InRequestScope();
            kernel.Bind(typeof(IPerfilAppService)).To(typeof(PerfilAppService)).InRequestScope();

            // Domain
            kernel.Bind(typeof(IAtividadeService)).To(typeof(AtividadeService)).InSingletonScope();
            kernel.Bind(typeof(IGrupoAtividadeService)).To(typeof(GrupoAtividadeService)).InSingletonScope();
            kernel.Bind(typeof(ILogService)).To(typeof(LogService)).InSingletonScope();
            kernel.Bind(typeof(IOrdemServicoService)).To(typeof(OrdemServicoService)).InSingletonScope();
            kernel.Bind(typeof(ITipoAtividadeService)).To(typeof(TipoAtividadeService)).InSingletonScope();
            kernel.Bind(typeof(IUsuarioService)).To(typeof(UsuarioService)).InSingletonScope();
            kernel.Bind(typeof(IOS_GrupoAtividadeService)).To(typeof(OS_GrupoAtividadeService)).InSingletonScope();
            kernel.Bind(typeof(IPactoService)).To(typeof(PactoService)).InSingletonScope();
            kernel.Bind(typeof(IRHService)).To(typeof(RHService)).InSingletonScope();
            kernel.Bind(typeof(IProdutoService)).To(typeof(ProdutoService)).InSingletonScope();
            kernel.Bind(typeof(IAnexoProdutoService)).To(typeof(IAnexoProdutoService)).InSingletonScope();
            kernel.Bind(typeof(ISituacaoProdutoService)).To(typeof(ISituacaoProdutoService)).InSingletonScope();
            kernel.Bind(typeof(IHistoricoService)).To(typeof(HistoricoService)).InSingletonScope();
            kernel.Bind(typeof(ICronogramaService)).To(typeof(CronogramaService)).InSingletonScope();
            kernel.Bind(typeof(IAdministradorService)).To(typeof(AdministradorService)).InSingletonScope();
            kernel.Bind(typeof(IArquivoDadoBrutoService)).To(typeof(ArquivoDadoBrutoService)).InSingletonScope();
            kernel.Bind(typeof(IJustificativaService)).To(typeof(JustificativaService)).InSingletonScope();
            kernel.Bind(typeof(ISituacaoPactoService)).To(typeof(SituacaoPactoService)).InSingletonScope();
            kernel.Bind(typeof(ITipoPactoService)).To(typeof(TipoPactoService)).InSingletonScope();
            kernel.Bind(typeof(IIniciativaPlanoOperacionalService)).To(typeof(IniciativaPlanoOperacionalService)).InSingletonScope();
            kernel.Bind(typeof(IParametroSistemaService)).To(typeof(ParametroSistemaService)).InSingletonScope();
            kernel.Bind(typeof(IAvaliacaoProdutoService)).To(typeof(AvaliacaoProdutoService)).InSingletonScope();
            kernel.Bind(typeof(IUnidade_TipoPactoService)).To(typeof(Unidade_TipoPactoService)).InRequestScope();
            kernel.Bind(typeof(ICriterioAvaliacaoService)).To(typeof(CriterioAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IItemAvaliacaoService)).To(typeof(ItemAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(INotaAvaliacaoService)).To(typeof(NotaAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(INivelAvaliacaoService)).To(typeof(NivelAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IOS_CriterioAvaliacaoService)).To(typeof(OS_CriterioAvaliacaoService)).InRequestScope();
            kernel.Bind(typeof(IUnidadeService)).To(typeof(UnidadeService)).InRequestScope();
            kernel.Bind(typeof(IFeriadoService)).To(typeof(FeriadoService)).InRequestScope();
            kernel.Bind(typeof(IPerfilService)).To(typeof(PerfilService)).InRequestScope();

            // Infra Dados
            kernel.Bind(typeof(IAtividadeRepository)).To(typeof(AtividadeRepository)).InSingletonScope();
            kernel.Bind(typeof(IGrupoAtividadeRepository)).To(typeof(GrupoAtividadeRepository)).InSingletonScope();
            kernel.Bind(typeof(ILogRepository)).To(typeof(LogRepository)).InSingletonScope();
            kernel.Bind(typeof(IOrdemServicoRepository)).To(typeof(OrdemServicoRepository)).InSingletonScope();
            kernel.Bind(typeof(IUsuarioRepository)).To(typeof(UsuarioRepository)).InSingletonScope();
            kernel.Bind(typeof(ITipoAtividadeRepository)).To(typeof(TipoAtividadeRepository)).InSingletonScope();
            kernel.Bind(typeof(IOS_GrupoAtividadeRepository)).To(typeof(OS_GrupoAtividadeRepository)).InSingletonScope();
            kernel.Bind(typeof(IPactoRepository)).To(typeof(PactoRepository)).InSingletonScope();
            kernel.Bind(typeof(IProdutoRepository)).To(typeof(ProdutoRepository)).InSingletonScope();
            kernel.Bind(typeof(ICronogramaRepository)).To(typeof(CronogramaRepository)).InSingletonScope();
            kernel.Bind(typeof(IHistoricoRepository)).To(typeof(HistoricoRepository)).InSingletonScope();
            kernel.Bind(typeof(IAdministradorRepository)).To(typeof(AdministradorRepository)).InSingletonScope();
            kernel.Bind(typeof(IArquivoDadoBrutoRepository)).To(typeof(ArquivoDadoBrutoRepository)).InSingletonScope();
            kernel.Bind(typeof(IJustificativaRepository)).To(typeof(JustificativaRepository)).InSingletonScope();
            kernel.Bind(typeof(ISituacaoPactoRepository)).To(typeof(SituacaoPactoRepository)).InSingletonScope();
            kernel.Bind(typeof(ITipoPactoRepository)).To(typeof(TipoPactoRepository)).InSingletonScope();
            kernel.Bind(typeof(IIniciativaPlanoOperacionalRepository)).To(typeof(IniciativaPlanoOperacionalRepository)).InSingletonScope();
            kernel.Bind(typeof(IParametroSistemaRepository)).To(typeof(ParametroSistemaRepository)).InSingletonScope();
            kernel.Bind(typeof(IAvaliacaoProdutoRepository)).To(typeof(AvaliacaoProdutoRepository)).InSingletonScope();
            kernel.Bind(typeof(IAvaliacaoDetalhadaProdutoRepository)).To(typeof(AvaliacaoDetalhadaProdutoRepository)).InSingletonScope();
            kernel.Bind(typeof(IUnidade_TipoPactoRepository)).To(typeof(Unidade_TipoPactoRepository)).InSingletonScope();
            kernel.Bind(typeof(ICriterioAvaliacaoRepository)).To(typeof(CriterioAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IItemAvaliacaoRepository)).To(typeof(ItemAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(INotaAvaliacaoRepository)).To(typeof(NotaAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(INivelAvaliacaoRepository)).To(typeof(NivelAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IOS_CriterioAvaliacaoRepository)).To(typeof(OS_CriterioAvaliacaoRepository)).InRequestScope();
            kernel.Bind(typeof(IUnidadeRepository)).To(typeof(UnidadeRepository)).InRequestScope();
            kernel.Bind(typeof(IFeriadoRepository)).To(typeof(FeriadoRepository)).InRequestScope();
            kernel.Bind(typeof(IPerfilRepository)).To(typeof(PerfilRepository)).InRequestScope();
            kernel.Bind(typeof(ISituacaoProdutoRepository)).To(typeof(SituacaoProdutoRepository)).InRequestScope();

            kernel.Bind(typeof(IUnitOfWork)).To(typeof(UnitOfWork)).InSingletonScope();
            kernel.Bind(typeof(PGDDbContext)).ToSelf().InSingletonScope();

        }
    }
}

