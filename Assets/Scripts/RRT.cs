using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRT : MonoBehaviour{

    public float XMIN = -10f;
    public float XMAX =  10f;
    public float YMIN = -10f;
    public float YMAX =  10f;
    public float ZMIN = -10f;
    public float ZMAX = 10f;

    public Vector3 START_NODE = new Vector3(0f, 0f, 0f);

    public List<Vector3> nodes = new List<Vector3>();

    public float nodeDist = 1;
    private bool isInit = false;

    /*
     * Helper function that gets the next valid location
     */
    private Vector3 stepFromTo(Vector3 v1, Vector3 v2) {
        if (Vector3.Distance(v1, v2) < nodeDist) {
            return v2;
        } else {
            return Vector3.MoveTowards(v1, v2, nodeDist);
        }
    }

    /*
     * Gets the next node position that need to be attached to the tree
     */
    Vector3 getNextNodePos() {
        Vector3 nPoint = new Vector3(Random.Range(XMIN, XMAX), Random.Range(YMIN, YMAX), Random.Range(ZMIN, ZMAX));
        Vector3 nextNode = nodes[0];
        foreach (Vector3 n in nodes){
            if (Vector3.Distance(n, nPoint) < Vector3.Distance(nextNode, nPoint)) {
                nextNode = n;
            }
        }
        return stepFromTo(nextNode, nPoint);
    }

    public GameObject generateNode(GameObject nodeFab) {
        GameObject newNode = Instantiate(nodeFab);
        Vector3 pos = getNextNodePos();
        newNode.transform.position = pos;
        nodes.Add(pos);
        return newNode;
    }

    public void init() {
        nodes.Add(START_NODE);
        isInit = true;
    }

    // Update is called once per frame
    void Update() {
        if (!isInit) {
            return;
        }
    }
}
