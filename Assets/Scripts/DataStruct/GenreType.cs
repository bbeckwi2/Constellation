using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenreType {
    Documentary = 0,
    Crime = 1,
    History = 2,
    Family = 3,
    Mystery = 4,
    Comedy = 5,
    Animation = 6,
    War = 7,
    Thriller = 8,
    Action = 9,
    ScienceFiction = 10,
    Horror = 11,
    Adventure = 12,
    Romance = 13,
    Western = 14,
    TVMovie = 15,
    Music = 16,
    Drama = 17,
    Foreign = 18,
    Fantasy = 19,
    None = 20
}

public static class GenreTypeMethods {

    private static Color[] colors = {
        new Color(128f/255f, 128f/255f, 128f/255f), // Documentary
        new Color(255f/255f, 255f/255f, 025f/255f), // Crime
        new Color(128f/255f, 128f/255f, 000f/255f), // History
        new Color(245f/255f, 130f/255f, 048f/255f), // Family
        new Color(230f/255f, 190f/255f, 255f/255f), // Mystery
        new Color(240f/255f, 050f/255f, 230f/255f), // Comedy
        new Color(000f/255f, 130f/255f, 200f/255f), // Animation
        new Color(128f/255f, 000f/255f, 000f/255f), // War
        new Color(210f/255f, 245f/255f, 060f/255f), // Thriller
        new Color(070f/255f, 240f/255f, 240f/255f), // Action
        new Color(000f/255f, 000f/255f, 128f/255f), // Science Fiction
        new Color(230f/255f, 025f/255f, 075f/255f), // Horror 
        new Color(000f/255f, 128f/255f, 128f/255f), // Adventure
        new Color(250f/255f, 190f/255f, 190f/255f), // Romance
        new Color(170f/255f, 170f/255f, 040f/255f), // Western
        new Color(255f/255f, 250f/255f, 200f/255f), // TVMovie
        new Color(145f/255f, 030f/255f, 180f/255f), // Music  
        new Color(255f/255f, 215f/255f, 180f/255f), // Drama
        new Color(170f/255f, 255f/255f, 195f/255f), // Foriegn
        new Color(060f/255f, 180f/255f, 075f/255f), // Fantasy
        new Color(000f/255f, 000f/255f, 000f/255f), // None
    };

    private static string[] descriptions = {
        "Documentaries provide factual information about the world",
        "Crime movies involve either detectives or some aspect of crime",
        "History movies generally are based off of some historical event",
        "Family movies are appropriate for the whole family :D",
        "Mystery movies focus on solving something and generally feature investigation",
        "Comedy movies are generally funny movies that make you laugh XD",
        "Animation movies are either hand drawn or generated through Computer Graphics",
        "War movies generally focus on a war or a soldier and their struggles",
        "Thriller movies are made to get the heart pumping, can be scary",
        "Action movies generally feature crazy stunts and fast paced action",
        "Science Fiction movies deal with plausiable fiction based on science",
        "Horror movies aim to scare or make their viewers uncomfortable",
        "Adventure movies focus on exploring a far off land",
        "Romance movies focus on people building a relationship",
        "Western movies feature the Wild West and cowboys",
        "TV Movies debut on TV and are generally made for TV's format",
        "Music movies focus on either a band or a specific type of music",
        "Drama movies focus on displaying realistic characters in conflict",
        "Foriegn films come from another country",
        "Fantasy movies focus on another world and based on fiction",
        "None, this is an error. Sorry about that. Have a heart <3"
    };

    private static string[] names = {
         "Documentary",
         "Crime",
         "History",
         "Family",
         "Mystery",
         "Comedy",
         "Animation",
         "War",
         "Thriller",
         "Action",
         "Science Fiction",
         "Horror",
         "Adventure",
         "Romance",
         "Western",
         "TV Movie",
         "Music",
         "Drama",
         "Foriegn",
         "Fantasy",
         "None",
    };


    public static Color getColor(this GenreType t) {
        return colors[(int) t];
    }

    public static string getDescription(this GenreType t) {
        return descriptions[(int)t];
    }

    public static string getName(this GenreType t) {
        return names[(int)t];
    }

    public static GenreType fromString(string s) {
        switch (s) {
            case "Documentary":
                return GenreType.Documentary;
            case "Crime":
                return GenreType.Crime;
            case "History":
                return GenreType.History;
            case "Family":
                return GenreType.Family;
            case "Mystery":
                return GenreType.Mystery;
            case "Comedy":
                return GenreType.Comedy;
            case "Animation":
                return GenreType.Animation;
            case "War":
                return GenreType.War;
            case "Thriller":
                return GenreType.Thriller;
            case "Action":
                return GenreType.Action;
            case "Fantasy":
                return GenreType.ScienceFiction;
            case "Horror":
                return GenreType.Horror;
            case "Adventure":
                return GenreType.Adventure;
            case "Romance":
                return GenreType.Romance;
            case "Science Fiction":
                return GenreType.Western;
            case "TV Movie":
                return GenreType.TVMovie;
            case "Music":
                return GenreType.Music;
            case "Drama":
                return GenreType.Drama;
            case "Foreign":
                return GenreType.Foreign;
            case "Western":
                return GenreType.Fantasy;
            default:
                return GenreType.None;
        }
    }

}