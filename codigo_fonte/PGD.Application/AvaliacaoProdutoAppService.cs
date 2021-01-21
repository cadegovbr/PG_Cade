using AutoMapper;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using PGD.Domain.Enums;

namespace PGD.Application
{
    public class AvaliacaoProdutoAppService : ApplicationService, IAvaliacaoProdutoAppService
    {
        private readonly IAvaliacaoProdutoService _avaliacaoProdutoService;
        private readonly INotaAvaliacaoService _notaAvaliacaoService;
        private readonly IOS_ItemAvaliacaoService _itemAvaliacaoService;

        public AvaliacaoProdutoAppService(IUnitOfWork uow, IAvaliacaoProdutoService avaliacaoProdutoService, INotaAvaliacaoService notaAvaliacaoService,
                                          IOS_ItemAvaliacaoService itemAvaliacaoService)
            : base(uow)
        {
            _avaliacaoProdutoService = avaliacaoProdutoService;
            _notaAvaliacaoService = notaAvaliacaoService;
            _itemAvaliacaoService = itemAvaliacaoService;
        }

        public NotaAvaliacaoViewModel CalcularNotaAvaliacaoDetalhada(List<ItemAvaliadoViewModel> lstItensAvaliados)
        {
            decimal nota = 10.0M;
            decimal notaMaximaLimitada = 10.0M;
            NotaAvaliacao notaFinal;            

            foreach (ItemAvaliadoViewModel itemAvaliadoViewModel in lstItensAvaliados)
            {
                OS_ItemAvaliacao itemAvaliacao = _itemAvaliacaoService.ObterPorId(itemAvaliadoViewModel.IdItemAvaliacao);

                nota += itemAvaliacao.ImpactoNota; 
                if (itemAvaliacao.NotaMaxima.LimiteSuperiorFaixa < notaMaximaLimitada)
                {
                    notaMaximaLimitada = itemAvaliacao.NotaMaxima.LimiteSuperiorFaixa;
                }
            }

            if (nota < notaMaximaLimitada)
            {
                notaFinal = _notaAvaliacaoService.ObterTodos().FirstOrDefault(n => n.LimiteSuperiorFaixa >= nota && n.LimiteInferiorFaixa <= nota);                
            }
            else
            {
                notaFinal = _notaAvaliacaoService.ObterTodos().FirstOrDefault(n => n.LimiteSuperiorFaixa >= notaMaximaLimitada && n.LimiteInferiorFaixa <= notaMaximaLimitada);
            }

            
            NotaAvaliacaoViewModel notaAvaliacaoViewModel = Mapper.Map<NotaAvaliacao, NotaAvaliacaoViewModel>(notaFinal);
            notaAvaliacaoViewModel.ValorNotaFinal = nota;

            return notaAvaliacaoViewModel;
        }

        public AvaliacaoProdutoViewModel ObterPorId(int idAvaliacaoProduto)
        {
            return Mapper.Map<AvaliacaoProduto, AvaliacaoProdutoViewModel>(_avaliacaoProdutoService.ObterPorId(idAvaliacaoProduto));
        }

        public int RetornaQualidadeAvaliacaoDetalhada(NotaAvaliacaoViewModel nota)
        {
            var retorno = 1;

            switch (nota.DescNotaAvaliacao.ToLower().Trim())
            {
                case "excelente":
                    retorno = (int) eAvaliacao.Excelente;
                    break;
                case "muito bom":
                    retorno = (int) eAvaliacao.MuitoBom;
                    break;
                case "bom":
                    retorno = (int) eAvaliacao.Bom;
                    break;
                case "regular":
                    retorno = (int) eAvaliacao.Regular;
                    break;
                case "insatisfatório":
                    retorno = (int) eAvaliacao.Insatisfatorio;
                    break;
            }

            return retorno;
        }
    }
}
