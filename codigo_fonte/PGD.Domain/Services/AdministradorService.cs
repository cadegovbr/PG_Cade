using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.Adminstrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _classRepository;

        public AdministradorService(IAdministradorRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public IEnumerable<Administrador> ObterTodosAdm()
        {
            return _classRepository.ObterTodos();
        }

        public Administrador Adicionar(Administrador obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new AdminstradorValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public Administrador Atualizar(Administrador obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new AdminstradorValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public Administrador ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Administrador> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public Administrador Remover(Administrador obj)
        {
            _classRepository.Remover(obj.IdAdministrador);
            return obj;
        }
    }
}
