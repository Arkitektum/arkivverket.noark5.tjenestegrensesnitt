using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using arkivverket.noark5tj.models;
using Newtonsoft.Json;

namespace arkitektum.kommit.noark5.api.Controllers
{
    [XmlInclude(typeof(MappeType))]
    [DataContract]
    [JsonObject]
    public class ListWithLinksResult<T> : IEnumerable<T>
    {
        [DataMember(Name = "results")]
        public IEnumerable<T> Results { get; private set; }
        
        [DataMember(Name = "_links")]
        public IEnumerable<LinkType> Links { get; set; }

        [DataMember(Name = "count")]
        public long Count => Results?.Count() ?? 0L;

        public ListWithLinksResult (IEnumerable<T> results, IEnumerable<LinkType> links)
        {
            Results = results;
            Links = links;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T value)
        {
            throw new NotSupportedException("Add is not supported for result list.");
        }
    }
}