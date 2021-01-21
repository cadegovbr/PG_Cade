using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;
using PGD.Domain.Validations.OrdensServico;

namespace PGD.Domain.Services
{
    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _classRepository;
        private readonly IOS_GrupoAtividadeRepository _osgrupoRepository;

        public OrdemServicoService(IOrdemServicoRepository classRepository, IOS_GrupoAtividadeRepository osgrupoRepository)
        {
            _classRepository = classRepository;
            _osgrupoRepository = osgrupoRepository;
        }

        public OrdemServico Adicionar(OrdemServico obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OrdemServicoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.AdicionarSave(obj);
        }

        public OrdemServico Atualizar(OrdemServico obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult = new OrdemServicoValidation().Validate(obj);
            if (!obj.ValidationResult.IsValid)
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public void DeletarGrupos(OrdemServico ordemservico)
        {
            var list = ordemservico.Grupos.Select(a => a.IdGrupoAtividade).ToList();
            foreach(int i in list)
                _osgrupoRepository.Remover(i);
        }

        public OrdemServico ObterOrdemVigente()
        {

            //return _classRepository.ObterTodos().Where(x => x.DatInicioSistema <= DateTime.Now).OrderByDescending(a => a.DatInicioSistema).FirstOrDefault();
            return _classRepository.ObterTodosInclude();
        }

        public OrdemServico ObterPorId(int id)
        {

            return _classRepository.ObterPorId(id);
          
        }

        public OrdemServico ObterPorIdInclude(int id)
        {

            return _classRepository.ObterPorIdInclude(id);

        }

        public IEnumerable<OrdemServico> ObterTodos()
        {
            var lista =  _classRepository.ObterTodos();
            return lista;
        }

        public IEnumerable<OrdemServico> ObterTodosAtivos()
        {
            var lista = _classRepository.Buscar(a => a.Inativo == false);
            return lista;
        }

        public OrdemServico Remover(OrdemServico obj)
        {
            var os = ObterPorId(obj.IdOrdemServico);
            os.Inativo = true;
            _classRepository.Atualizar(os);
            os.ValidationResult.Message = Mensagens.MS_005;
            return os;
        }
    }
}
