using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMock.Models
{
    public class Mock
    {
        public Request Request { get; set; }
        public Response Response { get; set; }
        public bool MatchRequestCaseInsensitive { get; set; }

    }
}
