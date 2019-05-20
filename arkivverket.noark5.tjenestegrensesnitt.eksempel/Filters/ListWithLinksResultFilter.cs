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
        /*

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
                dynamic responseContent=null;

                if (actionExecutedContext.Result != null)


                    actionExecutedContext.
                //    responseContent = await actionExecutedContext.Result.ExecuteResultAsync();
                        //.Content.ReadAsAsync<dynamic>().Result;


                if (responseContent != null)
                {

                    actionExecutedContext

                    var linkList = new List<LinkType>();
                    var contextLinks = HttpContext.Items["links"];
                    if (contextLinks != null)
                    {
                        linkList.AddRange((List<LinkType>)contextLinks);
                    }

                    var res = new ListWithLinksResult<dynamic>(responseContent, linkList);
                 
                    actionExecutedContext.Response =
                        actionExecutedContext.Request.CreateResponse(actionExecutedContext.Response.StatusCode,
                            res);
                }
        }*/

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result != null)
            {
                var controllerResult = (ObjectResult)context.Result;
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