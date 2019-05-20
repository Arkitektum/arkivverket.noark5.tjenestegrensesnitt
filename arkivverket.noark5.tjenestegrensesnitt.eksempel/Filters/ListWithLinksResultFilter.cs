using System.Collections.Generic;
using arkivverket.noark5tj.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace arkitektum.kommit.noark5.api.Controllers
{
    /// <summary>
    /// Use this attribute whenever total number of records needs to be returned in the response in order to perform paging related operations at client side.
    /// </summary>
    public class ListWithLinksResultAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result != null)
            {
                var controllerResult = (ObjectResult)context.Result;

                // only run on successful responses - not where the statusCode is explicitly set - typically bad result or something else
                if (controllerResult.StatusCode == null)
                {
                    var resultList = controllerResult.Value as IEnumerable<dynamic>;

                    var links = new List<LinkType>();
                    if (context.HttpContext.Items.ContainsKey("links"))
                    {
                        var contextLinks = (List<LinkType>) context.HttpContext.Items["links"];
                        links.AddRange(contextLinks);
                    }

                    context.Result = new OkObjectResult(new ListWithLinksResult<dynamic>(resultList, links));
                }

            }
        }
    }
}