using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlace : MonoBehaviour
{
    public GameObject nodeFab;
    public List<GameObject> nodes;
    public int numObjects = 20;

    // Start is called before the first frame update
    void Start() {
        nodes = new List<GameObject>();
        
        for (int i =0; i < numObjects; i++) {
            Vector3 loc = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            GameObject t = Instantiate(nodeFab);
            ForceNode a = t.GetComponent<ForceNode>();
            a.setPos(loc);
            nodes.Add(t);
            addLayer(a, 3);
        }
    }

    void addLayer(ForceNode n, int level) {
        if (level == 0) {
            return;
        }

        for (int i = 0; i < Random.Range(0, 10); i++) {
            Vector3 loc = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            GameObject t = Instantiate(nodeFab);
            ForceNode a = t.GetComponent<ForceNode>();
            a.setParent(n);
            a.setPos(loc);
            if (Random.value < 0.5f) {
                addLayer(a, level - 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
