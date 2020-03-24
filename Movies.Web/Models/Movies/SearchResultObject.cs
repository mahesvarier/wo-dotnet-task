using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Web.Views.Movies
{
    public class SearchResultObject
    {
        public List<Items> Search { get; set; } = new List<Items>();
        public int totalResults { get; set; }
        public bool Response { get; set; }
    }

    public class Items
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

}
