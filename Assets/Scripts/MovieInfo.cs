using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovieInfo {

    public static List<Movie> generateFromReader(CSVReader reader) {
        List<Movie> movies = new List<Movie>();
        for (int i=0; i < reader.data["title"].Count; i++) {
            Movie m = new Movie();
            m.budget = reader.data["budget"][i];
            m.genres = commaSplit(reader.data["genres"][i]);
            m.homepage = reader.data["homepage"][i];
            m.id = reader.data["id"][i];
            m.keywords = commaSplit(reader.data["keywords"][i]);
            m.original_language = reader.data["original_language"][i];
            m.original_title = reader.data["original_title"][i];
            m.overview = reader.data["overview"][i];
            m.popularity = reader.data["popularity"][i];
            m.production_companies = commaSplit(reader.data["production_companies"][i]);
            m.production_countries = commaSplit(reader.data["production_countries"][i]);
            m.release_date = reader.data["release_date"][i];
            m.revenue = reader.data["revenue"][i];
            m.runtime = reader.data["runtime"][i];
            m.spoken_languages = commaSplit(reader.data["spoken_languages"][i]);
            m.status = reader.data["status"][i];
            m.tagline = reader.data["tagline"][i];
            m.title = reader.data["title"][i];
            m.vote_average = reader.data["vote_average"][i];
            m.vote_count = reader.data["vote_count"][i];
            movies.Add(m);
        }
        return movies;
    }

    private static List<string> commaSplit(string s) {
        return s.Split(',').OfType<string>().ToList();
    }
}


[System.Serializable]
public class Movie {
    public string budget { get; set; }
    public List<string> genres { get; set; }
    public string homepage { get; set; }
    public string id { get; set; }
    public List<string> keywords { get; set; }
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string overview { get; set; }
    public string popularity { get; set; }
    public List<string> production_companies { get; set; }
    public List<string> production_countries { get; set; }
    public string release_date { get; set; }
    public string revenue { get; set; }
    public string runtime { get; set; }
    public List<string> spoken_languages { get; set; }
    public string status { get; set; }
    public string tagline { get; set; }
    public string title { get; set; }
    public string vote_average { get; set; }
    public string vote_count { get; set; }
}
