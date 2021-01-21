using PGD.Domain.Entities.RH;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;

namespace PGD.Domain.Services
{
    public class FeriadoService : IFeriadoService
    {
        private readonly IFeriadoRepository _feriadoRepository;

        public FeriadoService(IFeriadoRepository feriadoRepository)
        {
            _feriadoRepository = feriadoRepository;
        }

        public IEnumerable<Feriado> ObterFeriados(DateTime dtAPartirDe)
        {
            List<Feriado> retorno = new List<Feriado>();

            return retorno;
        }

        public bool VerificaFeriado(DateTime dataAVerificar)
        {
            return false;
        }

        public Feriado ObterFeriado(DateTime data)
        {
            return _feriadoRepository
                .Buscar(x => DbFunctions.TruncateTime(x.data_feriado) == DbFunctions.TruncateTime(data)).FirstOrDefault();
        }

        public Feriado Adicionar(Feriado obj)
        {
            throw new NotImplementedException();
        }

        public Feriado ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Feriado> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Feriado Atualizar(Feriado obj)
        {
            throw new NotImplementedException();
        }

        public Feriado Remover(Feriado obj)
        {
            throw new NotImplementedException();
        }
    }
}
