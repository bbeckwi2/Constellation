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

    public GameObject nodeFab;
    public GameObject connectionFab;
    public Vector3 START_POS = new Vector3(0f, 0f, 0f);

    public List<GameObject> nodes = new List<GameObject>();
    
    private int dying = -1;

    public float nodeDist = 1;
    private bool isInit = false;

    /* Initializes the tree with a the default fab */
    public GameObject init() {
        GameObject nN = Instantiate(nodeFab);
        nN.transform.position = START_POS;
        NormalNode nNode = nN.GetComponent<NormalNode>();
        nNode.connectionFab = connectionFab;
        nNode.init();
        nodes.Add(nN);
        isInit = true;
        return nN;
    }

    /* Initializes the tee with a given fab and populates the node with info */
    public GameObject init(GameObject preFab, NodeInfo info) {
        GameObject nN = Instantiate(preFab);
        nN.transform.position = START_POS;
        NormalNode nNode = nN.GetComponent<NormalNode>();
        nNode.connectionFab = connectionFab;
        nNode.init(info);
        nodes.Add(nN);
        isInit = true;
        return nN;
    }
    
    /* Struct used to pass data from stepFromTo */
    struct NodeWithPos {
        public GameObject lastNode;
        public Vector3 nextPos;
    };

    /*
     * Helper function that gets the next valid location
     */
    private Vector3 stepFromTo(Vector3 v1, Vector3 v2) {
        if (Vector3.Distance(v1, v2) < nodeDist) {
            return v2;
        } else {
            return Vector3.MoveTowards(v1, v2, Random.Range(nodeDist, nodeDist * 2));
        }
    }

    /*
     * Gets the next node position that need to be attached to the tree
     */
    NodeWithPos getNextNodePos() {
        Vector3 nPoint = new Vector3(this.START_POS.x + Random.Range(XMIN, XMAX), this.START_POS.y + Random.Range(YMIN, YMAX), this.START_POS.z + Random.Range(ZMIN, ZMAX));
        GameObject nextNode = nodes[0];
        foreach (GameObject n in nodes){
            if (Vector3.Distance(n.transform.position, nPoint) < Vector3.Distance(nextNode.transform.position, nPoint)) {
                nextNode = n;
            }
        }
        NodeWithPos t;
        t.lastNode = nextNode;
        t.nextPos = stepFromTo(nextNode.transform.position, nPoint);
        return t;
    }

    /* Generates a node using a given prefab, returns the node GameObject */
    public GameObject generateNode(GameObject prefab) {
        GameObject newNode = Instantiate(prefab);
        NodeWithPos nP = getNextNodePos();
        newNode.transform.position = nP.nextPos;
        NormalNode cNode = newNode.GetComponent<NormalNode>();
        cNode.connectionFab = connectionFab;
        cNode.init();
        NormalNode pNode = nP.lastNode.GetComponent<NormalNode>();
        cNode.addParent(pNode);
        pNode.addChild(cNode);
        nodes.Add(newNode);
        return newNode;
    }
    
    /* Generates a node using a given prefab, set the info and returns a GameObject */
    public GameObject generateNode(GameObject prefab, NodeInfo info) {
        GameObject node = generateNode(prefab);
        NormalNode n = node.GetComponent<NormalNode>();
        n.setInfo(info);
        return node;
    }

    /* Generates a node using the default fab, returns a GameObject */
    public GameObject generateNode() {
        return generateNode(this.nodeFab);
    }

    /* Start the collapse of the tree */
    public void remove() {
        nodes[0].GetComponent<NormalNode>().remove();
    }

    // Update is called once per frame
    void Update() {
        if (!isInit) {
            return;
        }
    }
}
