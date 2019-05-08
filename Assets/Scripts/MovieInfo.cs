using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieInfo {
    public MovieData movies;

    public static RootObject LoadFromJSON(string filename) {
        string text = System.IO.File.ReadAllText(filename);
        Debug.Log(text);
        Debug.Log(text.Substring(text.Length - 40));
        return JsonUtility.FromJson<RootObject>(text);
    }
}


[System.Serializable]
public class MovieData {
    public string budget;
    //public Genre[] genres;
    public string homepage;
    public string id;
    //public Keyword[] keywords;
    public string original_language;
    public string original_title;
    public string overview;
    public string popularity;
    //public ProductionCompany[] production_companies;
    //public ProductionCountry[] production_countries;
    public string release_date;
    public string revenue;
    public string runtime;
    //public SpokenLanguage[] spoken_languages;
    public string status;
    public string tagline;
    public string title;
    public string vote_average;
    public string vote_count;
}

[System.Serializable]
public class Genre {
    public string id { get; set; }
    public string name { get; set; }
}

[System.Serializable]
public class Keyword {
    public string id { get; set; }
    public string name { get; set; }
}

[System.Serializable]
public class ProductionCompany {
    public string id { get; set; }
    public string name { get; set; }
}

[System.Serializable]
public class ProductionCountry {
    public string iso_3166_1 { get; set; }
    public string name { get; set; }
}

[System.Serializable]
public class SpokenLanguage {
    public string iso_639_1 { get; set; }
    public string name { get; set; }
}

[System.Serializable]
public class Movie {
    public string budget { get; set; }
    public List<Genre> genres { get; set; }
    public string homepage { get; set; }
    public string id { get; set; }
    public List<Keyword> keywords { get; set; }
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string overview { get; set; }
    public string popularity { get; set; }
    public List<ProductionCompany> production_companies { get; set; }
    public List<ProductionCountry> production_countries { get; set; }
    public string release_date { get; set; }
    public string revenue { get; set; }
    public string runtime { get; set; }
    public List<SpokenLanguage> spoken_languages { get; set; }
    public string status { get; set; }
    public string tagline { get; set; }
    public string title { get; set; }
    public string vote_average { get; set; }
    public string vote_count { get; set; }
}

[System.Serializable]
public class RootObject {
    public List<Movie> movies { get; set; }
}