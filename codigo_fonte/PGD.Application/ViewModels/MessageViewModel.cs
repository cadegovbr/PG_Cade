using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class MessageViewModel
    {
        public TipoMessage Tipo { get; set; }
        public string Mensagem { get; set; }
    }

    public enum TipoMessage
    {
        success = 1,
        info = 2,
        warning = 3,
        danger = 4
    }
}
