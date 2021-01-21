using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Util
{
    public static class Utilitarios
	{
        public static string FormatarParaHoras(double valorEmDouble)
        {
            var tsTempo = TimeSpan.FromHours((double)valorEmDouble);
            string minutes = tsTempo.Minutes < 10 ? "0" + tsTempo.Minutes : tsTempo.Minutes.ToString();
            return $"{Math.Floor(tsTempo.TotalHours)}:{minutes}";
        }

		public static string RetornaCpfCorrigido(string cpf)
		{
			var CpfCorrigido = string.Empty;
			if (!String.IsNullOrEmpty(cpf))
			{
				if (cpf.Length < 11)
					CpfCorrigido = cpf.PadLeft(11, '0');
				else

					CpfCorrigido = cpf;
			}
			return CpfCorrigido;
		}
	}
}
