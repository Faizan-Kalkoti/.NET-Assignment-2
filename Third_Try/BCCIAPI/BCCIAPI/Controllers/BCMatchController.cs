using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCCIAPI.Models;

namespace BCCIAPI.Controllers
{
    public class BCMatchController : ApiController
    {
        // To get data from BCMatches that match the id (Fk)
        public HttpResponseMessage Get(int id)
        {
            if (id <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "A valid foreign key ID is required.");
            }

            // Filter records based on foreign key ID
            using (Test_DBEntities2 dbobj = new Test_DBEntities2())
            {
                var filteredRecords = dbobj.BCMatches.Where(record => record.SerieId == id).ToList();
                if (filteredRecords.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, filteredRecords);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No records found for the provided foreign key ID.");
                }
            }
        }//end of the get filter method


        public HttpResponseMessage Post([FromBody] BCMatch obj)
        {
            try
            {
                if(obj == null)
                {
                    var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No element passed!");
                    return message;
                }
                using(Test_DBEntities2 dbobj = new Test_DBEntities2())
                {
                    dbobj.BCMatches.Add(obj);
                    dbobj.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.OK, "Successfully created!");
                    message.Headers.Location = new Uri(Request.RequestUri + obj.Id.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unexpected Error Occurred! - " +
                    "Please check if the foreign key record exists for the given SerieID");
                return message;
            }
        }//end of the post method




        // Start of the Put Method
        public HttpResponseMessage Put(int id, [FromBody] BCMatch obj)
        {
            try
            {
                using (Test_DBEntities2 dbobj = new Test_DBEntities2())
                {

                    var record = dbobj.BCMatches.FirstOrDefault(e => e.Id == id);
                    if (record == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Matches with ID" + id.ToString() + "Not found!");
                    }
                    else
                    {
                        record.SerieId = obj.SerieId;
                        record.Opponent = obj.Opponent;
                        record.MatchDate = obj.MatchDate;

                        dbobj.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, record);
                    }
                }
            }
            catch(Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
                return message;
            }
        }// end of the Put Method



        // Delete method
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (Test_DBEntities2 dbobj = new Test_DBEntities2())
                {
                    var record = dbobj.BCMatches.FirstOrDefault(e => e.Id == id);
                    if (record == null)
                    {
                        var message = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Element not found!");
                        return message;
                    }
                    else
                    {
                        dbobj.BCMatches.Remove(record);
                        dbobj.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK);
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return message;

            }
        }//end of the delete method



    } 
}
