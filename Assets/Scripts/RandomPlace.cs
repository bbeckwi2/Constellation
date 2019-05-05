using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlace : MonoBehaviour
{
    public GameObject nodeFab;
    public GameObject connectionFab;
    public float BOX_SIZE = 5;
    public List<GameObject> nodes;
    public int numObjects = 50;
    private RRT rTree;
    public int addDelay = 60;
    private int cDelay = 0;
    private int spawned = 0;
    private float dying = -1f;

    // Start is called before the first frame update
    void Start() {
        rTree = new RRT();
        rTree.XMAX = BOX_SIZE;
        rTree.XMIN = -BOX_SIZE;
        rTree.YMAX = BOX_SIZE;
        rTree.YMIN = -BOX_SIZE;
        rTree.ZMAX = BOX_SIZE;
        rTree.ZMIN = -BOX_SIZE;
        rTree.START_POS = this.transform.position;
        rTree.nodeFab = nodeFab;
        rTree.connectionFab = connectionFab;
        rTree.init();
    }

    // Update is called once per frame
    void Update() {
        if (spawned > numObjects) {
            if (dying == -1f) {
                rTree.remove();
                dying = 300;
            } else if (dying > 0) {
                dying--;
            } else if (dying == 0) {
                Destroy(this.gameObject);
            }
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
