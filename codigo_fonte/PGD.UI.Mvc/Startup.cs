using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace PGD.UI.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
