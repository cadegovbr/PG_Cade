using CGU.Util;
using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using PGD.Infra.Data.EntityConfig;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PGD.Infra.Data.Context
{
    public class PGDDbContext : DbContext
    {

        public PGDDbContext()
            : base("PGDConnectionString")
        {
            Database.SetInitializer<PGDDbContext>(null);
        }
        public PGDDbContext(string connectionstring)
            : base(connectionstring)
        {
            Database.SetInitializer<PGDDbContext>(null);
        }

        public DbSet<NotaAvaliacao> NotaAvaliacao { get; set; }
        public DbSet<CriterioAvaliacao> CriterioAvaliacao { get; set; }
        public DbSet<ItemAvaliacao> ItemAvaliacao { get; set; }
        public DbSet<Atividade> Atividade { get; set; }
        public DbSet<TipoAtividade> TipoAtividade { get; set; }
        public DbSet<GrupoAtividade> GrupoAtividade { get; set; }
        public DbSet<OrdemServico> OrdemServico { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Historico> Historico { get; set; }
        public DbSet<Administrador> Administrador { get; set; }
        public DbSet<TipoPacto> TipoPacto { get; set; }
        public DbSet<Unidade_TipoPacto> Unidade_TipoPacto { get; set; }
        public DbSet<NivelAvaliacao> NivelAvaliacao { get; set; }

        #region CSU006_CSU008
        public DbSet<Pacto> Pacto { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Justificativa> Justificativa { get; set; }
        public DbSet<SituacaoPacto> SituacaoPacto { get; set; }
        public DbSet<Cronograma> Cronograma { get; set; }
        #endregion

        public DbSet<OS_CriterioAvaliacao> OS_CriterioAvaliacao { get; set; }
        public DbSet<OS_Atividade> OS_Atividade { get; set; }        
        public DbSet<OS_TipoAtividade> OS_TipoAtividade { get; set; }
        public DbSet<OS_GrupoAtividade> OS_GrupoAtividade { get; set; }
        public DbSet<ParametroSistema> ParametrosSistema { get; set; }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<Feriado> Feriado { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.Configuration.LazyLoadingEnabled = false;

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(p => p.IsKey());

            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(p => p.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));

            modelBuilder.Configurations.Add(new AtividadeConfig());
            modelBuilder.Configurations.Add(new AvaliacaoProdutoConfig());
            modelBuilder.Configurations.Add(new TipoAtividadeConfig());
            modelBuilder.Configurations.Add(new GrupoAtividadeConfig());
            modelBuilder.Configurations.Add(new OrdemServicoConfig());
            modelBuilder.Configurations.Add(new LogConfig());
            modelBuilder.Configurations.Add(new PactoConfig());
            modelBuilder.Configurations.Add(new ParametroSistemaConfig());
            modelBuilder.Configurations.Add(new ProdutoConfig());
            modelBuilder.Configurations.Add(new AnexoProdutoConfig());
            modelBuilder.Configurations.Add(new SituacaoProdutoConfig());
            modelBuilder.Configurations.Add(new JustificativaConfig());
            modelBuilder.Configurations.Add(new SituacaoPactoConfig());
            modelBuilder.Configurations.Add(new CronogramaConfig());
            modelBuilder.Configurations.Add(new AdminstradorConfig());
            modelBuilder.Configurations.Add(new IniciativaPlanoOperacionalConfig());
            modelBuilder.Configurations.Add(new TipoPactoConfig());
            modelBuilder.Configurations.Add(new Unidade_TipoPactoConfig());
            modelBuilder.Configurations.Add(new NivelAvaliacaoConfig());

            modelBuilder.Configurations.Add(new OS_AtividadeConfig());
            modelBuilder.Configurations.Add(new OS_TipoAtividadeConfig());
            modelBuilder.Configurations.Add(new OsGrupoAtividadeConfig());

            modelBuilder.Configurations.Add(new NotaAvaliacaoConfig());
            modelBuilder.Configurations.Add(new CriterioAvaliacaoConfig());
            modelBuilder.Configurations.Add(new ItemAvaliacaoConfig());
            modelBuilder.Configurations.Add(new OS_CriterioAvaliacaoConfig());
            modelBuilder.Configurations.Add(new OS_ItemAvaliacaoConfig());
            modelBuilder.Configurations.Add(new AvaliacaoDetalhadaProdutoConfig());

            modelBuilder.Configurations.Add(new UsuarioConfig());
            modelBuilder.Configurations.Add(new UnidadeConfig());
            modelBuilder.Configurations.Add(new FeriadoConfig());

            modelBuilder.Configurations.Add(new HistoricoConfig());

            modelBuilder.Entity<GrupoAtividade>()
                .HasMany<Atividade>(s => s.Atividades)
                .WithMany(c => c.Grupos)
                .Map(cs =>
                {
                    cs.MapLeftKey("IdGrupoAtividade");
                    cs.MapRightKey("IdAtividade");
                    cs.ToTable("GrupoAtividade_Atividade");
                });

            modelBuilder.Entity<GrupoAtividade>()
               .HasMany<TipoPacto>(s => s.TiposPacto)
               .WithMany(c => c.Grupos)
               .Map(cs =>
               {
                   cs.MapLeftKey("IdGrupoAtividade");
                   cs.MapRightKey("IdTipoPacto");
                   cs.ToTable("TipoPactoGrupoAtividade");
               });

            modelBuilder.Entity<OS_GrupoAtividade>()
               .HasMany<TipoPacto>(s => s.TiposPacto)
               .WithMany(c => c.OS_Grupos)
               .Map(cs =>
               {
                   cs.MapLeftKey("IdGrupoAtividade");
                   cs.MapRightKey("IdTipoPacto");
                   cs.ToTable("OS_TipoPacto_GrupoAtividade");
               });
            
            modelBuilder.Configurations.Add(new PerfilConfig());
            modelBuilder.Configurations.Add(new UsuarioPerfilUnidadeConfig());

            base.OnModelCreating(modelBuilder);

        }

        public void AtivarLog(bool logAtivo = true)
        {
            // if (logAtivo) (this as DbContext).Database.Log = message => LogManagerComum.LogarDebug(mensagem: message);
            // else 
                (this as DbContext).Database.Log = null;
        }
    }

    public static class ChangeDb
    {
        public static void ChangeConnection(this PGDDbContext context, string cn)
        {
            context.Database.Connection.ConnectionString = cn;
        }
    }
}
