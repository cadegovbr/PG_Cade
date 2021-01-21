using CGU.Util;
using System;

namespace PGD.Infra.CrossCutting.Util
{
    public class EmailCGU
    {
        public void EnviarEmail(String mensagemHTML, String assunto, String destinatario)
        {
            MailManagerComum mailManager = new MailManagerComum();

            bool gravarEmailEmArquivo = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["GravarEmailEmArquivo"].ToString());

            mailManager.EnviarEmail(assunto, mensagemHTML, true, destinatario, gravarEmArquivo: gravarEmailEmArquivo);            
        }


    }
}
