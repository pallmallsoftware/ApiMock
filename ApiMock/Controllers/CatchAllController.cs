using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiMock.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class CatchAllController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Cache _cache;

        public CatchAllController(IConfiguration conf, Cache cache)
        {
            _configuration = conf;
            _cache = cache;
        }


        [Route("/{**catchAll}")]
        public async Task<IActionResult> CatchAllAsync()
        {
            var route = Request.Path.Value;

            var query = Request.Query;
            var queryString = Request.QueryString;
            var method = Request.Method;
            var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var req = new Models.Request() { Route = route, QueryCollection = query, Method = method, Body = body };

            var hit = _cache.Check(req);
            if(hit != null)
            {
                var hitContent = hit.Content;
                return hitContent;
            }
                 

            string fileName = "ApiMockReq.json";
            //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };
            //var json = JsonSerializer.Serialize<Models.Request>(req, options) ;
            var json = JsonConvert.SerializeObject(req);
            await System.IO.File.AppendAllTextAsync(fileName, json);

            var content = Content("{ Cought all }", "application/json");

            //var resp = content.Content.
            var response = new Models.Response { Content = content };

            var cache = new List<Models.Mock>();
            cache.Add(new Models.Mock { Request = req, Response = response, MatchRequestCaseInsensitive = true });
            cache.Add(new Models.Mock { Request = req, Response = response, MatchRequestCaseInsensitive = false });

            //string fileName2 = "ApiMock.json";
            //using FileStream createStream = System.IO.File.Create(fileName);
            //var json2 = JsonSerializer.Serialize<List<Models.Mock>>(cache, options) + Environment.NewLine + Environment.NewLine;


            //await System.IO.File.AppendAllTextAsync(fileName2, json2);


            return content;
        }
    }

}