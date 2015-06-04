using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Spring.Context;
using Spring.Context.Support;

namespace Framework
{
    public class WebSpringFactory : ISpringFactory
    {
        //private readonly string _springConfigFile;

        private WebSpringFactory()
        {
            //_springConfigFile = WebConnectionManager.AppSettings["SpringConfigFilePath"];
        }

        private ISpringFactory _instance;

        public ISpringFactory Instance
        {
            get { return _instance = _instance ?? new WebSpringFactory(); }
        }
    }
}
