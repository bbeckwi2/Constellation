using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlace : MonoBehaviour
{
    public GameObject nodeFab;
    public List<GameObject> nodes;
    public int numObjects = 20;
    private RRT rTree;
    public int addDelay = 60;
    private int cDelay = 0;

    // Start is called before the first frame update
    void Start() {
        rTree = new RRT();
        rTree.init();
    }

    // Update is called once per frame
    void Update() {
        if (cDelay >= addDelay) {
            rTree.generateNode(nodeFab);
            cDelay = 0;
        } else {
            cDelay++;
        }
    }
}
