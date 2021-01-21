using Ninject;
using NUnit.Framework;
using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class OS_GrupoAtividadeTests
    {
        private IOS_GrupoAtividadeService _iOS_GrupoAtividadeService;
        private IUnitOfWork _uow;
        public static StandardKernel kernel = new StandardKernel();

        [OneTimeSetUp]
        public void Initialize()
        {
            var dbInicio = new DbTests();
            try
            {
                dbInicio.Limpa_E_Cria_Database();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Banco de dados Limpo e atualizado");

            BootStrapper.RegisterServicesSingleton(kernel);
            _uow = kernel.Get<IUnitOfWork>();

            _iOS_GrupoAtividadeService = kernel.Get<IOS_GrupoAtividadeService>();

        }

        int itemId = 0;

        [Order(0), TestCase(TestName = "OS Grupo Atividade Insert")]
        public void TestInsert()
        {
            var oS_GrupoAtividade = new OS_GrupoAtividade
            {
                IdGrupoAtividadeOriginal = 0,
                IdOrdemServico = 1,
                NomGrupoAtividade = "Grupo Atividade Insert OS"
            };
            _uow.BeginTransaction();
            var resultado = _iOS_GrupoAtividadeService.Adicionar(oS_GrupoAtividade);
            _uow.Commit();
            itemId = oS_GrupoAtividade.IdGrupoAtividade;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "OS Grupo Atividade Retrieve")]
        public void TestRetrieveId()
        {
            var oS_GrupoAtividade = _iOS_GrupoAtividadeService.ObterPorId(itemId);
            Assert.IsNotNull(oS_GrupoAtividade);
            Assert.AreEqual(oS_GrupoAtividade.IdGrupoAtividade, itemId);
        }

        [Order(2), TestCase(TestName = "OS Grupo Atividade Update")]
        public void TestUpdate()
        {
            var oS_GrupoAtividade = _iOS_GrupoAtividadeService.ObterPorId(itemId);
            oS_GrupoAtividade.NomGrupoAtividade = "Grupo Atividade Update OS";
            var resultado = _iOS_GrupoAtividadeService.Atualizar(oS_GrupoAtividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "OS Grupo Atividade Delete")]
        public void TestDelete()
        {
            var oS_GrupoAtividade = _iOS_GrupoAtividadeService.ObterPorId(itemId);
            var resultado = _iOS_GrupoAtividadeService.Remover(oS_GrupoAtividade);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }
    }
}
