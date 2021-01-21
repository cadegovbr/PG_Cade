using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Paginacao;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Domain.Services
{
    public class Unidade_TipoPactoService : IUnidade_TipoPactoService
    {
        private readonly IUnidade_TipoPactoRepository _classRepository;

        public Unidade_TipoPactoService(IUnidade_TipoPactoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public Unidade_TipoPacto Adicionar(Unidade_TipoPacto obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.AdicionarSave(obj);
        }

        public Unidade_TipoPacto Atualizar(Unidade_TipoPacto obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public Unidade_TipoPacto BuscarPorIdUnidadeTipoPacto(int idUnidade, int idTipoPacto)
        {
            return _classRepository.Buscar(a => a.IdUnidade == idUnidade && a.IdTipoPacto == idTipoPacto).OrderByDescending(x => x.IdUnidade_TipoPacto).FirstOrDefault();
        }

        public Unidade_TipoPacto ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Unidade_TipoPacto> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public IEnumerable<Unidade_TipoPacto> ObterTodosPorTipoPacto(int idTipoPacto)
        {
            if (idTipoPacto > 0)
            {
                return _classRepository.Buscar(a => a.IdTipoPacto == idTipoPacto);
            } 
            else
            {
                return _classRepository.ObterTodos();
            }
        }

        public Unidade_TipoPacto Remover(Unidade_TipoPacto obj)
        {
            _classRepository.Remover(obj.IdUnidade_TipoPacto);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }

        public Paginacao<Unidade_TipoPacto> Buscar(UnidadeTipoPactoFiltro filtro)
        {
            return _classRepository.Buscar(filtro);
        }
    }
}
