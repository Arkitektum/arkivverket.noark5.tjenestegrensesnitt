using arkivverket.noark5tj.models;

namespace arkitektum.kommit.noark5.api.Controllers
{
    public static class Set
    {
        public  const string _REL = "http://rel.arkivverket.no/noark5/v4/api";

        
        public static LinkType addLink(string baseUri, string apiUrl, string relUrl)
        {
            return new LinkType(relUrl, baseUri + apiUrl);
        }

        public static LinkType addTempLink(string baseUri, string apiUrl, string relUrl, string template)
        {
            return new LinkType(relUrl, baseUri + apiUrl + "{" + template + "}");
        }
    }
}