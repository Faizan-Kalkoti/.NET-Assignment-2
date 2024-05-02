using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCCIAPI.Models;

namespace BCCIAPI.Controllers
{
    public class BCCIController : ApiController
    {
        public IEnumerable<BCSery> Get()
        {
            using (Test_DBEntities dbobj = new Test_DBEntities())
            {
                return dbobj.BCSeries.ToList();
            }
        }

        public BCSery Get(int id)
        {
            using (Test_DBEntities dbobj = new Test_DBEntities())
            {
                return dbobj.BCSeries.FirstOrDefault(e => e.Id == id);
            }
        }

        // For handling post request
        public HttpResponseMessage Post([FromBody] BCSery bobj)
        {
            using(Test_DBEntities dbobj= new Test_DBEntities())
            {
                if (bobj == null)
                {
                    // Handle the null data scenario, for example, return a BadRequest response.
                    var errorMessage = "The data provided is null.";
                    var badRequestMessage = Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return badRequestMessage;
                }

                dbobj.BCSeries.Add(bobj);
                dbobj.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, bobj);
                message.Headers.Location = new Uri(Request.RequestUri + bobj.Id.ToString());
                return message;

            }
        }

        // For handling Put request (update request)
        public HttpResponseMessage Put(int id, [FromBody] BCSery obj)
        {
            try
            {
                using (Test_DBEntities dbobj = new Test_DBEntities())
                {
                    var record = dbobj.BCSeries.FirstOrDefault(e => e.Id == id);
                    if (record == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Series with ID" + id.ToString() + "Not found!");
                    }
                    else
                    {
                        record.Name = obj.Name;
                        record.Venue = obj.Venue;
                        record.StartDate = obj.StartDate;
                        record.EndDate = obj.EndDate;

                        dbobj.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, record);

                    }
                }
            }
            catch(Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return message;
            }
        } // end of the post method



        // Delete method
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using(Test_DBEntities dbobj = new Test_DBEntities())
                {
                    var record = dbobj.BCSeries.FirstOrDefault(e => e.Id == id);
                    if (record == null)
                    {
                        var message = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Element not found!");
                        return message;
                    }
                    else
                    {
                        dbobj.BCSeries.Remove(record);
                        dbobj.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK);
                        return message;
                    }
                }
            }
            catch(Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return message;

            }
        }//end of the delete method


    }
}
