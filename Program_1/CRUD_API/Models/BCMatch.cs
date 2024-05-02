using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_API.Models
{
    public class BCMatch
    {
        public int Id { get; set; }
        public Nullable<int> SerieId { get; set; }
        public string Opponent { get; set; }
        public System.DateTime MatchDate { get; set; }
    }
}