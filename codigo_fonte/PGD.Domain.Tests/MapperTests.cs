using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGD.Application.ViewModels;
using PGD.Domain;
using PGD.Domain.Entities;
using AutoMapper;
using PGD.Application.AutoMapper;
using System.Linq;
using System.Collections.Generic;

namespace PGD.Domain.Tests
{
    [TestClass]
    public class MapperTests
    {

        [TestInitialize]
        public void Initialize()
        {
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod]
        public void Mapper_Pacto_Produto_Test()
        {

            var iniciativaPlanoVM1 = new IniciativaPlanoOperacionalViewModel()
            {
                IdIniciativaPlanoOperacional = "1",
                DescIniciativaPlanoOperacional = "nadar"
            };

            var pactoViewModel = new PactoViewModel()
            {
                CpfUsuario = "113912758761",
                Nome="Persio Otta",
                PossuiCargaHoraria=false,
                Produtos = {
                    new ProdutoViewModel() {
                        IdProduto = 101,
                        IdOrdemServico = 5050,
                        IdGrupoAtividade = 1,
                        IdAtividade = 2,
                        IdTipoAtividade = 3,
                        QuantidadeProduto = 30,
                        ObservacoesAdicionais = "Alterado por mim",
                        Avaliacao = 3,
                        IniciativasPlanoOperacionalProduto = new List<IniciativaPlanoOperacionalProdutoViewModel>()
                        {
                            new IniciativaPlanoOperacionalProdutoViewModel()
                            {
                                IdIniciativaPlanoOperacional ="1",
                                IdProduto = 101
                            }
                        }
                    },
                    new ProdutoViewModel() {
                        IdProduto = 102,
                        IdOrdemServico = 5050,
                        IdGrupoAtividade = 1,
                        IdAtividade = 2,
                        IdTipoAtividade = 4,
                        QuantidadeProduto = 3,
                        ObservacoesAdicionais = "Alterado por mim o segundo produto",
                        Avaliacao = 4,
                        IniciativasPlanoOperacionalProduto = new List<IniciativaPlanoOperacionalProdutoViewModel>()
                        {
                            new IniciativaPlanoOperacionalProdutoViewModel()
                            {
                                IdIniciativaPlanoOperacional ="1",
                                IdProduto = 102
                            }
                        }
                    }
                }
            };
            var iniciativaPlano1 = new IniciativaPlanoOperacional()
            {
                IdIniciativaPlanoOperacional = "1",
                DescIniciativaPlanoOperacional = "nadar"
            };

            var pacto = new Pacto()
            {
                CpfUsuario = "113912758761",
                Nome = "Persio Otta",
                PossuiCargaHoraria = false,
                Produtos = new List<Produto>()
                {
                    new Produto() {
                        IdProduto = 101,
                        IdGrupoAtividade = 1,
                        IdAtividade = 2,
                        IdTipoAtividade = 3,
                        QuantidadeProduto = 30,
                        ObservacoesAdicionais = "Original",
                        Avaliacao = 0,
                        IniciativasPlanoOperacionalProduto = new List<IniciativaPlanoOperacionalProduto>()
                        {
                            new IniciativaPlanoOperacionalProduto()
                            {
                                IdIniciativaPlanoOperacional ="1",
                                IdProduto = 101
                            }
                        }
                    },
                    new Produto () {
                        IdProduto = 102,
                        IdGrupoAtividade = 1,
                        IdAtividade = 2,
                        IdTipoAtividade = 4,
                        QuantidadeProduto = 3,
                        ObservacoesAdicionais = "original2",
                        Avaliacao = 0,
                        IniciativasPlanoOperacionalProduto = new List<IniciativaPlanoOperacionalProduto>()
                        {
                            new IniciativaPlanoOperacionalProduto()
                            {
                                IdIniciativaPlanoOperacional ="1",
                                IdProduto = 102
                            }
                        }
                    }
                }
            };

            pacto = Mapper.Map<PactoViewModel, Pacto>(pactoViewModel);


            Assert.AreEqual(3, pacto.Produtos.Where(p => p.IdProduto == 101).FirstOrDefault().Avaliacao);
            Assert.AreEqual("Alterado por mim o segundo produto", pacto.Produtos.Where(p => p.IdProduto == 102).FirstOrDefault().ObservacoesAdicionais);
        }
    }
}
