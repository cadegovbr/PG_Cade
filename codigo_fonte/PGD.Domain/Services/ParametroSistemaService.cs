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
    public class ParametroSistemaService : IParametroSistemaService
    {
        private readonly IParametroSistemaRepository  _classRepository;

        public ParametroSistemaService(IParametroSistemaRepository parametroSistemaRepository)
        {
            _classRepository = parametroSistemaRepository;
        }

        public ParametroSistema Adicionar(ParametroSistema obj)
        {
            throw new NotImplementedException();
        }

        public ParametroSistema Atualizar(ParametroSistema obj)
        {
            throw new NotImplementedException();
        }

        public ParametroSistema ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<ParametroSistema> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public ParametroSistema Remover(ParametroSistema obj)
        {
            throw new NotImplementedException();
        }
    }
}
