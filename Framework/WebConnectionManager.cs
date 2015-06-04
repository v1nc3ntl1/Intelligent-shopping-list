using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class WebConnectionManager : IConnectionManager
    {
        private string _connectionString = string.Empty;

        public string GetConnectionString(string configKey)
        {
            if (string.IsNullOrEmpty(_connectionString))
                if (ConfigurationManager.ConnectionStrings[configKey] != null)
                    _connectionString = ConfigurationManager.ConnectionStrings[configKey].ConnectionString;
            return _connectionString;
        }
    }
}
