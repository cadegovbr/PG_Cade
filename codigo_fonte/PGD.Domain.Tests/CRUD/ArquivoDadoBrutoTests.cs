using Ninject;
using NUnit.Framework;
using PGD.Application.Interfaces;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.CrossCutting.IoC;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Tests.CRUD
{
    [TestFixture]
    public class ArquivoDadoBrutoTests
    {
        private IArquivoDadoBrutoService _arquivoDadoBrutoService;

        public static StandardKernel kernel = new StandardKernel();

        [OneTimeSetUp]
        public void Initialize()
        {
            String diretorio = ConfigurationManager.AppSettings["DiretorioArquivos"].ToString();

            DirectoryInfo dir = new DirectoryInfo(diretorio);

            if (dir.Exists)
            {
                dir.Delete(true);
            }

            dir.Create();

            // 1990            
            string fileName = "PGD_19900101.csv";
            string pathString = System.IO.Path.Combine(dir.FullName, fileName);
            PreencherArquivo(pathString);

            fileName = "PGD_19901002.csv";
            pathString = System.IO.Path.Combine(dir.FullName, fileName);
            PreencherArquivo(pathString);

            fileName = "PGD_19900520.csv";
            pathString = System.IO.Path.Combine(dir.FullName, fileName);
            PreencherArquivo(pathString);

            //1998            
            fileName = "PGD_19980305.csv";
            pathString = System.IO.Path.Combine(dir.FullName, fileName);
            PreencherArquivo(pathString);

            fileName = "PGD_19980507.csv";
            pathString = System.IO.Path.Combine(dir.FullName, fileName);
            PreencherArquivo(pathString);

            fileName = "PGD_19980103.csv";
            pathString = System.IO.Path.Combine(dir.FullName, fileName);
            PreencherArquivo(pathString);

            BootStrapper.RegisterServicesSingleton(kernel);
            _arquivoDadoBrutoService = kernel.Get<IArquivoDadoBrutoService>();
        }

        private static void PreencherArquivo(string pathString)
        {
            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
        }

        [Order(1), TestCase(TestName = "Arquivo Dado Bruto Retrieve - Todos")]
        public void TestRetrieveTodos()
        {
            var lstArquivos = _arquivoDadoBrutoService.ObterTodos();
            Assert.IsNotNull(lstArquivos);
            Assert.IsNotNull(lstArquivos.Count() == 2);
        }

        [Order(2), TestCase(TestName = "Arquivo Dado Bruto Retrieve - Ano Com Arquivo")]
        public void TestRetrievePorAno_OK()
        {
            var lstArquivos = _arquivoDadoBrutoService.ObterPorAno(1998);
            Assert.IsNotNull(lstArquivos);
            Assert.IsNotNull(lstArquivos.FirstOrDefault().NomeArquivo.Equals("PGD_19980507.csv"));
        }

        [Order(3), TestCase(TestName = "Arquivo Dado Bruto Retrieve - Ano Sem Arquivo")]
        public void TestRetrievePorAno_OK_Vazio()
        {
            var lstArquivos = _arquivoDadoBrutoService.ObterPorAno(1996);
            Assert.IsNotNull(lstArquivos);
            Assert.IsTrue(lstArquivos.Count() == 0);
        }

    }
}
