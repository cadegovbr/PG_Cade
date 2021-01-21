using CGU.Util;
using Ninject;
using PGD.Application;
using PGD.Application.AutoMapper;
using PGD.Infra.CrossCutting.IoC;

namespace PGD.Services.Console
{
    class Program
    {
        public static StandardKernel kernel = new StandardKernel();

        static void Main(string[] args)
        {
            BootStrapper.RegisterServicesSingleton(kernel);

            AutoMapperConfig.RegisterMappings();

            var pactoAppService = kernel.Get<PactoAppService>();

            LogManagerComum.LogarInfo(null, "Iniciando o gerente de tarefas do PGD");
            pactoAppService.IniciarAutomaticamente();
            pactoAppService.FinalizarAutomaticamente();
            pactoAppService.RetornarSuspensaoAutomaticamente();
            LogManagerComum.LogarInfo(null, "Finalizando o gerente de tarefas do PGD");
        }
    }
}

