using System;
using NUnit.Framework;
using PGD.Services.REST.SIGRHAPI.Service;

namespace PGD.Services.REST.SIGRHAPI.Tests
{
    [TestFixture]
    public class PostgresTests
    {
 
        [OneTimeSetUp]
        public void Initialize()
        {

        }
        [Category("Integration")]
        [TestCase("01368739105", TestName = "É dirigente - Zeros a esquerda")]
        [TestCase("1368739105", TestName = "É dirigente - Sem zeros")]
        public void VerificaEhDirigente(string CPF)
        {
            var sigrh = new SigRHService();
            //sigrh.connString = connStringPostGres;
            Assert.IsTrue(sigrh.EhDirigente(CPF));
        }
        [Category("Integration")]
        [TestCase("02941397450", TestName = "É solicitante - Zeros a esquerda")]
        [TestCase("2941397450", TestName = "É solicitante - Sem zeros")]
        public void VerificaEhSolicitante(string CPF)
        {
            var sigrh = new SigRHService();
            //sigrh.connString = connStringPostGres;
            Assert.IsTrue(sigrh.EhSolicitante(CPF));
        }
    }
}
