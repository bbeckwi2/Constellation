using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContellationManager : MonoBehaviour
{
    public GameObject movieFab;
    public GameObject genreFab;
    public GameObject dataFab;

    public GameObject connectionFab;

    public float BOX_SIZE = 5;

    private List<GameObject> nodes;

    private RRT rTree;
    private float dying = -1f;
    private bool isInit = false;

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
        rTree.nodeFab = movieFab;
        rTree.connectionFab = connectionFab;
        rTree.init();
        init();
    }

    public void init() {
        isInit = true;
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
