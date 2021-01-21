using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Constantes;

namespace PGD.Domain.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _classRepository;

        public LogService(ILogRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public Log Adicionar(Log obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_003;
            return _classRepository.Adicionar(obj);
        }

        public Log Atualizar(Log obj)
        {
            if (!obj.IsValid())
            {
                return obj;
            }

            obj.ValidationResult.Message = Mensagens.MS_004;

            return _classRepository.Atualizar(obj);
        }

        public Log ObterPorId(int id)
        {
            return _classRepository.ObterPorId(id);
        }

        public IEnumerable<Log> ObterTodos()
        {
            return _classRepository.ObterTodos();
        }

        public Log Remover(Log obj)
        {
            _classRepository.Remover(obj.IdLog);
            return obj;
        }

        public Log Logar(object obj, string CpfUsuario, string oper)
        {
            var log = new Log();

            try
            {
                Type userType;
                if (obj.GetType().FullName.Contains("DynamicProxies"))
                    userType = obj.GetType().BaseType;
                else
                    userType = obj.GetType();

                var propid = obj.GetType().GetProperties()
                    .Where(p => p.Name == "Id" + userType.Name)
                    .FirstOrDefault();
                if (propid == null)
                {
                    propid = obj.GetType().GetProperties()
                    .Where(p => p.Name == "Id")
                    .FirstOrDefault();
                }

                var id = propid.GetValue(obj, null).ToString();
                string valores = "|";

                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (!prop.PropertyType.Name.StartsWith("ICollection") && !prop.Name.Contains("Validation") && !prop.GetGetMethod().IsVirtual)
                        valores += string.Format("{0}={1}|", prop.Name, prop.GetValue(obj, null));
                }

                log.Data = DateTime.Now;
                log.IdTabela = int.Parse(id);
                log.CpfUsuario = CpfUsuario;
                log.Operacao = oper.ToString();
                log.Tabela = userType.Name;
                log.Valores = valores;

            }
            catch (Exception ex)
            {
                log.Data = DateTime.Now;
                log.IdTabela = 0;
                log.CpfUsuario = CpfUsuario;
                log.Operacao = oper.ToString();
                log.Tabela ="Erro de "+ oper.ToString();
                log.Valores = ex.Message;
            }

            return Adicionar(log);
        }

    }
}
