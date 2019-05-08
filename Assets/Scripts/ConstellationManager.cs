using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager : MonoBehaviour
{
    public GameObject movieFab;
    public GameObject genreFab;
    public GameObject dataFab;

    public GameObject connectionFab;

    public float BOX_SIZE = 5;

    public NormalNode mainNode;
    private RRT rTree;
    private float dying = -1f;
    private bool isInit = false;

    /* Generates the core star */
    public void init(NodeInfo info, GameObject fab, float size) {
        rTree = new RRT();
        rTree.XMAX = BOX_SIZE;
        rTree.XMIN = -BOX_SIZE;
        rTree.YMAX = BOX_SIZE;
        rTree.YMIN = -BOX_SIZE;
        rTree.ZMAX = BOX_SIZE;
        rTree.ZMIN = -BOX_SIZE;
        rTree.START_POS = this.transform.position;
        rTree.nodeFab = fab;
        rTree.connectionFab = connectionFab;
        mainNode = rTree.init(fab, info).GetComponent<NormalNode>();
        mainNode.size = size;
        isInit = true;
    }

    public void init(NodeInfo info, GameObject fab) {
        init(info, fab, 0.75f);
    }

    public void init(NodeInfo info) {
        init(info, getFab(info), 0.75f);
    }
   
    /* Gets the prefab for the specific type */
    private GameObject getFab(NodeInfo info) {
        switch (info.type) {
            case NodeType.movie:
                return movieFab;
            case NodeType.genre:
                return genreFab;
            default:
                return dataFab;
        }
    }
    
    /* Adds a node, the node shape is determined by the info */
    public void addNode(NodeInfo info) {
        GameObject n = rTree.generateNode(getFab(info), info);
        if (info.type == NodeType.genre) {
            Color c = info.genreType.getColor();
            n.GetComponent<NormalNode>().setColor(c);
        }
    }

    /* The death of the stars */
    public void remove() {
        this.dying = 300;
        rTree.remove();
    }

    // Update is called once per frame
    void Update() {
        
        if (!isInit) {
            return;
        }
        if (dying > 0) {
            dying--;
        } else if (dying == 0) {
            Destroy(this.gameObject);
        }
    }
}
