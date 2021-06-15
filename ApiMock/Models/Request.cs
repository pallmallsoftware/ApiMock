using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiMock.Models
{
    public class Request : IEquatable<Request>
    {
        public string Route { get; set; }
        
        //public List<KeyValuePair<string, string[]>> Query { get; set; }

        public List<KeyValuePair<string, List<string>>> Query { get; set; } 
                     

        [JsonIgnore]
        public IQueryCollection QueryCollection {
            /*get { 
                return (IQueryCollection) Query
                    .Select(x => new KeyValuePair<string, StringValues>(x.Key, x.Value.ToArray()))
                    .ToList();
            } */
            set { 
                Query = value
                    .Select(x => new KeyValuePair<string, List<string>>(x.Key, x.Value.ToList()))
                    .ToList();
            } 
        }
        //var queryString = Request.QueryString;
        public string Method { get; set; }
        public string Body { get; set; }

        public bool Equals([AllowNull] Request other)
        {
            var q1 = Query.OrderBy(t => t.Key).ToList();
            var q2 = other.Query.OrderBy(t => t.Key).ToList();

            if (q1.Count != q2.Count)
                return false;

            if (!(Route.Equals(other.Route)
                && Method.Equals(other.Method)
                && Body.Equals(other.Body)))
                return false;

            for (int i =0; i<q1.Count; i++)
            {
                var key1 = q1[i].Key;
                var key2 = q2[i].Key;

                var val1 = q1[i].Value;
                var val2 = q2[i].Value;

                var enumEq = Enumerable.SequenceEqual(val1.OrderBy(t => t), val2.OrderBy(t => t));
                var keyEq = key1 == key2;

                if (!enumEq)
                    return false;
                if (!keyEq)
                    return false;

            }
            return true;
            
        }
    }
}
