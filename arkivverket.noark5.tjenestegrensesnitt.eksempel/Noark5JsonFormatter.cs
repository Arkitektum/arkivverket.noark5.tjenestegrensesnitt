using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace arkivverket.noark5tj.webapi
{
    public class Noark5JsonFormatter : JsonOutputFormatter
    {
        public Noark5JsonFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool) : base(serializerSettings, charPool)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd.noark5-v4+json"));
        }
    }
}
