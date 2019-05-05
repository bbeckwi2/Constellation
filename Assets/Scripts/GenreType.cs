using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenreTypes {
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
    Fantasy = 19
}

public static class GenreTypesMethods {

    private static Color[] colors = {
        new Color(128f, 128f, 128f), // Documentary
        new Color(255f, 255f, 025f), // Crime
        new Color(128f, 128f, 000f), // History
        new Color(245f, 130f, 048f), // Family
        new Color(230f, 190f, 255f), // Mystery
        new Color(240f, 050f, 230f), // Comedy
        new Color(000f, 130f, 200f), // Animation
        new Color(128f, 000f, 000f), // War
        new Color(210f, 245f, 060f), // Thriller
        new Color(070f, 240f, 240f), // Action
        new Color(000f, 000f, 128f), // Science Fiction
        new Color(230f, 025f, 075f), // Horror 
        new Color(000f, 128f, 128f), // Adventure
        new Color(250f, 190f, 190f), // Romance
        new Color(170f, 170f, 040f), // Western
        new Color(255f, 250f, 200f), // TVMovie
        new Color(145f, 030f, 180f), // Music  
        new Color(255f, 215f, 180f), // Drama
        new Color(170f, 255f, 195f), // Foriegn
        new Color(060f, 180f, 075f), // Fantasy
    };


    public static Color getColor(this GenreTypes t) {
        return colors[(int) t];
    }

}