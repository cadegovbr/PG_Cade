using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Paginacao;
using PGD.Domain.Validations.UsuarioPerfilUnidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGD.Domain.Services
{
    public class UsuarioPerfilUnidadeService : IUsuarioPerfilUnidadeService
    {
        private readonly IUsuarioPerfilUnidadeRepository _usuarioPerfilUnidadeRepository;

        public UsuarioPerfilUnidadeService(IUsuarioPerfilUnidadeRepository usuarioPerfilUnidadeRepository)
        {
            _usuarioPerfilUnidadeRepository = usuarioPerfilUnidadeRepository;
        }

        public Paginacao<UsuarioPerfilUnidade> Buscar(UsuarioPerfilUnidadeFiltro filtro)
        {
            return _usuarioPerfilUnidadeRepository.Buscar(filtro);
        }

        public UsuarioPerfilUnidade Adicionar(UsuarioPerfilUnidade usuarioPerfilUnidade)
        {
            try
            {
                usuarioPerfilUnidade.ValidationResult = new UsuarioPerfilUnidadeValidation().Validate(usuarioPerfilUnidade);

                if (!usuarioPerfilUnidade.ValidationResult.IsValid)
                    return usuarioPerfilUnidade;

                usuarioPerfilUnidade.ValidationResult.Message = Mensagens.MS_011;
                return _usuarioPerfilUnidadeRepository.AdicionarSave(usuarioPerfilUnidade);
            }
            catch (Exception ex)
            {
                usuarioPerfilUnidade.ValidationResult.Add(new DomainValidation.Validation.ValidationError(ex.Message));
                return usuarioPerfilUnidade;
            }
        }

        private UsuarioPerfilUnidade ObterPorId(long id)
        {
            var retorno = _usuarioPerfilUnidadeRepository.Buscar(new UsuarioPerfilUnidadeFiltro { IdUsuarioPerfilUnidade = id });
            return retorno.Lista.FirstOrDefault();
        }

        public UsuarioPerfilUnidade Remover(long id)
        {
            try
            {
                var usuarioPerfilUnidade = ObterPorId(id);
                usuarioPerfilUnidade.Excluido = true;
                _usuarioPerfilUnidadeRepository.Atualizar(usuarioPerfilUnidade);
                usuarioPerfilUnidade.ValidationResult.Message = Mensagens.MS_011;
                return usuarioPerfilUnidade;
            }
            catch (Exception ex)
            {
                var retornoException = new UsuarioPerfilUnidade();
                retornoException.ValidationResult.Add(new DomainValidation.Validation.ValidationError(ex.Message));
                return retornoException;
            }
        }
    }
}
