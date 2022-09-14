using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjeHaber
{
    class Connection
    {
        public IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "{0}",
            BasePath = "{1}"
        };

        public IFirebaseClient client;
        public FirebaseResponse response;
    }
}
