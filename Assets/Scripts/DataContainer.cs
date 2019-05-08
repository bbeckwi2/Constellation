using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public string PATH = "Assets/CSV/movies.csv";
    private CSVReader reader;
    public GameObject constellationSpawner;
    public List<Movie> movies;

    // Start is called before the first frame update
    void Start() {
        reader = new CSVReader(PATH, '~', "\"");
        reader.debugCategories();
        movies = MovieInfo.generateFromReader(reader);
        Debug.Log("Movie Count:" + movies.Count);
        GameObject cS = Instantiate(constellationSpawner);
        ConstellationManager cM = cS.GetComponent<ConstellationManager>();
        NodeInfo gen;
        gen.type = NodeType.genre;
        gen.name = "Test";
        gen.genreType = GenreType.Action;
        gen.details = "";

        NodeInfo mov;
        mov.type = NodeType.movie;
        mov.name = "Movie";
        mov.details = "";
        mov.genreType = GenreType.None;
        cM.init(mov);

        System.Array values = System.Enum.GetValues(typeof(GenreType));
        System.Random random = new System.Random();

        for (int i=0; i < 20; i++) {
            gen.genreType = (GenreType)values.GetValue(random.Next(values.Length));
            cM.addNode(gen);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
