using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using CRUD_API.Models;

namespace CRUD_API.Controllers
{
    public class BCSeriesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            List<BCSery> dataList = new List<BCSery>();

            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://192.9.200.73:8002/api/BCCI/");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dataList = JsonConvert.DeserializeObject<List<BCSery>>(json);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to fetch data from the API.";
                }
            }

            return View("ListBCSeries", dataList);
        } // End of the list view method


        // To Get matches based on ID
        public async Task<ActionResult> GetMatches(int id)
        {
            List<BCMatch> datalist2 = new List<BCMatch>();
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://192.9.200.73:8002/api/BCMatch/{id}/");
                HttpResponseMessage singleresponse = await client.GetAsync($"http://192.9.200.73:8002/api/BCCI/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    datalist2 = JsonConvert.DeserializeObject<List<BCMatch>>(json);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to fetch data from the API.";
                }

                if (singleresponse.IsSuccessStatusCode)
                {
                    string json2 = await singleresponse.Content.ReadAsStringAsync();
                    //  List<BCSery> records = JsonConvert.DeserializeObject<List<BCSery>>(json2);
                    BCSery record = JsonConvert.DeserializeObject<BCSery>(json2);
                    ViewBag.CurrentBCSery = record.Name;
                    ViewBag.CurrentBCSeryID = record.Id;
                }
                else
                {
                    ViewBag.CurrentBCSery = "Invalid Series";
                }

            }

            return View("ListBCMatches",datalist2);
        } // End of the List Mathches controller action method



        // GET: /BCSery/Create
        [System.Web.Mvc.HttpGet]
        public ActionResult Create()
        {
            // Return the view with an empty model
            return View(new BCSery());
        }



        // POST: /BCSery/Create
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken] // For security
        public async Task<ActionResult> Create(BCSery bCSery)
        {
            // Check if model state is valid
            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(bCSery);

                // Send POST request to API
                using (HttpClient client = new HttpClient())
                {
                    // Set base address of the API endpoint
                    client.BaseAddress = new Uri("http://192.9.200.73:8002/api/BCCI/");

                    // Set content type header
                    HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    // Send POST request and await response
                    HttpResponseMessage response = await client.PostAsync("bcsery", content);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Redirect to success page or perform other action
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Handle failure
                        ModelState.AddModelError("", "Failed to submit data to API.");
                        return View(bCSery);
                    }
                }
            }
            else
            {
                // Model state is not valid, return the view with errors
                return View(bCSery);
            }
        }// End of the post method for BCCI Series



        // Update Series method (get the forms)
        public ActionResult UpdateSeries(int Id)
        {
            ViewBag.SerieID = Id;
            return View(new BCSery());
        }

        // Update Series method for after post data Handling
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSeries(int Id, BCSery BCsery)
        {
            if (ModelState.IsValid)
            {
                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Base address of the API
                    client.BaseAddress = new Uri($"http://192.9.200.73:8002/api/BCCI/");

                    try
                    {
                        // Serialize BCMatch object to JSON
                        var jsonContent = JsonConvert.SerializeObject(BCsery);
                        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                        // Send PUT request to the API endpoint
                        var response = await client.PutAsync($"{Id}", content);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to update data via API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while sending data to API: " + ex.Message);
                    }
                }
            }
            else
            {
                ViewBag.errormessage = "Incorrect data format!";
            }

            return View("UpdateSeries", BCsery);
        } // End of the edit method for BCSeries



        // Start of the BCSeries delete method
        public async Task<ActionResult> DeleteSeries(int id)
        {
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Base address of the API
                client.BaseAddress = new Uri($"http://192.9.200.73:8002/");

                try
                {
                    // Send DELETE request to the API endpoint
                    var response = await client.DeleteAsync($"api/BCCI/{id}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to delete data via API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while sending data to API: " + ex.Message);
                }
            }

            return RedirectToAction("Index");
        }// End of the delete method for BCSeries




        // Start of the get method for create
        public ActionResult CreateMatch(int Id)
        {
            ViewBag.SerieId = Id;
            return View();
        }


        // For post request 
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMatch(BCMatch bCMatch)
        {
            if (ModelState.IsValid)
            {
                // Set the SerieId from the hidden field
                bCMatch.SerieId = Convert.ToInt32(Request.Form["SerieId"]);

                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Base address of the API
                    client.BaseAddress = new Uri("http://192.9.200.73:8002/");

                    try
                    {
                        // Serialize BCMatch object to JSON
                        var jsonContent = JsonConvert.SerializeObject(bCMatch);
                        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                        // Send POST request to the API endpoint
                        var response = await client.PostAsync("api/BCMatch", content);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to submit data to API.");
                            return View("CreateMatch", bCMatch);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while sending data to API: " + ex.Message);
                        return View("CreateMatch", bCMatch);
                    }
                }
            }

            ViewBag.SerieId = bCMatch.SerieId; // Pass the SerieId back to the view
            return View("CreateMatch", bCMatch);
        } // end of the create match action method







        // Start of the get method for update match
        public ActionResult UpdateMatch(int Id)
        {
            ViewBag.SerieId = Id;
            return View();
        }


        // Start of the edit method for BCMatch
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMatch(int Id, BCMatch bCMatch)
        {
            if (ModelState.IsValid)
            {
                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Base address of the API
                    client.BaseAddress = new Uri($"http://192.9.200.73:8002/api/BCMatch/");

                    try
                    {
                        // Serialize BCMatch object to JSON
                        var jsonContent = JsonConvert.SerializeObject(bCMatch);
                        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                        // Send PUT request to the API endpoint
                        var response = await client.PutAsync($"{Id}", content);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to update data via API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while sending data to API: " + ex.Message);
                    }
                }
            }
            else
            {
                ViewBag.errormessage = "Incorrect data format!";
            }

            return View("UpdateMatch", bCMatch);
        } // End of the edit method for BCMatch





        // Start of the BCMatch delete method
        public async Task<ActionResult> DeleteMatch(int id)
        {
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Base address of the API
                client.BaseAddress = new Uri($"http://192.9.200.73:8002/");

                try
                {
                    // Send DELETE request to the API endpoint
                    var response = await client.DeleteAsync($"api/BCMatch/{id}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to delete data via API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while sending data to API: " + ex.Message);
                }
            }

            return RedirectToAction("Index");
        }// End of the delete method for BCMatch




    }
}
