using Microsoft.AspNetCore.Mvc;
using Movies.Web.Models.Movies;
using Movies.Web.Views.Movies;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace Movies.Web.Controllers
{
    public class MoviesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new SearchResult();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SearchResult data)
        {
            //Please enter the API Key
            string apiKey = "";
            
            string baseUri = $"http://www.omdbapi.com/?apikey={apiKey}";

            string name = data.Title;
            string type = data.Type;

            //Initializing page Number to be called in API
            var pageNumber = 1;
            var totalResult = 0;
            var totalPages = 1;

            SearchResults movieList = new SearchResults();

            while (pageNumber <= totalPages)
            {
                //Creating the request string
                var sb = new StringBuilder(baseUri);

                if (name != null)
                {
                    sb.Append($"&s={name}");
                }
                if (type != null)
                {
                    sb.Append($"&type={type}");
                }
                if(pageNumber > 1)
                {
                    sb.Append($"&page={pageNumber}");
                }

                var request = WebRequest.Create(sb.ToString());
                request.Timeout = 1000;
                request.Method = "GET";
                request.ContentType = "application/json";

                string result = string.Empty;

                SearchResultObject resultObject = new SearchResultObject();

                try
                {
                    using (var response = request.GetResponse())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                                //Deserializing the JSON response to Object
                                resultObject = JsonConvert.DeserializeObject<SearchResultObject>(result);
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                //Creating the List to be displayed in the details page.
                if (resultObject != null)
                {
                    foreach (var movieItem in resultObject.Search)
                    {
                        var i = new SearchResult()
                        {
                            Title = movieItem.Title,
                            ImageUrl = movieItem.Poster,
                            Year = movieItem.Year,
                            ImdbId = movieItem.imdbID,
                        };

                        i.MovieDetails = getMovieDetails(i.ImdbId);
                        totalResult = resultObject.totalResults;
                        totalPages = totalResult / 10;

                        movieList.Results.Add(i);
                        
                    }
                }
                //Incrementing the page numbers to fetch rest pages.
                pageNumber++;
            };

            //movieList.Results = movieList.Results.Distinct().ToList();
            return View("Details", movieList);
        }

        public IActionResult Details(SearchResults data)
        {
            return View(data);
        }

        public MovieDetails getMovieDetails(string movieId)
        {
            //Please enter the API key
            string apiKey = "";
            string baseUri = $"http://www.omdbapi.com/?apikey={apiKey}";

            string imdbId = movieId;

            var sb = new StringBuilder(baseUri);
            sb.Append($"&i={imdbId}");

            var request = WebRequest.Create(sb.ToString());
            request.Timeout = 1000;
            request.Method = "GET";
            request.ContentType = "application/json";

            string result = string.Empty;

            MovieDetails movieDetails = new MovieDetails();

            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                            movieDetails = JsonConvert.DeserializeObject<MovieDetails>(result);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return movieDetails;
        }
        
        public IActionResult MovieDetails(string movieId)
        {
            var movieDetails = getMovieDetails(movieId);

            return View(movieDetails);
        }
    }
}
