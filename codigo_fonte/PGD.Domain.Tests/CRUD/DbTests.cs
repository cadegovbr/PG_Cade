using NUnit.Framework;
using PGD.Domain.Entities;
using PGD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{
    public class DbTests
    {
        readonly string connectionString = "PGDConnectionString";
        // you don't want any of these executed automatically
        [Test, Ignore("Somente para execução manual")]
        public void Limpa_E_Cria_Database()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            // drop database first
            ReallyDropDatabase(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString);

            // Now time to create the database from migrations
            // MyApp.Data.Migrations.Configuration is migration configuration class 
            // this class is crated for you automatically when you enable migrations
            var initializer = new MigrateDatabaseToLatestVersion<PGDDbContext, EF6Config>();

            // set DB initialiser to execute migrations
            Database.SetInitializer(initializer);

            // now actually force the initialisation to happen
            using (var domainContext = new PGDDbContext(connectionString))
            {
                Debug.WriteLine("Iniciando criação database");
                domainContext.Database.Initialize(true);
                Debug.WriteLine("Database foi criado");
            }

            // And after the DB is created, you can put some initial base data 
            // for your tests to use
            // usually this data represents lookup tables, like Currencies, Countries, Units of Measure, etc
            using (var domainContext = new PGDDbContext(connectionString))
            {
                Debug.WriteLine("Seeding test data into database");
                // discussion for that to follow
                Update_Database();
                Debug.WriteLine("Seeding test data is complete");
            }
        }

        // this method is only updates your DB to latest migration.
        // does the same as if you run "Update-Database" in nuget console in Visual Studio
        [Test, Ignore("Somente para execução manual")]
        public void Update_Database()
        {

            var migrationConfiguration = new EF6Config();

            migrationConfiguration.TargetDatabase = new DbConnectionInfo(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString, "System.Data.SqlClient");

            var migrator = new DbMigrator(migrationConfiguration);

            migrator.Update();
        }
        public class EF6Config : DbMigrationsConfiguration<PGDDbContext>
        {
            public EF6Config()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }
            /// <summary>
            /// Cria um database com esses dados padrões
            /// </summary>
            /// <param name="context"></param>
            protected override void Seed(PGDDbContext context)
            {
                NotaAvaliacao nota1 = new NotaAvaliacao { IdNotaAvaliacao = 1, DescNotaAvaliacao = "Nota 1", LimiteInferiorFaixa = 9, LimiteSuperiorFaixa = 10 };
                NotaAvaliacao nota2 = new NotaAvaliacao { IdNotaAvaliacao = 2, DescNotaAvaliacao = "Nota 2", LimiteInferiorFaixa = 8, LimiteSuperiorFaixa = 9.9M };

                // Critério de Avaliação 

                context.CriterioAvaliacao.AddOrUpdate(p => p.IdCriterioAvaliacao, new CriterioAvaliacao
                {
                    DescCriterioAvaliacao = "Critério teste 1",
                    StrTextoExplicativo = "Texto explicativo 1",
                    ItensAvaliacao = new List<ItemAvaliacao> { new ItemAvaliacao { ImpactoNota = -0.1M, DescItemAvaliacao = "Desc1",  NotaMaxima =  nota1, IdNotaMaxima = nota1.IdNotaAvaliacao} }
                });

                context.CriterioAvaliacao.AddOrUpdate(p => p.IdCriterioAvaliacao, new CriterioAvaliacao
                {
                    DescCriterioAvaliacao = "Critério teste 2",
                    StrTextoExplicativo = "Texto explicativo 2",
                    ItensAvaliacao = new List<ItemAvaliacao> { new ItemAvaliacao { ImpactoNota = -0.2M, DescItemAvaliacao = "Desc2", NotaMaxima = nota2, IdNotaMaxima = nota2.IdNotaAvaliacao } }
                });

                context.CriterioAvaliacao.AddOrUpdate(p => p.IdCriterioAvaliacao, new CriterioAvaliacao
                {
                    DescCriterioAvaliacao = "Critério teste 3",
                    StrTextoExplicativo = "Texto explicativo 3",
                    ItensAvaliacao = new List<ItemAvaliacao> { new ItemAvaliacao { ImpactoNota = -0.4M, DescItemAvaliacao = "Desc3", NotaMaxima = nota2, IdNotaMaxima = nota2.IdNotaAvaliacao } }
                });

                context.SaveChanges();

                // Atividade 
                context.Atividade.AddOrUpdate(p => p.IdAtividade, new Atividade
                {
                    NomAtividade = "Atividade teste 1",
                    PctMinimoReducao = 30,
                    Tipos = new List<TipoAtividade> { new TipoAtividade { DuracaoFaixa = 15, DuracaoFaixaPresencial = 35, Faixa = "Faixa Teste 1" } }
                });
                context.Atividade.AddOrUpdate(p => p.IdAtividade, new Atividade
                {
                    NomAtividade = "Atividade teste 2",
                    PctMinimoReducao = 16,
                    Tipos = new List<TipoAtividade> { new TipoAtividade { DuracaoFaixa = 10, DuracaoFaixaPresencial = 20, Faixa = "Faixa Teste 1" }, new TipoAtividade { DuracaoFaixa = 10, DuracaoFaixaPresencial = 20, Faixa = "Faixa Teste 2" } }
                });
                context.Atividade.AddOrUpdate(p => p.IdAtividade, new Atividade
                {
                    NomAtividade = "Atividade teste 3",
                    PctMinimoReducao = 50,
                    Tipos = new List<TipoAtividade> { new TipoAtividade { DuracaoFaixa = 5, DuracaoFaixaPresencial = 32, Faixa = "Faixa Teste 1" }, new TipoAtividade { DuracaoFaixa = 24, DuracaoFaixaPresencial = 6, Faixa = "Faixa Teste 2" }, new TipoAtividade { DuracaoFaixa = 35, DuracaoFaixaPresencial = 30, Faixa = "Faixa Teste 3" } }
                });
                context.SaveChanges();

                //GrupoAtividade
                var Atividades = context.Atividade.ToListAsync().Result;

                context.GrupoAtividade.AddOrUpdate(x => x.IdGrupoAtividade, new GrupoAtividade
                {
                    NomGrupoAtividade = "Grupo Atividade Teste 1",
                    Atividades = new List<Atividade>() { Atividades[0], Atividades[1] }
                });
                context.GrupoAtividade.AddOrUpdate(x => x.IdGrupoAtividade, new GrupoAtividade
                {
                    NomGrupoAtividade = "Grupo Atividade Teste 2",
                    Atividades = new List<Atividade>() { Atividades[0], Atividades[2] }
                });
                context.GrupoAtividade.AddOrUpdate(x => x.IdGrupoAtividade, new GrupoAtividade
                {
                    NomGrupoAtividade = "Grupo Atividade Teste 3",
                    Atividades = new List<Atividade>() { Atividades[1] }
                });

                context.SaveChanges();

                //OS_Atividade
                context.OS_Atividade.AddOrUpdate(p => p.IdAtividade, new OS_Atividade
                {
                    NomAtividade = "Atividade teste 1",
                    PctMinimoReducao = 30,
                    Tipos = new List<OS_TipoAtividade> { new OS_TipoAtividade { DuracaoFaixa = 15, DuracaoFaixaPresencial = 35, Faixa = "Faixa Teste 1" } }
                });
                context.OS_Atividade.AddOrUpdate(p => p.IdAtividade, new OS_Atividade
                {
                    NomAtividade = "Atividade teste 2",
                    PctMinimoReducao = 16,
                    Tipos = new List<OS_TipoAtividade> { new OS_TipoAtividade { DuracaoFaixa = 10, DuracaoFaixaPresencial = 20, Faixa = "Faixa Teste 1" }, new OS_TipoAtividade { DuracaoFaixa = 10, DuracaoFaixaPresencial = 20, Faixa = "Faixa Teste 2" } }
                });
                context.OS_Atividade.AddOrUpdate(p => p.IdAtividade, new OS_Atividade
                {
                    NomAtividade = "Atividade teste 3",
                    PctMinimoReducao = 50,
                    Tipos = new List<OS_TipoAtividade> { new OS_TipoAtividade { DuracaoFaixa = 5, DuracaoFaixaPresencial = 32, Faixa = "Faixa Teste 1" }, new OS_TipoAtividade { DuracaoFaixa = 24, DuracaoFaixaPresencial = 6, Faixa = "Faixa Teste 2" }, new OS_TipoAtividade { DuracaoFaixa = 35, DuracaoFaixaPresencial = 30, Faixa = "Faixa Teste 3" } }
                });
                context.SaveChanges();



                //OrdemServico
                context.OrdemServico.AddOrUpdate(x => x.IdOrdemServico, new OrdemServico
                {
                    DatInicioNormativo = new DateTime(2010, 6, 12),//12/06/2010
                    DatInicioSistema = new DateTime(2011, 7, 15),//15/07/2011
                    DescOrdemServico = "Ordem serviço teste 1",
                });
                context.SaveChanges();

                //OS_GrupoAtividade
                var os_Atividades = context.OS_Atividade.ToListAsync().Result;
                var os = context.OrdemServico.FirstOrDefault();

                context.OS_GrupoAtividade.AddOrUpdate(x => x.IdGrupoAtividade, new OS_GrupoAtividade
                {
                    NomGrupoAtividade = "Grupo Atividade Teste 1",
                    Atividades = new List<OS_Atividade>() { os_Atividades[0], os_Atividades[1] },
                    IdOrdemServico = os.IdOrdemServico,
                    IdGrupoAtividade = context.GrupoAtividade.FirstOrDefault(a => a.NomGrupoAtividade == "Grupo Atividade Teste 1").IdGrupoAtividade
                });
                context.OS_GrupoAtividade.AddOrUpdate(x => x.IdGrupoAtividade, new OS_GrupoAtividade
                {
                    NomGrupoAtividade = "Grupo Atividade Teste 2",
                    Atividades = new List<OS_Atividade>() { os_Atividades[0], os_Atividades[2] },
                    IdOrdemServico = os.IdOrdemServico,
                    IdGrupoAtividade = context.GrupoAtividade.FirstOrDefault(a => a.NomGrupoAtividade == "Grupo Atividade Teste 2").IdGrupoAtividade
                });
                context.OS_GrupoAtividade.AddOrUpdate(x => x.IdGrupoAtividade, new OS_GrupoAtividade
                {
                    NomGrupoAtividade = "Grupo Atividade Teste 3",
                    Atividades = new List<OS_Atividade>() { os_Atividades[1] },
                    IdOrdemServico = os.IdOrdemServico,
                    IdGrupoAtividade = context.GrupoAtividade.FirstOrDefault(a => a.NomGrupoAtividade == "Grupo Atividade Teste 3").IdGrupoAtividade
                });

                context.TipoPacto.AddOrUpdate(x => x.IdTipoPacto, new TipoPacto
                {
                    IdTipoPacto = 1,
                    DescTipoPacto = "PGD - Pontual"
                });

                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 1,
                    DescSituacaoPacto = "Pendente de Assinatura"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 2,
                    DescSituacaoPacto = "A Iniciar"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto =3,
                    DescSituacaoPacto = "Em Andamento"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 4,
                    DescSituacaoPacto = "Pendente de Avaliação"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 5,
                    DescSituacaoPacto = "Avaliado"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 6,
                    DescSituacaoPacto = "Negado"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 7,
                    DescSituacaoPacto = "Excluído"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 8,
                    DescSituacaoPacto = "Interrompido"
                });
                context.SituacaoPacto.AddOrUpdate(x => x.IdSituacaoPacto, new SituacaoPacto
                {
                    IdSituacaoPacto = 8,
                    DescSituacaoPacto = "Suspenso"
                });
                context.ParametrosSistema.AddOrUpdate(p => p.IdParametroSistema, new ParametroSistema()
                {
                    IdParametroSistema = 1,
                    DescParametroSistema = "Parametro 1",
                    Valor = "1"
                });
                context.ParametrosSistema.AddOrUpdate(p => p.IdParametroSistema, new ParametroSistema()
                {
                    IdParametroSistema = 2,
                    DescParametroSistema = "Parametro 2",
                    Valor = "2"
                });
                context.ParametrosSistema.AddOrUpdate(p => p.IdParametroSistema, new ParametroSistema()
                {
                    IdParametroSistema = 7,
                    DescParametroSistema = "Quantidade dias a retroagir quando interrupcao",
                    Valor = "30"
                });

                context.SaveChanges();

            }
        }

        /// <summary>
        /// Drops the database that is specified in the connection string.
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        private static void ReallyDropDatabase(String connectionString)
        {
            System.Data.Entity.Database.Delete(connectionString);
        }

    }
}
