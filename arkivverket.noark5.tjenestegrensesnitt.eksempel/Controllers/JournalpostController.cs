using System.Linq;
using arkivverket.noark5.tjenestegrensesnitt.eksempel.Services;
using arkivverket.noark5tj.models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    public class JournalpostController : ControllerBase
    {
        /// <summary>
        /// Henter tilgjengelige arkiv
        /// </summary>
        /// <returns>en liste med arkiv</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        [Route("api/sakarkiv/journalpost")]
        [EnableQuery()]
        [ListWithLinksResult]
        public IQueryable<JournalpostType> JournalposterIndex()
        {
            return MockNoarkDatalayer.Journalposter.AsQueryable();
        }


        /// <summary>
        /// Henter et arkiv med gitt id
        /// </summary>
        /// <param name="id">systemid for journalpost</param>
        /// <returns>et arkiv eller 404 hvis det ikke finnes</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        [Route("api/sakarkiv/journalpost/{id}")]
        [HttpGet]
        public ActionResult<JournalpostType> GetJournalpost(string id)
        {
            JournalpostType journalpost = MockNoarkDatalayer.GetJournalpostById(id);

            if (journalpost == null)
            {
                return NotFound();
            }
            return Ok(journalpost);
        }



    }
}
