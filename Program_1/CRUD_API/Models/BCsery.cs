using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_API.Models
{
    public class BCSery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Venue { get; set; }
    }
}