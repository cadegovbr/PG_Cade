using System;
using NUnit.Framework;
using System.Collections.Generic;
using PGD.Domain.Entities;
using PGD.UI.Mvc.App_Start;
using Ninject;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using PGD.Domain.Entities.Usuario;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class OrdemServicoTests
    {
        private IOrdemServicoService _ordemServicoservice;
        private IGrupoAtividadeService _grupoAtividadeService;
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

            _ordemServicoservice = kernel.Get<IOrdemServicoService>();
            _grupoAtividadeService = kernel.Get<IGrupoAtividadeService>();

        }

        //CRUD
        int itemId = 0;
        [Order(0), TestCase("Ordem Servico 4", TestName = "OrdemServico Insert Ordem 4")]
        public void TestInsert(string NomeOrdem)
        {
            var ordemServico = new OrdemServico
            {
                DescOrdemServico = NomeOrdem,
                DatInicioNormativo = DateTime.Now.Date,
                DatInicioSistema = DateTime.Now.Date,
                Grupos = new List<OS_GrupoAtividade>()
            };
            var grupo = _grupoAtividadeService.ObterPorId(1);

            var osgrupo = new OS_GrupoAtividade
            {
                IdGrupoAtividade = grupo.IdGrupoAtividade,
                Inativo = grupo.Inativo,
                NomGrupoAtividade = grupo.NomGrupoAtividade,
                Atividades = new List<OS_Atividade>()
            };
            foreach (var atividade in grupo.Atividades)
            {
                var osatividade = new OS_Atividade
                {
                    IdAtividade = atividade.IdAtividade,
                    Inativo = atividade.Inativo,
                    NomAtividade = atividade.NomAtividade,
                    PctMinimoReducao = atividade.PctMinimoReducao,
                    Tipos = new List<OS_TipoAtividade>()
                };

                foreach (var tipo in atividade.Tipos)
                    osatividade.Tipos.Add(new OS_TipoAtividade { DuracaoFaixa = tipo.DuracaoFaixa, DuracaoFaixaPresencial = tipo.DuracaoFaixaPresencial, Faixa = tipo.Faixa });

                osgrupo.Atividades.Add(osatividade);
            }

            ordemServico.Grupos.Add(osgrupo);
            _uow.BeginTransaction();
            var resultado = _ordemServicoservice.Adicionar(ordemServico);
            _uow.Commit();
            itemId = ordemServico.IdOrdemServico;
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(1), TestCase(TestName = "OrdemServico Retrieve Ordem 4 By ID")]
        public void TestRetrieveId()
        {
            var grupo = _ordemServicoservice.ObterPorId(itemId);
            Assert.IsNotNull(grupo);
            Assert.AreEqual(grupo.IdOrdemServico, itemId);
        }

        [Order(2), TestCase("NomeNovo", TestName = "OrdemServico Update Ordem 4")]
        public void TestUpdate(string nome)
        {
            var ordem = _ordemServicoservice.ObterPorId(itemId);
            ordem.DescOrdemServico = nome;
            var resultado = _ordemServicoservice.Atualizar(ordem);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

        [Order(3), TestCase(TestName = "OrdemServico Delete Ordem 4")]
        public void TestDelete()
        {
            var ordem = _ordemServicoservice.ObterPorId(itemId);
            var resultado = _ordemServicoservice.Remover(ordem);
            Assert.IsTrue(resultado.ValidationResult.Message.Contains("sucesso"));
        }

    }
}
