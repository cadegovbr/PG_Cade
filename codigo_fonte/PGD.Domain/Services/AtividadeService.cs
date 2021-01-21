using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.Atividades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Domain.Services
{
    public class AtividadeService : IAtividadeService
    {
        private readonly IAtividadeRepository _classRepository;

        public AtividadeService(IAtividadeRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public Atividade Adicionar(Atividade obj)
        {
            try
            {
                if (!obj.IsValid())
                {
                    return obj;
                }

                obj.ValidationResult = new AtividadeValidation().Validate(obj);
                if (!obj.ValidationResult.IsValid)
                {
                    return obj;
                }

                obj.ValidationResult.Message = Mensagens.MS_003;
                return _classRepository.AdicionarSave(obj);
            }
            catch (Exception ex)
            {
                obj.ValidationResult.Add(new DomainValidation.Validation.ValidationError(ex.Message));
                return obj;
            }
        }

        public Atividade Atualizar(Atividade obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new AtividadeValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public Atividade ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Atividade> ObterTodos()
        {
            return _classRepository.ObterTodos().OrderBy(a=>a.NomAtividade);
        }

        public IEnumerable<Atividade> ObterTodosAtivos()
        {
            return _classRepository.Buscar(a => a.Inativo == false).OrderBy(a => a.NomAtividade);
        }

        public Atividade Remover(Atividade obj)
        {
            var atividade = ObterPorId(obj.IdAtividade);
            atividade.Inativo = true;
            _classRepository.Atualizar(atividade);
            atividade.ValidationResult.Message = Mensagens.MS_005;
            return atividade;
        }
    }
}
