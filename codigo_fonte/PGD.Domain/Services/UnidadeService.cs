using PGD.Domain.Entities.RH;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Paginacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PGD.Domain.Services
{
    public class UnidadeService : IUnidadeService
    {
        private readonly IUnidade_TipoPactoService _unidade_TipoPactoService;
        private readonly IUnidadeRepository _unidadeRepository;

        public UnidadeService(IUnidade_TipoPactoService unidade_TipoPactoService, IUnidadeRepository unidadeRepository)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _unidade_TipoPactoService = unidade_TipoPactoService;
            _unidadeRepository = unidadeRepository;
        }

        public IEnumerable<Unidade> ObterUnidades(int idTipoPacto = 0)
        {
            var listaTodasUnidades = _unidadeRepository.ObterTodos();

            if (idTipoPacto > 0)
            {
                var listaUnidadeHabilitadasParaTipoPacto = _unidade_TipoPactoService.ObterTodosPorTipoPacto(idTipoPacto).ToList();

                return listaTodasUnidades.Where(l => listaUnidadeHabilitadasParaTipoPacto.Select(h => h.IdUnidade).Contains(l.IdUnidade)).ToList();
            }
            else
            {
                return listaTodasUnidades;
            }
        }

        public IEnumerable<Unidade> ObterUnidadesSubordinadas(int idUnidadePai)
        {
            var lista = new List<Unidade>();

            return lista;
        }

        public Unidade ObterUnidade(int idUnidade)
        {
            var listaTodasUnidades = ObterUnidades();
            return listaTodasUnidades.SingleOrDefault(i => i.IdUnidade == idUnidade);
        }

        public Paginacao<Unidade> Buscar(UnidadeFiltro filtro)
        {
            return _unidadeRepository.Buscar(filtro);
        }

        public Unidade Adicionar(Unidade obj)
        {
            throw new NotImplementedException();
        }

        public Unidade ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Unidade> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Unidade Atualizar(Unidade obj)
        {
            throw new NotImplementedException();
        }

        public Unidade Remover(Unidade obj)
        {
            throw new NotImplementedException();
        }
    }
}
