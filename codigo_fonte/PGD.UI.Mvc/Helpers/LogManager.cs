using CGU.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGD.UI.Mvc.Helpers
{
    public class LogManager
    {
        public static void LogErro(Exception ex, string infoAdicional = "")
        {
            // LogManagerComum.LogarErro(ex, null, infoAdicional);
        }
        public static void LogErro(string infoAdicional = "")
        {
            // LogManagerComum.LogarErro(null, null, infoAdicional);
        }
    }
}