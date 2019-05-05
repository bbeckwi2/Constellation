using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNode : MonoBehaviour
{
    private NormalNode parent;
    private List<NormalNode> children;
    private bool isInit = false;

    public GameObject connectionFab;
    private GameObject connection;

    public void init() {
        children = new List<NormalNode>();
        isInit = true;
    }

    public void addParent(NormalNode parent) {
        this.parent = parent;

        // Connection fabrication
        connection = Instantiate(connectionFab);

        // Positions
        Vector3 gPos = this.parent.gameObject.transform.position;
        Vector3 sPos = this.gameObject.transform.position;

        // Get the transform and distance
        Transform t = connection.transform;
        float dist = Vector3.Distance(gPos, sPos);

        // Get and set the link between the stars
        t.position = Vector3.Lerp(sPos, gPos, 0.5f);
        t.localScale = new Vector3(t.localScale.x, t.localScale.y, dist);
        t.LookAt(gPos);
        
    }

    public void addChild(NormalNode child) {
        children.Add(child);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit) {
            return;
        }
    }
}
