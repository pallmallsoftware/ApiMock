using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text.Json;
using System.Threading.Tasks;

namespace ApiMock
{
    public class Cache
    {
        public IConfiguration _conf;
        public List<Models.Mock> Mocks { get; set; }

        public Cache(IConfiguration conf)
        {
            _conf = conf;
            var filename = conf.GetApiMockSettings().CacheFileName;
            LoadCache(filename);

        }

        public void LoadCache(string filename)
        {
            using (StreamReader r = new StreamReader(filename))
            {
                string json = File.ReadAllText(filename);
                //var mocks = JsonSerializer.Deserialize<List<Models.Mock>>(json);
                var mocks = JsonConvert.DeserializeObject<List<Models.Mock>>(json);

                mocks.ForEach(m => m.Request.Query.OrderBy(x => x.Key).ThenBy(y=>y.Value));
                
                Mocks = mocks;


            }
        }

        public Models.Response Check(Models.Request request)
        {
            //request.Query.OrderBy(x => x.Key).ThenBy(y => y.Value);

            foreach (var m in Mocks)
                if (request.Equals(m.Request))
                    return m.Response;

            return null;
        }
    }
}
