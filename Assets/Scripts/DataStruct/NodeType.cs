using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used to define the type of node */
public enum NodeType {
    movie = 0,
    budget = 1,
    genre = 2,
    productionCompany = 3,
    data = 4,
    homepage = 5,
    id = 6,
    keywords = 7,
    originalLanguage = 8,
    originalTitle = 9,
    overview = 10,
    popularity = 11,
    productionCompanies = 12,
    productionCountries = 13,
    releaseDate = 14,
    revenue = 15,
    runtime = 16,
    spokenLanguages = 17,
    status = 18,
    tagline = 19,
    voteAverage = 20,
    voteCount = 21,
    custom = 22,
    // Feel free to add more to this list. ~Brandon
}

public static class NodeTypeMethods {

    private static Color[] colors = {
        new Color(128f/255f, 128f/255f, 128f/255f), // Documentary
        new Color(255f/255f, 255f/255f, 025f/255f), // Crime
        new Color(128f/255f, 128f/255f, 000f/255f), // History
        new Color(245f/255f, 130f/255f, 048f/255f), // Family
        new Color(230f/255f, 190f/255f, 255f/255f), // Mystery
        new Color(240f/255f, 050f/255f, 230f/255f), // Comedy
        new Color(000f/255f, 130f/255f, 200f/255f), // Animation
        new Color(000f/255f, 000f/255f, 000f/255f), // None
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
        new Color(000f/255f, 000f/255f, 000f/255f), // None
        new Color(060f/255f, 180f/255f, 075f/255f), // Fantasy
        new Color(128f/255f, 000f/255f, 000f/255f), // War
        new Color(123f/255f, 1f, 1f)
    };


    public static Color getColor(this NodeType t) {
        return colors[(int) t];
    }
}