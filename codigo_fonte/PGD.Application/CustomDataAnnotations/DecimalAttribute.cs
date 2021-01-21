using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.CustomDataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DecimalAttribute : DataTypeAttribute
    {
        public DecimalAttribute()
            : base("decimal")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = "Número decimal inválido";
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            decimal retNum;

            return decimal.TryParse(Convert.ToString(value), out retNum);
        }

    }
}
