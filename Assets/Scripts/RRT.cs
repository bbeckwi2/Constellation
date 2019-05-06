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
            return Vector3.MoveTowards(v1, v2, nodeDist);
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

    public GameObject generateNode(GameObject prefab) {
        GameObject newNode = Instantiate(nodeFab);
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

    public GameObject generateNode(GameObject prefab, NodeInfo info) {
        GameObject node = generateNode(prefab);
        NormalNode n = node.GetComponent<NormalNode>();
        n.setInfo(info);
        return node;
    }

    public GameObject generateNode() {
        return generateNode(this.nodeFab);
    }

    public void remove() {
        nodes[0].GetComponent<NormalNode>().remove();
    }

    public void init() {
        GameObject nN = Instantiate(nodeFab);
        nN.transform.position = START_POS;
        NormalNode nNode = nN.GetComponent<NormalNode>();
        nNode.connectionFab = connectionFab;
        nNode.init();
        nodes.Add(nN);
        isInit = true;
    }

    public void init(NodeInfo info, GameObject preFab) {
        GameObject nN = Instantiate(preFab);
        nN.transform.position = START_POS;
        NormalNode nNode = nN.GetComponent<NormalNode>();
        nNode.connectionFab = connectionFab;
        nNode.init(info);
        nodes.Add(nN);
        isInit = true;
    }

    // Update is called once per frame
    void Update() {
        if (!isInit) {
            return;
        }
    }
}
