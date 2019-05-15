﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using arkivverket.noark5tj.models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        public IQueryable JournalposterIndex()
        {
            List<ActionResult<JournalpostType>> testdata = new List<ActionResult<JournalpostType>>();

            testdata.Add(GetJournalpost(Guid.NewGuid().ToString()));
            testdata.Add(GetJournalpost(Guid.NewGuid().ToString()));
            testdata.Add(GetJournalpost(Guid.NewGuid().ToString()));
            testdata.Add(GetJournalpost(Guid.NewGuid().ToString()));
            testdata.Add(GetJournalpost(Guid.NewGuid().ToString()));

            return testdata.AsQueryable();
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
        [EnableQuery()]
        public ActionResult<JournalpostType> GetJournalpost(string id)
        {
            JournalpostType journalPost = new JournalpostType();
            journalPost.systemID = id;
            journalPost.opprettetDato = DateTime.Now;
            journalPost.opprettetDatoSpecified = true;
            journalPost.oppdatertDato = DateTime.Now;
            journalPost.journaldato = DateTime.Now;
            journalPost.tittel = "journalpost - " + journalPost.systemID;
            journalPost.oppdatertAv = "bruker";
            journalPost.LinkList.Clear();
            journalPost.RepopulateHyperMedia();

            return Ok(journalPost);
        }



    }
}