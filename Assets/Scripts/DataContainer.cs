using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public string PATH = "Assets/CSV/movies.csv";
    private CSVReader reader;
    public GameObject constellationSpawner;
    public List<Movie> movies;
    public Dictionary<GenreType, NodeInfo> genreTypeNodes;

    // Start is called before the first frame update
    void Start() {
        reader = new CSVReader(PATH, '~', "\"");
        reader.debugCategories();
        movies = MovieInfo.generateFromReader(reader);
        Debug.Log("Movie Count:" + movies.Count);
        GameObject cS = Instantiate(constellationSpawner);
        ConstellationManager cM = cS.GetComponent<ConstellationManager>();

        genreTypeNodes = new Dictionary<GenreType, NodeInfo>();

        NodeInfo mov;
        mov.type = NodeType.custom;
        mov.name = "Instructions";
        mov.details = "Mix and the genres to apply filters! Once you have selected the genres you wish to have simply place the new node in the world. From there take other nodes and spawn further nodes!";
        mov.genreType = GenreType.None;
        cM.init(mov);
        
        foreach (GenreType gT in System.Enum.GetValues(typeof(GenreType))) {
            NodeInfo gen;
            gen.type = NodeType.genre;
            gen.name = gT.getName();
            gen.genreType = gT;
            gen.details = gT.getDescription();
            genreTypeNodes.Add(gT, gen);
            if (gT == GenreType.None) {
                continue;
            }
            cM.addNode(gen);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
