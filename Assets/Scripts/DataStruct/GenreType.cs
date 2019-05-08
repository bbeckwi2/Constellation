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

    public static Color getColor(this GenreType t) {
        return colors[(int) t];
    }

}