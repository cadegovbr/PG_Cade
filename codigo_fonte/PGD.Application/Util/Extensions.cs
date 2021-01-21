using System;
using System.Text.RegularExpressions;

namespace PGD.Application.Util
{
    public static class Extensions
    {
        // C#
        /// <summary>
        /// Retorna apenas números
        /// </summary>
        public static string OnlyNumber(this string texto)
        {
            return !string.IsNullOrEmpty(texto) ? Regex.Replace(texto, @"[^\d]", "").ToString() : texto;
        }

        // C#
        /// <summary>
        /// Boolean IsNumber
        /// </summary>
        public static bool IsNumber(this string texto)
        {
            return Regex.IsMatch(texto, @"^\d+$");
        }

        // C#
        /// <summary>
        /// Retorna apenas Letras
        /// </summary>
        public static string OnlyLetters(this string texto)
        {
            return !string.IsNullOrEmpty(texto) ? Regex.Replace(texto, @"[^a-zA-Z]+", "").ToString() : "";
        }

        public static string OnlyAlphaNumericWithSpace(this string texto)
        {
            return !string.IsNullOrWhiteSpace(texto) ? Regex.Replace(texto, @"[^\w\s]", "") : texto;
        }

        // C#
        /// <summary>
        /// Valida E-mail com retorno boleano de Email
        /// </summary>
        public static bool IsEmail(this string texto)
        {
            return Regex.Match(texto, @"^([\w\-]+\.)*[\w\- ]+@([\w\- ]+\.)+([\w\-]{2,3})$").Success;
        }

        /// <summary>
        /// Insere máscara de CNPJ ou de CPF
        /// </summary>
        /// <param name="numCpfCnpj"></param>
        /// <returns>String</returns>
        public static string MaskCpfCpnj(this string numCpfCnpj)
        {
            if (string.IsNullOrEmpty(numCpfCnpj)) return string.Empty;

            // Se for CNPJ
            if (numCpfCnpj.Length == 14)
            {
                return numCpfCnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
            }

            // Se for CPF
            else if (numCpfCnpj.Length == 11)
            {
                return numCpfCnpj.Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            else
                return numCpfCnpj;

        }

        public static bool CpfCnpjValido(this string cpfcnpj)
        {

            int[] d = new int[14];
            int[] v = new int[2];
            int j, i, soma;

            var soNumero = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

            //verificando se todos os numeros são iguais
            if (new string(soNumero[0], soNumero.Length) == soNumero) return false;

            // se a quantidade de dígitos numérios for igual a 11
            // iremos verificar como CPF
            if (soNumero.Length != 11)
            {
                if (soNumero.Length == 14)
                {
                    var sequencia = "6543298765432";
                    for (i = 0; i <= 13; i++) d[i] = Convert.ToInt32(soNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 11 + i; j++)
                            soma += d[j] * Convert.ToInt32(sequencia.Substring(j + 1 - i, 1));

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }

                    return (v[0] == d[12] & v[1] == d[13]);
                }
            }
            else
            {
                for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(soNumero.Substring(i, 1));
                for (i = 0; i <= 1; i++)
                {
                    soma = 0;
                    for (j = 0; j <= 8 + i; j++) soma += d[j] * (10 + i - j);

                    v[i] = (soma * 10) % 11;
                    if (v[i] == 10) v[i] = 0;
                }

                return (v[0] == d[9] & v[1] == d[10]);
            }

            // CPF ou CNPJ inválido se
            // a quantidade de dígitos numérios for diferente de 11 e 14
            return false;
        }

        /// <summary>
        /// Método de Calcular idade
        /// </summary>
        /// <param name="Data de Nascimento"></param>
        /// <returns></returns>
        public static int CalcularIdade(this DateTime dtNascimento)
        {
            // Retorna o número de anos
            int yearsAge = DateTime.Now.Year - dtNascimento.Year;

            // Se a data de aniversário não ocorreu ainda este ano, subtrair um ano a partir da idade
            if (DateTime.Now.Month < dtNascimento.Month ||
                (DateTime.Now.Month == dtNascimento.Month && DateTime.Now.Day < dtNascimento.Day))
            {
                yearsAge--;
            }

            return yearsAge;
        }

        public static bool EmailValido(this string email)
        {
            Regex rg =
                new Regex(
                    @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            return rg.IsMatch(email);
        }

        public static string RemoverMaskCpfCnpj(this string cpfCnpj)
        {
            return string.IsNullOrWhiteSpace(cpfCnpj) 
             ? ""
             : cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
        }

        public static string ConverterParaHora(this double value)
        {
            var timeSpan = TimeSpan.FromHours(value);

            var horas = timeSpan.Hours;
            var minutos = timeSpan.Minutes.ToString("D2");

            if (timeSpan.Days > 0)
                horas += timeSpan.Days * 24;

            return $"{horas}:{minutos}";
        }
    }
}