using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;
using PGD.Domain.Validations.TiposAtividade;
using DomainValidation.Validation;
using PGD.Domain.Validations.Cronogramas;

namespace PGD.Domain.Services
{
    public class CronogramaService : ICronogramaService
    {
        private readonly ICronogramaRepository _classRepository;
        private readonly IPactoService _pactoService;

        public CronogramaService(ICronogramaRepository classRepository, IPactoService pactoService)
        {
            _pactoService = pactoService;
            _classRepository = classRepository;
        }

        public Cronograma Adicionar(Cronograma obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public Cronograma Atualizar(Cronograma obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }
            
            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public Cronograma ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Cronograma> ObterTodos()
        {
           return _classRepository.ObterTodos();
        }
        public IEnumerable<Cronograma> ObterTodos(Cronograma obj)
        {
            return _classRepository.ObterTodos();
        }

        public IEnumerable<Cronograma> ObterTodos(int idPacto)
        {
            return _classRepository.Buscar(x => x.IdPacto == idPacto);
        }

        public Cronograma Remover(Cronograma obj)
        {
            _classRepository.Remover(obj.IdCronograma);
            obj.ValidationResult.Message = Mensagens.MS_005;
            return obj;
        }

        public void ValidarCronograma(CronogramaPacto cronogramaPacto, List<Pacto> pactosConcorrentes, bool validarHorasADistribuir = true)
        {
            cronogramaPacto.ValidationResult = new CronogramaPactoValidation(validarHorasADistribuir).Validate(cronogramaPacto);

            cronogramaPacto.Cronogramas.ForEach(c => 
            {
                var qtdHorasOcupadas = pactosConcorrentes.Select(p => p.Cronogramas.FirstOrDefault(cop => c.DataCronograma.Date == cop.DataCronograma.Date)).Sum(cop => cop == null ? 0 : cop.HorasCronograma);

                var cronogramaValidation = new CronogramaValidation(cronogramaPacto.HorasDiarias, 
                    c.DataCronograma, qtdHorasOcupadas);

                c.ValidationResult = cronogramaValidation.Validate(c);
                c.ValidationResult.Erros.ToList().ForEach(e => cronogramaPacto.ValidationResult.Add(e));

            });
            
        }

    }
}
