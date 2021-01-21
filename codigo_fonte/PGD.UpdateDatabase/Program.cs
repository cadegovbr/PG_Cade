using Cgu.Util.EntityFramework;
using CGU.Util;
using System;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace PGD.DatabaseUpdate
{
    public static class Program
    {
        private static readonly string NOME_ARQUIVO_LAST_MIGRATION_VALIDA = "UltimaMigrationValida.txt";

        static void Main(string[] args)
        {
            var configuration = new PGD.DatabaseUpdate.Migrations.Configuration();

            configuration.TargetDatabase =
                new DbConnectionInfo(ConfigurationManager.ConnectionStrings["PGDConnectionString"].ConnectionString, "System.Data.SqlClient");

            var migrator = new DbMigrator(configuration);

            MigratorLoggingDecorator logger = new MigratorLoggingDecorator(migrator, new ConsoleLoggerDecoratorCGU());


            if (args.Any() && args[0] == "rollback")
            {
                RollBack(logger);
            }
            else
            {
                Update(logger);
            }
        }

        private static void Update(MigratorLoggingDecorator logger)
        {
            var last = logger.GetDatabaseMigrations().OrderByDescending(m => m).FirstOrDefault();

            try
            {
                if (File.Exists(NOME_ARQUIVO_LAST_MIGRATION_VALIDA))
                    File.Delete(NOME_ARQUIVO_LAST_MIGRATION_VALIDA);

                logger.Update();

                File.WriteAllText(NOME_ARQUIVO_LAST_MIGRATION_VALIDA, last);
            }
            catch (SqlException ex)
            {
                // LogManagerComum.LogarErro(ex, mensagem: "DEPLOY PGD -> UPDATE DATABASE");
                Console.WriteLine("Erro ao executar Update Database: " + ex.Message );

                try
                {
                    logger.Configuration.AutomaticMigrationDataLossAllowed = true;
                    logger.Update(last);
                    logger.Configuration.AutomaticMigrationDataLossAllowed = false;
                }
                catch (SqlException ex2)
                {
                    // LogManagerComum.LogarErro(ex2, mensagem: "DEPLOY PGD -> UPDATE DATABASE -> *** ATENCAO *** PROBLEMAS NO ROLLBACK!!!!!");
                }

                Environment.ExitCode = -1;
            }
        }

        private static void RollBack(MigratorLoggingDecorator logger)
        {
            try
            {
                if (File.Exists(NOME_ARQUIVO_LAST_MIGRATION_VALIDA))
                {
                    var last = File.ReadAllText(NOME_ARQUIVO_LAST_MIGRATION_VALIDA);

                    // LogManagerComum.LogarInfo(mensagem: $"DEPLOY PGD -> UPDATE DATABASE - EXECUTANDO ROLLBACK PARA -> {last}");

                    logger.Configuration.AutomaticMigrationDataLossAllowed = true;
                    logger.Update(last);
                    logger.Configuration.AutomaticMigrationDataLossAllowed = false;

                    File.Delete(NOME_ARQUIVO_LAST_MIGRATION_VALIDA);
                    // LogManagerComum.LogarInfo(mensagem: "DEPLOY PGD -> UPDATE DATABASE - ROLLBACK EXECUTADO COM SUCESSO");
                }
            }
            catch (SqlException ex)
            {
                // LogManagerComum.LogarErro(ex, mensagem: "DEPLOY PGD -> UPDATE DATABASE -> *** ATENCAO *** PROBLEMAS NO ROLLBACK!!!!!");
                Environment.ExitCode = -1;
            }
        }
    }
}