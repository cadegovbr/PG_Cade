using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class Mensagem
    {
 
        /// <summary>
        /// Nome do campo a ser validado
        /// </summary>
        public string Campo { get; set; }
        /// <summary>
        /// Menssagem de validação
        /// </summary>
        public string Descricao { get; set; }

        public string Metodo { get; set; }

        public string Pagina { get; set; }

 
        /// <summary>
        /// Construtor da classe.
        /// //
        /// </summary>
        public Mensagem()
        {
            Descricao = string.Empty;
            Campo = string.Empty;
        }

        public Mensagem(string campo, string mensagem)
            : this()
        {
            Descricao = mensagem;
            Campo = campo;
        }

        public Mensagem(string pDescricao, string pMetodo, string pPagina)
            : this()
        {
            Descricao = pDescricao;
            Metodo = pMetodo;
            Pagina = pPagina;
        }

     
        public static Mensagem Cria(string campo, string descricao)
        {
            var m = new Mensagem
            {
                Campo = campo,
                Descricao = descricao
            };

            return m;
        }
 
    }
}
