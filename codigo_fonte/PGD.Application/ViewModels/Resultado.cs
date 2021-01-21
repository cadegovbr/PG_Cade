using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class Resultado
    {

        #region Propriedades

        /// <summary>
        /// Status que informa se a operação ocorreu com sucesso
        /// </summary>
        public bool Sucesso { get; set; }
        /// <summary>
        /// Lista dos campos e mensagens de validação
        /// </summary>
        public List<Mensagem> Mensagens { get; set; }
        /// <summary>
        /// Identificador único dos resultado
        /// </summary>
        public int NumError { get; set; }
        public int Id { get; set; }
        public string Metodo { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="sucesso">Sucesso.</param>
        /// <param name="mensagens">Mensagens.</param>
        /// <param name="id">Id.</param>
        public Resultado(bool sucesso, List<Mensagem> mensagens, int id)
        {
            Sucesso = sucesso;
            Mensagens = mensagens;
            Id = id;
        }

        /// <summary>
        /// Construtor vazio da classe.
        /// </summary>
        public Resultado()
            : this(true, new List<Mensagem>(), int.MinValue)
        {
        }

        public Resultado(bool sucesso, params Mensagem[] mensagens)
            : this(sucesso, mensagens.ToList(), int.MinValue)
        { }

        public Resultado(bool sucesso)
            : this(sucesso, new List<Mensagem>(), int.MinValue)
        { }

        #endregion

        #region Métodos Estáticos
        public static Resultado operator +(Resultado value1, Resultado value2)
        {
            Merge(value1, value2);
            return new Resultado((value1.Sucesso && value2.Sucesso), value1.Mensagens, value1.Id);
        }

        private static void Merge(Resultado value1, Resultado value2)
        {
            foreach (var mensagem in value2.Mensagens)
            {
                if (!value1.Mensagens.Exists(
                    delegate (Mensagem m)
                    {
                        return m.Descricao == mensagem.Descricao;
                    }))
                {
                    value1.Mensagens.Add(mensagem);
                }
            }
        }

        public static Resultado Criar(Exception ex, string metodo, string pagina)
        {
            var result = new Resultado { Sucesso = false };
            if (ex.GetType() == typeof(DbEntityValidationException))
            {
                foreach (var err in ((DbEntityValidationException)ex).EntityValidationErrors)
                {
                    foreach (var vErr in err.ValidationErrors)
                        result.Mensagens.Add(new Mensagem(vErr.PropertyName, vErr.ErrorMessage));
                }
            }
            else
            {
                var exInner = ex;
                while (exInner != null)
                {
                    if (!exInner.Message.ToLower().Contains("see the inner exception"))
                        result.Mensagens.Add(new Mensagem(exInner.Message, metodo, pagina));

                    exInner = exInner.InnerException;
                }
            }

            return result;
        }

        public static Resultado Criar(bool sucesso, string mensagem, string metodo, string pagina)
        {
            var result = new Resultado { Sucesso = sucesso };
            result.Mensagens.Add(new Mensagem(mensagem, metodo, pagina));

            return result;
        }

        public static Resultado Criar(bool sucesso, string campo, string mensagem)
        {
            var result = new Resultado { Sucesso = sucesso };
            result.Mensagens.Add(new Mensagem(campo, mensagem));

            return result;
        }


        #endregion
    }
}
