using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDogIntegration
{
    class DataDogConfig
    {
        public string BaseUrl { get; set; }
        public string AuthToken { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }

        public string api_key { get; set; }
        public string app_key { get; set; }
        
    }
}
