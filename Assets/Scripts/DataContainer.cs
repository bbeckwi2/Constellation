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

    void Start() {
        init();
        GameObject cS = Instantiate(constellationSpawner);
        ConstellationManager cM = cS.GetComponent<ConstellationManager>();

        NodeInfo mov;
        mov.type = NodeType.custom;
        mov.name = "Instructions";
        mov.details = "Mix and the genres to apply filters! Once you have selected the genres you wish to have simply place the new node in the world. From there take other nodes and spawn further nodes!";
        mov.genreType = GenreType.None;
        cM.init(mov);
        
        foreach (KeyValuePair<GenreType, NodeInfo> t in genreTypeNodes) {
            if (t.Key == GenreType.None) {
                continue;
            }
            cM.addNode(t.Value);
        }
    }

    /* Initialize the node */
    public void init() {
        genreTypeNodes = new Dictionary<GenreType, NodeInfo>();
        reader = new CSVReader(PATH, '~', "\""); // Both a blessing and a curse
        reader.debugCategories();
        movies = MovieInfo.generateFromReader(reader);
        foreach (GenreType gT in System.Enum.GetValues(typeof(GenreType))) {
            NodeInfo gen;
            gen.type = NodeType.genre;
            gen.name = gT.getName();
            gen.genreType = gT;
            gen.details = gT.getDescription();
            genreTypeNodes.Add(gT, gen);
        }
    }

    /* Generate a list of nodes from a movie name */
    public List<NodeInfo> fromMovie(string name) {
        List<NodeInfo> outList = new List<NodeInfo>();
        Movie m = new Movie();
        foreach (Movie mov in movies) {
            if (mov.title == name) {
                m = mov;
            }
        }

        if (m.title == null) {
            return outList;
        }

        NodeInfo info;
        info.genreType = GenreType.None;

        // Main node
        info.type = NodeType.movie;
        info.name = m.title;
        info.details = m.overview;
        outList.Add(info);

        // Genres
        foreach (GenreType gT in m.genres) {
            outList.Add(genreTypeNodes[gT]);
        }

        // Budget
        info.type = NodeType.budget;
        info.name = "Budget";
        info.details = m.budget;
        outList.Add(info);

        // Original Language
        info.type = NodeType.originalLanguage;
        info.name = "Original Language";
        info.details = m.original_language;
        outList.Add(info);

        // Popularity
        info.type = NodeType.popularity;
        info.name = "Popularity";
        info.details = m.popularity;
        outList.Add(info);

        // Production Companies
        info.type = NodeType.productionCompanies;
        info.name = "Production Company";
        foreach (string s in m.production_companies) {
            info.details = s;
            outList.Add(info);
        }

        // Release Date
        info.type = NodeType.releaseDate;
        info.name = "Release Date";
        info.details = m.release_date;
        outList.Add(info);

        // Revenue
        info.type = NodeType.revenue;
        info.name = "Revenue";
        info.details = m.revenue;
        outList.Add(info);

        // Runtime
        info.type = NodeType.runtime;
        info.name = "Runtime";
        info.details = m.runtime;
        outList.Add(info);

        // Scores
        info.type = NodeType.voteAverage;
        info.name = "Score";
        info.details = "Average: " + m.vote_average + "\nVote count: " + m.vote_count;
        outList.Add(info);

        return outList;
    }

    /* Generate a list of movies from a list of genres */
    public List<NodeInfo> fromGenres(List<GenreType> gens) {
        List<NodeInfo> outList = new List<NodeInfo>();
        int approveCount = gens.Count;
        foreach (Movie mov in movies) {
            int aCount = 0;
            foreach (GenreType g in mov.genres) {
                if (gens.Contains(g)) {
                    aCount++;
                }
            }
            if (aCount == approveCount) {
                NodeInfo n;
                n.name = mov.title;
                n.type = NodeType.movie;
                n.details = mov.overview;
                n.genreType = GenreType.None;
                outList.Add(n);
            }
        }
        return outList;
    }
}
