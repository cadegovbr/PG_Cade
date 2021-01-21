using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;

namespace PGD.Domain.Services
{
    public class HistoricoService : IHistoricoService
    {
        private readonly IHistoricoRepository _classRepository;

        public HistoricoService(IHistoricoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public Historico Adicionar(Historico obj)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();
            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
 
        }

        public Historico Atualizar(Historico obj)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();
            obj.ValidationResult.Message = Mensagens.MS_004;
            return _classRepository.Atualizar(obj);

        }
        public Historico AtualizarIdPacto(Historico obj, int idPacto)
        {
            obj.ValidationResult = new DomainValidation.Validation.ValidationResult();
            obj.ValidationResult.Message = Mensagens.MS_004;
            return _classRepository.Atualizar(obj, idPacto);
       
        }
        public Historico BuscarPorId(int idpacto, int idhistorico)
        {
            return _classRepository.BuscarPorId(idpacto, idhistorico);
        }

        public Historico ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Historico> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Historico> ObterTodos(int idpacto)
        {
            return this._classRepository.Buscar(x => x.IdPacto == idpacto).ToList();
        }

        public IEnumerable<Historico> ObterTodos(Historico objFiltro)
        {
            throw new NotImplementedException();
        }

        public Historico Remover(Historico obj)
        {
            throw new NotImplementedException();
        }
    }
}
