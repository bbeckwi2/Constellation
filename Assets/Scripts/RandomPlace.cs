using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlace : MonoBehaviour
{
    public GameObject nodeFab;
    public GameObject connectionFab;
    public List<GameObject> nodes;
    public int numObjects = 50;
    private RRT rTree;
    public int addDelay = 60;
    private int cDelay = 0;
    private int spawned = 0;
    // Start is called before the first frame update
    void Start() {
        rTree = new RRT();
        rTree.nodeFab = nodeFab;
        rTree.connectionFab = connectionFab;
        rTree.init();
    }

    // Update is called once per frame
    void Update() {
        if (spawned > numObjects) {
            return;
        }
        if (cDelay >= addDelay) {
            rTree.generateNode();
            cDelay = 0;
            spawned++;
        } else {
            cDelay++;
        }
    }
}
