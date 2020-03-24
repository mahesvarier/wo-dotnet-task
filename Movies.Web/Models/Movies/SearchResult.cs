namespace Movies.Web.Models.Movies
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string ImageUrl { get; set; }
        public string ImdbId { get; set; }
        public MovieDetails MovieDetails { get; set; }
    }
}
