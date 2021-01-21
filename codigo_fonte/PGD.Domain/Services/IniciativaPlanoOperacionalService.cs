using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Services
{
    public class IniciativaPlanoOperacionalService : IIniciativaPlanoOperacionalService
    {
        private readonly IIniciativaPlanoOperacionalRepository _classRepository;

        public IniciativaPlanoOperacionalService(IIniciativaPlanoOperacionalRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public IniciativaPlanoOperacional Adicionar(IniciativaPlanoOperacional obj)
        {
            return _classRepository.Adicionar(obj);
        }

        public IniciativaPlanoOperacional Atualizar(IniciativaPlanoOperacional obj)
        {
            return _classRepository.Atualizar(obj);
        }

        public IniciativaPlanoOperacional ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<IniciativaPlanoOperacional> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public IniciativaPlanoOperacional Remover(IniciativaPlanoOperacional obj)
        {
            return _classRepository.Remover(obj.IdIniciativaPlanoOperacional);
        }
    }
}
