using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using arkivverket.noark5.tjenestegrensesnitt.eksempel.Services;
using arkivverket.noark5tj.models;
using arkivverket.noark5tj.webapi.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;


namespace arkitektum.kommit.noark5.api.Controllers
{
    public class DokumentobjektController : ControllerBase
    {
        [Route("api/arkivstruktur/Dokumentobjekt")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<DokumentobjektType> DokumentobjekterIndex()
        {
            return MockNoarkDatalayer.Dokumentobjekter.AsQueryable();
        }

        [Route("api/arkivstruktur/Dokumentobjekt/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(DokumentobjektType), 200)]
        public ActionResult<DokumentobjektType> GetDokumentobjekt(string id)
        {
            DokumentobjektType dokumentobjekt = MockNoarkDatalayer.GetDokumentobjektById(id);

            if (dokumentobjekt == null)
            {
                return NotFound();
            }
            return Ok(dokumentobjekt);
        }

        //NY
        [Route("api/arkivstruktur/Dokumentobjekt/{id}")]
        [HttpPost]
        public HttpResponseMessage OppdaterDokumentobjekt(string id)
        {
            return null;
        }

        //NY
        [Route("api/arkivstruktur/ny-dokumentobjekt")]
        [HttpGet]
        public DokumentobjektType InitialiserDokumentobjekt(string id)
        {
            return null;
        }

        //NY
        [Route("api/arkivstruktur/ny-dokumentobjekt")]
        [HttpPost]
        public HttpResponseMessage PostDokumentobjekt(string id)
        {
            return null;
        }


        [Route("api/arkivstruktur/Registrering/{Id}/dokumentobjekt")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IEnumerable<DokumentobjektType> GetDokumentobjekterFraRegistrering(string Id)
        {
            return MockNoarkDatalayer.Dokumentobjekter.AsQueryable();
        }

        [Route("api/arkivstruktur/Registrering/{Id}/dokumentobjekt/{dokumentobjektId}")]
        [HttpGet]
        public DokumentobjektType GetDokumentobjektFraRegistrering(string Id)
        {
            return null;
        }

        [Route("api/arkivstruktur/Registrering/{Id}/dokumentobjekt/{dokumentobjektId}")]
        [HttpPost]
        public DokumentobjektType OppdaterDokumentobjektFraRegistrering(string Id)
        {
            return null;
        }

        [Route("api/arkivstruktur/Registrering/{Id}/ny-dokumentobjekt")]
        [HttpGet]
        public DokumentobjektType InitialiserNyttDokumentobjekterIRegistrering(ODataQueryOptions<DokumentobjektType> queryOptions)
        {
            return null;
        }

        [Route("api/arkivstruktur/Registrering/{Id}/ny-dokumentobjekt")]
        [HttpPost]
        [ListWithLinksResult]
        [EnableQuery()]
        public ActionResult PostDokumentobjektIRegistrering(string Id, DokumentobjektType dokumentobjekt)
        {
            if (dokumentobjekt == null)
                return BadRequest();

            return Ok(dokumentobjekt);

        }

        ////Nedlasting av fil til et Dokumentobjekt
        //[Route("api/arkivstruktur/Dokumentobjekt/{Id}/referanseFil")]
        //[HttpGet]
        //public ActionResult GetFile(string Id)
        //{
        //    string root = HttpContext.Server.MapPath("~/App_Data");
        //    var path = root + @"\eksempel.pdf";
        //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    var stream = new FileStream(path, FileMode.Open);
        //    result.Content = new StreamContent(stream);
        //    result.Content.Headers.ContentType =
        //        new MediaTypeHeaderValue("application/pdf");
        //    return result;
        //}

        ////Opplasting av fil til et Dokumentobjekt
        //[Route("api/arkivstruktur/Dokumentobjekt/{Id}/referanseFil")]
        //[HttpPost]
        //[EnableQuery()]
        //public async Task<HttpResponseMessage> UploadFile(string Id)
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (System.Exception e)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }
        //}


        [Route("api/arkivstruktur/Dokumentobjekt/{Id}/ny-referansefil")]
        [HttpGet]
        public DokumentobjektType InitialiserReferanseFilIDokumentobjekt(string Id)
        {
            return null;
        }

        //Opplasting av fil til et Dokumentobjekt
        [Route("api/arkivstruktur/Dokumentobjekt/{Id}/ny-referansefil")]
        [HttpPost]
        public HttpResponseMessage PostReferansefilIDokumentobjekt(string Id)
        {
            return null;
        }







    }
}
