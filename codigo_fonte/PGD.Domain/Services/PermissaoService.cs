using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Services
{
    public class PermissaoService : IPermissaoService
    {
        private readonly IPermissaoRepository _permissaoRepository;

        public PermissaoService(IPermissaoRepository permissaoRepository)
        {
            _permissaoRepository = permissaoRepository;
        }


        public Paginacao<Permissao> Buscar(PermissaoFiltro filtro)
        {
            return _permissaoRepository.Buscar(filtro);
        }
    }
}
