using PGD.Domain.Constantes;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Validations.Adminstrador;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Services
{
    public class ArquivoDadoBrutoService : IArquivoDadoBrutoService
    {
        private readonly IArquivoDadoBrutoRepository _classRepository;

        public ArquivoDadoBrutoService(IArquivoDadoBrutoRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public IEnumerable<ArquivoDadoBruto> ObterTodos()
        {
            return ObterPorAno(0);
        }

        public IEnumerable<ArquivoDadoBruto> ObterPorAno(int ano)
        {
            var lstArqDadosBrutos = new List<ArquivoDadoBruto>();

            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["DiretorioArquivos"].ToString()))
            {
                return null;
            }
            else
            {
                String diretorio = ConfigurationManager.AppSettings["DiretorioArquivos"].ToString();

                // diretorio raiz dos arquivos
                DirectoryInfo dir = new DirectoryInfo(diretorio);

                if (!dir.Exists)
                {
                    return null;
                }

                List<String> lstAnos = dir.GetFiles().Select(c => c.Name.Substring(4, 4)).Where(c => c.Equals(ano.ToString()) || ano == 0).Distinct().ToList();

                foreach (String strAno in lstAnos)
                {
                    var arqDadosBrutos = new ArquivoDadoBruto();
                    arqDadosBrutos.Ano = strAno;
                    
                    // Para cada ano, pega o arquivo mais recente
                    FileInfo arqMaisRecente = dir.GetFiles().Where(p => p.Name.Contains("_" + strAno)).OrderByDescending(p => p.Name.Substring(4, 8)).FirstOrDefault();

                    if (arqMaisRecente != null)
                    {
                        arqDadosBrutos.NomeArquivo = arqMaisRecente.Name;
                        arqDadosBrutos.CaminhoCompletoArquivo = arqMaisRecente.FullName;
                        arqDadosBrutos.DataArquivo = arqMaisRecente.CreationTime.ToString("dd/MM/yyyy HH:mm:ss");

                        lstArqDadosBrutos.Add(arqDadosBrutos);
                    }
                }
            }

            return lstArqDadosBrutos;
        }
    }
}

